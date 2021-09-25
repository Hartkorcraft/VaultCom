using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;


public class SpriteMapObject : MapObject, IMouseable
{
    public static event Action<SpriteMapObject> end_transition_event;
    public static event Action<GameState> moving_on_path_event;
    public static event Action<SpriteMapObject> mouse_enter_over_event;
    public static event Action<SpriteMapObject> mouse_exit_over_event;

    public bool Transitioning { get => transitioning; set => transitioning = value; }
    public HashSet<TileType> blocking_movement { get; protected set; } = new HashSet<TileType>();
    public Area2D area2D { get; private set; } //IMouseable

    [Export] private float Transition_Speed = 0.3f;

    private List<Vector2i> transition_positions = new List<Vector2i>(); // Positions for moving on a path
    private bool transitioning;


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

    public bool InClipRange(Vector2 new_pos, float clip_range) => (Mathf.Abs(Position.x - new_pos.x) <= clip_range && Mathf.Abs(Position.y - new_pos.y) <= clip_range);

    protected void Transition_Position_Lerp(float smooth = 0.3f, float clip_range = 1f)
    {
        if (transition_positions.Count > 0)
        {
            var new_grid_pos = (Vector2i)transition_positions[0];
            Vector2 new_pos = new_grid_pos.Vec2() * Main.TILE_SIZE;
            if (InClipRange(new_pos, clip_range))
            {
                transition_positions.RemoveAt(0);
                GridPos = new_grid_pos;
                if (transition_positions.Count <= 0) { EndTransition(); }
            }
            Position = Lerp(Position, new_pos, smooth);
        }
    }

    protected virtual void EndTransition()
    {
        end_transition_event?.Invoke(this);
    }

    public void MoveOnPath(List<Vector2i> positions)
    {
        moving_on_path_event?.Invoke(new TransitionState());
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
            }
        }
    }

    public virtual void On_Left_Mouse_Click() { }
    public virtual void On_Right_Mouse_Click() { }

    public virtual void _on_Area2D_mouse_entered()
    {
        mouse_enter_over_event?.Invoke(this);
        Main.debug_Manager?.UpdateLog("Map_Object_under_mouse", Main.game_Manager.GetMouseOverHashSetString(), true);
    }

    public virtual void _on_Area2D_mouse_exited()
    {
        mouse_exit_over_event?.Invoke(this);
        Main.debug_Manager?.UpdateLog("Map_Object_under_mouse", Main.game_Manager.GetMouseOverHashSetString(), true);
    }
    #endregion
}
