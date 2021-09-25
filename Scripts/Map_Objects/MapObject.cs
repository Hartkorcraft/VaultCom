using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;
//using static GlobalConstants;

public abstract class MapObject : Node2D, IHealth, INameable
{
    public string ObjectName { get; protected set; } = "Default_name";
    public int HealthCap { get; set; } = 5;
    public int Health { get; set; } = 1;

    protected Vector2i gridPos;

    public virtual Vector2i GridPos
    {
        get => gridPos;
        set
        {
            Position = value.Vec2() * new Vector2(Main.TILE_SIZE, Main.TILE_SIZE);
            gridPos = value;
        }
    }



    #region IHEALTH
    public virtual void Damage(int dmg)
    {
        if (Health - dmg <= 0)
        {
            Kill();
        }
        else
        {
            Health -= dmg;
        }
        GD.Print("Damaged: ", this.ToString(), " ", dmg, " New health: ", Health);
    }

    public virtual void Heal(int health)
    {
        if (Health + health <= HealthCap)
        {
            Health += health;
            GD.Print("Healed: ", this.ToString(), " ", health, " New health: ", Health);
        }
    }

    public virtual void Kill()
    {
        GD.Print("Destroyed: ", this.ToString());
        QueueFree();
    }
    #endregion

    #region ENTER TREE READY ETC
    public override void _EnterTree()
    {

    }
    #endregion
}
