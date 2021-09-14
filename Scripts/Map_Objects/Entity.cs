using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public abstract class Entity : SpriteMapObject
{
    public int MovementPoints { get; set; } = 3;
    protected List<Vector2i> path_positions_cache = new List<Vector2i>();

}
