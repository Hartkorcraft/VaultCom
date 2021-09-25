using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public abstract class PlayerActivityBase
{
    protected PlayerCharacter player_character;

    public abstract void DoPlayerAction();

    public virtual void Start(PlayerCharacter p)
    {
        player_character = p;
    }

    public virtual void Exit() { }

    public virtual void UpdateDisplay() { }

    public virtual void ClearDisplay()
    {
        Main.map?.UpdateHightligthDisplay(null);
    }

    public PlayerActivityBase() { }
}

