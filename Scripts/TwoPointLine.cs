using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public class TwoPointLine : Line2D
{

    Func<Vector2> get_Line_End_Point;
    public override void _EnterTree()
    {
        Setup_Line2D();
    }
    public override void _Process(float delta)
    {
        base._Process(delta);
        
        SetPointPosition(0, (new Vector2(0, 0) + new Vector2(Main.TILE_SIZE / 2, Main.TILE_SIZE / 2)));
        SetPointPosition(1, get_Line_End_Point());
        //line2D.Update();
    }

    private void Setup_Line2D()
    {
        AddPoint(get_Line_End_Point());
        AddPoint(-Position);

        DefaultColor = new Color("#e22828");
        Width = 1;
    }

    public TwoPointLine(Func<Vector2> _get_Line_End_Point)
    {
        get_Line_End_Point = _get_Line_End_Point;
    }
}
