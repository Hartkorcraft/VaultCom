using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public class PlayerCharacter : Entity, ISelectable, IContextable
{

    Vector2i lastMouseGridPos;

    #region ENTER_TREE READY INPUT ETC
    //@ ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    public override void _EnterTree()
    {
        base._EnterTree();
        GridPos = new Vector2i(Position / new Vector2(Main.TILE_SIZE, Main.TILE_SIZE));
    }
    //@ ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    public override void _ExitTree()
    {
        base._ExitTree();
    }
    //@ ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    public override void _Ready()
    {
        base._Ready();
        GD.Print("Spawned at: " + Position);
    }
    //@ ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    public override void _Process(float delta)
    {
        base._Process(delta);
        if (Main.game_Manager?.AllowWorldInput is true)
        {
            //Pathfinding
            if (Main.game_Manager?.CurrentSelection == this && Main.game_Manager?.Mouseover.Count == 0 && MovementPoints > 0 && (!lastMouseGridPos.Equals(Main.Mouse_Grid_Pos) || path_positions_cache.Count == 0))
            {
                Main.map?.PathfindingTiles.Clear();
                var path = Main.map?.PathFinding.FindPath(GridPos, Main.Mouse_Grid_Pos, blocking_movement, collider_layer: 1, diagonals: true, big: false);

                if (path != null && path.Count > 0)
                {
                    path_positions_cache.Clear();
                    var distance = 0;

                    //*drawig path
                    foreach (var pathTile in path)
                    {
                        if (distance < MovementPoints)
                        {
                            Main.map?.PathfindingTiles.SetCellv(pathTile.GridPos.Vec2(), (int)Map.TileType.Green_Dot);
                            path_positions_cache.Add(pathTile.GridPos);
                        }
                        else
                        {
                            Main.map?.PathfindingTiles.SetCellv(pathTile.GridPos.Vec2(), (int)Map.TileType.Red_Dot);
                        }
                        distance++;
                    }
                }
            }
        }
        lastMouseGridPos = Main.Mouse_Grid_Pos;
    }
    //@ ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    public override void _Input(InputEvent inputEvent)
    {
        //*Movement
        if (inputEvent.IsActionPressed("Left_Mouse") && Main.game_Manager?.AllowWorldInput is true)
        {
            if (path_positions_cache.Count > 0 && path_positions_cache[path_positions_cache.Count - 1].Equals(Main.Mouse_Grid_Pos))
            {
                MovementPoints -= path_positions_cache.Count;
                MoveOnPath(path_positions_cache);
                path_positions_cache.Clear();
            }
        }
    }
    //@ ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    #endregion

    // void Setup_TwoPointLine()
    // {
    //     Func<Vector2> get_Line_End_Point = () => { return GetLocalMousePosition(); };
    //     twoPointLine = new TwoPointLine(get_Line_End_Point);
    //     AddChild(twoPointLine);
    // }

    #region  IMOUSEABLE 
    public override void On_Left_Mouse_Click()
    {
        base.On_Left_Mouse_Click();
        if (Main.game_Manager?.AllowWorldInput is true) // Main.game_Manager.ContextMenuSafeCheck)
        {
            Main.game_Manager?.Select(this, true);
        }
    }
    public override void On_Right_Mouse_Click()
    {
        base.On_Right_Mouse_Click();
    }
    #endregion

    #region ISELECTABLE
    public bool CanSelect { get; set; } = true;
    public virtual void HandleSelection()
    {

    }
    public virtual void HandleBeingSelected()
    {
        Main.map?.UpdateHightligthDisplay(posible_positions_tile_cache);
    }
    public virtual void HandleBeingUnselected()
    {
        Main.map?.UpdateHightligthDisplay(null);
    }
    #endregion

    #region IContextable
    public void Act_On_Context_Selection(ContextMenu popupMenu)
    {
        GD.Print("Acted on context selection!");

        // var popupBoxScene = (PackedScene)ResourceLoader.Load("res://Scenes/PopupInfoWindow.tscn");
        // PopupInfoWindow popupBox = (PopupInfoWindow)popupBoxScene.Instance();
        // Main.ui.Opened_UI.AddChild(popupBox);

        // popupBox.PopupCentered();
        popupMenu.Close();
    }
    #endregion

    public override void AddITurnableToGameManager()
    {
        Main.game_Manager?.PlayerTurnObjects.Add(this);
    }
}