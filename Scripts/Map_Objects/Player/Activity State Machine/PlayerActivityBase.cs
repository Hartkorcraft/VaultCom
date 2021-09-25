using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public abstract class PlayerActivityBase
{
    protected PlayerCharacter player_character;
    public bool Updated { get; set; } = false;
    public List<Vector2i> positions_cache { get; set; } = new List<Vector2i>();

    public abstract void DoPlayerActionProcess();
    public virtual void Start() { }
    public virtual void Exit() { }
    public virtual void ShowCurrentDisplay() { }
    public virtual void UpdateCalculations() { }

    public virtual void Clear_Hightlight_Display()
    {
        Main.map?.Update_Higthlight_Display(null);
    }

    public PlayerActivityBase(PlayerCharacter _p)
    {
        player_character = _p;
        GD.Print("Created activity!");
    }
}

