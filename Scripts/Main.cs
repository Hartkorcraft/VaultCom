using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public class Main : Node2D
{
    public static Main main { get; private set; }
    public static Map map { get; private set; }
    public static Debug_Manager debug_Manager { get; private set; }
    public static Game_Manager game_Manager { get; private set; }
    public static ContextMenu context_menu { get; private set; }
    public static UI ui { get; private set; }

    [Export] Vector2 initMapSize = new Vector2(5, 5);
    public Vector2i InitMapSize
    {
        get { return new Vector2i(initMapSize); }
        set { initMapSize.x = value.x; initMapSize.y = value.y; }
    }

    public static Vector2 Mouse_Pos { get { return main.GetGlobalMousePosition(); } }
    public static Vector2i Mouse_Grid_Pos { get { return MouseToCart(Mouse_Pos, Main.map.FloorTiles); } }
    public static Vector2i Last_Clicked_Grid_Pos { get; private set; } = new Vector2i(-1, -1);
    public static int TILE_SIZE { get; private set; }
    
    // public static CollisionObject2D Get_Collider_From_Pos(Vector2 pos)
    // {
    //     Godot.Collections.Array result = main.GetWorld2d().DirectSpaceState.IntersectPoint(pos, 32, null, 2147483647, true, true);

    //     if (result.Count > 0)
    //     {
    //         Godot.Collections.Dictionary dic = (Godot.Collections.Dictionary)result[0];
    //         return (CollisionObject2D)((Godot.Collections.Dictionary)result[0])["collider"];
    //     }
    //     return null;
    // }

    public static List<Godot.Collections.Dictionary> Get_Collider_Dicts_From_GridPos(Vector2i _pos, uint layer = 2147483647)
    {
        var pos = (_pos.Vec2() * TILE_SIZE) + new Vector2((float)TILE_SIZE / 2, (float)TILE_SIZE / 2);

        Godot.Collections.Array result = main.GetWorld2d().DirectSpaceState.IntersectPoint(pos, 32, null, layer, true, true);

        //Godot.Collections.Dictionary dic = (Godot.Collections.Dictionary)result[0];
        var list = new List<Godot.Collections.Dictionary>();
        foreach (Godot.Collections.Dictionary collider_dict in result)
        {
            list.Add(collider_dict);
        }

        return list;
    }

    #region  ENTER READY PROCESS
    public override void _EnterTree()
    {
        var Tiles = GetNode("Tiles");

        main = this;
        debug_Manager = (Debug_Manager)GetNode("DebugUI").GetNode("Debug_Manager");
        game_Manager = (Game_Manager)GetNode("Game_Manager");
        ui = (UI)GetNode("UI");
        context_menu = (ContextMenu)ui.GetNode("ContextMenu");
        
        map = new Map(
        new Vector2ui(InitMapSize)
        );

        TILE_SIZE = (int)map.FloorTiles.CellSize.x;
    }

    public override void _Ready()
    {
        map.InitGridMap(map.MapSize, false);

        //Main
        debug_Manager.AddLog(new DebugInfo("Mouse_and_Last_Click_Pos"));
        debug_Manager.AddLog(new DebugInfo("Tiles_under_mouse"));

        //SpriteMapObject
        debug_Manager.AddLog(new DebugInfo("Map_Object_under_mouse"));
    }

    public override void _Process(float delta)
    {
        debug_Manager.UpdateLog("Mouse_and_Last_Click_Pos", Mouse_Grid_Pos.ToString() + " | " + Mouse_Pos.ToString() + " | " + Last_Clicked_Grid_Pos.ToString());
        debug_Manager.UpdateLog("Tiles_under_mouse",
        "Floor: " + map.GetTileType(Mouse_Grid_Pos, map.FloorTiles).ToString() + " | " +
        "Mid: " + map.GetTileType(Mouse_Grid_Pos, map.MidTiles).ToString());
    }



    #endregion

    #region INPUT 
    public override void _Input(InputEvent inputEvent)
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
    
    public void On_Left_Mouse_Click()
    {
        Last_Clicked_Grid_Pos = Mouse_Grid_Pos;
    }
    public void On_Right_Mouse_Click()
    {

    }


    #endregion
}
