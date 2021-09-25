using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public class Map
{
    public TileMap FloorTiles { get; private set; }
    public TileMap MidTiles { get; private set; }
    public TileMap PathfindingTiles { get; private set; }
    public TileMap HighlightTiles { get; private set; }
    public Vector2ui MapSize { get; private set; } = new Vector2ui(10, 10);
    public PathFinding<TileType> PathFinding { get; private set; }

    private GridCell[,] gridMap;

    public bool OnMap(Vector2i pos) => CheckIfInRange(pos, MapSize.Vec2i());

    public GridCell GetGridCell(Vector2i gridPos)
    {
        if (OnMap(gridPos)) { return gridMap[gridPos.x, gridPos.y]; }
        return null;
    }

    public TileType GetTileType(Vector2i gridPos, TileMap tileMap) => (TileType)tileMap.GetCellv(gridPos.Vec2());

    public void Update_Higthlight_Display(List<Vector2i> positions, TileType tileType = TileType.Transparent_Green)
    {
        Main.map.HighlightTiles.Clear();
        if (positions != null)
        {
            foreach (var possible_tile in positions) { Main.map.HighlightTiles.SetCellv(possible_tile.Vec2(), (int)tileType); }
        }
    }
    public void InitGridMap(Vector2ui _MapSize, bool clear = true)
    {
        if (clear)
        {
            FloorTiles.Clear();
            MidTiles.Clear();
        }

        gridMap = new GridCell[_MapSize.x, _MapSize.y];

        for (int y = 0; y < _MapSize.y; y++)
        {
            for (int x = 0; x < _MapSize.x; x++)
            {
                var floorTileType = (TileType)FloorTiles.GetCellv(new Vector2(x, y));
                var midTileType = (TileType)MidTiles.GetCellv(new Vector2(x, y));
                if (floorTileType is TileType.Empty) { floorTileType = TileType.Grass; }
                gridMap[x, y] = new GridCell(new Vector2i(x, y), floorTileType, midTileType);
            }
        }
    }

    public Map(Vector2ui _mapSize)
    {
        var Tiles = Main.main.GetNode("Tiles");

        FloorTiles = (TileMap)Tiles.GetNode("Floor_Tiles");
        MidTiles = (TileMap)Tiles.GetNode("Mid_Tiles");
        PathfindingTiles = (TileMap)Tiles.GetNode("Pathfinding_Tiles");
        HighlightTiles = (TileMap)Tiles.GetNode("Highlight_Tiles");
        MapSize = _mapSize;

        Func<Vector2i, HashSet<TileType>, bool> checkForBlocking = (pos, blocking) =>
        {
            return
            blocking.Contains((TileType)MidTiles.GetCellv(pos.Vec2()));
        };

        Func<Vector2i, int, bool> check_for_colliders = (pos, layer) => { return Main.Get_Collider_Dicts_From_GridPos(pos, 1).Count > 0; };

        PathFinding = new PathFinding<TileType>(new Vector2i(_mapSize), checkForBlocking, check_for_colliders);
    }
}