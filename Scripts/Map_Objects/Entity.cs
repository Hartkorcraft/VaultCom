using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public abstract class Entity : SpriteMapObject, ITurnable
{
    public int MovementPointsCap { get; set; } = 6;
    public int MovementPoints { get; set; } = 6;
    protected List<Vector2i> path_positions_cache = new List<Vector2i>();

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

    public abstract void AddITurnableToGameManager();
}
