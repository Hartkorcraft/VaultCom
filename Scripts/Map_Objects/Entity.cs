using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public abstract class Entity : SpriteMapObject, ITurnable
{
    public int MovementPointsCap { get; set; } = 9;
    public int MovementPoints { get; set; } = 9;
    protected List<Vector2i> path_positions_cache = new List<Vector2i>();
    protected List<PathFindingCell<Map.TileType>> posible_positions_tile_cache = new List<PathFindingCell<Map.TileType>>();
    public void ResetMovementPoints() { MovementPoints = MovementPointsCap; }

    public override void _EnterTree()
    {
        base._EnterTree();
        AddITurnableToGameManager();
    }
    public virtual void StartTurn()
    {
        ResetMovementPoints();

    }

    public virtual void CalculatePossiblePositions()
    {
        posible_positions_tile_cache = Main.map.PathFinding.FindPossibleSpaces(GridPos, MovementPoints, blockingMovement, 1);

    }
    public void UpdateTurnObject()
    {
        CalculatePossiblePositions();
    }
    public abstract void AddITurnableToGameManager();
}
