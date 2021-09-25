using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public class GridCell
{
    public Vector2i GridPos { get; private set; }

    private TileType floorTile;
    private TileType midTile;

    public TileType FloorTile
    {
        get => floorTile;
        set
        {
            floorTile = value;
            Main.map?.FloorTiles.SetCellv(GridPos.Vec2(), (int)floorTile);
        }
    }
    public TileType MidTile
    {
        get => midTile;
        set
        {
            midTile = value;
            Main.map?.MidTiles.SetCellv(GridPos.Vec2(), (int)midTile);
        }
    }

    public GridCell(Vector2i _gridPos, TileType _floorTile = TileType.Grass, TileType _midTile = TileType.Empty)
    {
        GridPos = _gridPos;
        FloorTile = _floorTile;
        MidTile = _midTile;
    }
}
