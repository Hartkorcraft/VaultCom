using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public class PlayerCharacter : Entity, ISelectable, IContextable, IGetInfoable
{
    public static int number_of_player_characters { get; private set; } = 0;
    public Vector2i lastMouseGridPos { get; private set; }
    private PlayerActivityBase current_action;
    public PlayerActivityBase Default_Action { get; private set; }
    public PlayerActivityBase Current_Action
    {
        get { return current_action; }
        private set
        {
            current_action = value;
            current_action.Start(this);
        }
    }

    public static PlayerFindingPathActivity player_finding_path_activity = new PlayerFindingPathActivity();
    public static PlayerPrimaryActivity player_primary_activity = new PlayerPrimaryActivity();
    public static PlayerIdleActivity player_idle_activity = new PlayerIdleActivity();

    #region ENTER_TREE READY INPUT ETC
    public override void _EnterTree()
    {
        base._EnterTree();
        number_of_player_characters++;
        AddIGetInfoableToGameManager();
        GridPos = new Vector2i(Position / new Vector2(Main.TILE_SIZE, Main.TILE_SIZE));
        Default_Action = player_finding_path_activity;
        UI.primary_button_pressed += TryDoPrimaryAction;
        ObjectName += $"_{number_of_player_characters}";
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        number_of_player_characters--;
        UI.primary_button_pressed -= TryDoPrimaryAction;
    }
    public override void _Ready()
    {
        base._Ready();
        Current_Action = Default_Action;
        GD.Print("Spawned at: " + Position);
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        if (Main.game_Manager?.AllowWorldInput is true)
        {
            if (Game_Manager.CurrentSelection == this && Current_Action != null)
            {
                Current_Action.DoPlayerAction();
            }
        }
        lastMouseGridPos = Main.Mouse_Grid_Pos;
    }

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
    #endregion

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
        Current_Action = Default_Action;
        Current_Action.UpdateDisplay();
        //Main.map?.UpdateHightligthDisplay(posible_positions_tile_cache);
    }

    public virtual void HandleBeingSelected()
    {

    }

    public virtual void HandleUnselection()
    {
        Current_Action.ClearDisplay();
        Current_Action = player_idle_activity;
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

    public override void StartTurn()
    {
        base.StartTurn();
        Current_Action = player_idle_activity;
    }

    public void TryDoPrimaryAction()
    {
        if (Game_Manager.CurrentSelection == this)
        {
            if (Current_Action as PlayerPrimaryActivity is null)
            {
                Current_Action = player_primary_activity;
            }
            Current_Action?.UpdateDisplay();
        }
    }

    protected override void EndTransition()
    {
        base.EndTransition();
        Current_Action.UpdateDisplay();
    }

    public override void AddITurnableToGameManager()
    {
        Game_Manager.PlayerTurnObjects?.Add(this);
    }

    public string GetInfo()
    {
        return
        $@"-Name: {ObjectName}
        -Current_Action: {current_action?.ToString()}
        -GridPos: {GridPos.ToString()}
        -Health and Cap: {Health} {HealthCap}
        -Movement points and Cap: {MovementPoints} {MovementPointsCap}";
    }

    public void AddIGetInfoableToGameManager()
    {
        Game_Manager.infoObjects?.Add(this);
    }
}
