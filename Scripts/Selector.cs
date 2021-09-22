using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;


public class Selector : Sprite
{
    [Export] public float MoveSpeed { get; set; } = 0.3f;

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        if (this.Visible && Main.game_Manager.AllowWorldInput)
        {
            Position = Lerp(Position, (Main.Mouse_Grid_Pos.Vec2() * Main.TILE_SIZE) - new Vector2(1, 1), MoveSpeed);
        }
    }


}
