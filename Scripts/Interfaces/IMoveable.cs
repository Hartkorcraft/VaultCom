using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public interface IMoveable
{
    public List<Vector2i> path_positions_cache { get; }
    // public List<Vector2i> posible_positions_tile_cache { get; }
    // public void CalculatePossiblePositions();

}
