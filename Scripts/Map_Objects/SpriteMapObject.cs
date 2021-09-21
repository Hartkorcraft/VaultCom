using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;


public class SpriteMapObject : MapObject, IMouseable
{
    public Area2D area2D { get; private set; }
    public HashSet<Map.TileType> blockingMovement { get; set; } = new HashSet<Map.TileType>();


    #region  ENTER_TREE READY ETC.

    public override void _EnterTree()
    {
        base._EnterTree();
        area2D = (Area2D)GetNode("Area2D");
    }

    public override void _Ready()
    {
        base._Ready();
    }

    public override void _PhysicsProcess(float delta)
    {
        Transition_Position_Lerp(Transition_Speed);
    }

    #endregion

    #region MOVEABLE 
    private List<Vector2i> transition_positions = new List<Vector2i>();
    [Export] private float Transition_Speed = 0.3f;

    private bool transitioning;
    public bool Transitioning
    {
        get { return transitioning; }
        set { transitioning = value; }
    }

    protected void Transition_Position_Lerp(float smooth = 0.3f, float clip_range = 1f)
    {
        if (transition_positions.Count > 0)
        {
            var new_grid_pos = (Vector2i)transition_positions[0];
            Vector2 new_pos = new_grid_pos.Vec2() * Main.TILE_SIZE;
            if (Mathf.Abs(Position.x - new_pos.x) <= clip_range && Mathf.Abs(Position.y - new_pos.y) <= clip_range)
            {
                transition_positions.RemoveAt(0);
                GridPos = new_grid_pos;
                if (transition_positions.Count <= 0) { Main.game_Manager.EndSpriteMapObjectTransition(this); }
            }
            Position = Lerp(Position, new_pos, smooth);
        }
    }

    public void MoveOnPath(List<Vector2i> positions)
    {
        Main.game_Manager.SetState(new TransitionState());
        transition_positions.AddRange(positions);
    }

    #endregion

    #region  IMOUSEABLE 
    public virtual void _on_Area2D_input_event(Node viepoint, InputEvent inputEvent, int local_shape)
    {
        if (inputEvent is InputEventMouseButton)
        {
            if (inputEvent.IsPressed())
            {
                if (inputEvent.IsActionPressed("Left_Mouse"))
                {
                    On_Left_Mouse_Click();
                }
                else if (inputEvent.IsActionPressed("Right_Mouse"))
                {
                    On_Right_Mouse_Click();
                }

                //if (CanSelect) { Game_Manager.Select(this, true); }
            }
        }
    }


    public virtual void On_Left_Mouse_Click()
    {
        GD.Print("Clicked on map object LB");
    }
    public virtual void On_Right_Mouse_Click()
    {
        GD.Print("Clicked on map object RB");
    }

    public virtual void _on_Area2D_mouse_entered()
    {
        // GD.Print("Mouse entered map object");
        Main.game_Manager.AddMouseOverObject(this);
        Main.debug_Manager.UpdateLog("Map_Object_under_mouse", Main.game_Manager.GetMouseOverHashSetString());
    }
    public virtual void _on_Area2D_mouse_exited()
    {
        Main.game_Manager.RemoveMouseOverObject(this);
        Main.debug_Manager.UpdateLog("Map_Object_under_mouse", Main.game_Manager.GetMouseOverHashSetString());
        // GD.Print("Mouse exited map object");
    }
    #endregion
}
