using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public class PlayerUseActivity : PlayerActivityBase
{
    public override void DoPlayerAction()
    {

    }

    public override void UpdateDisplay()
    {
        Main.map?.UpdateHightligthDisplay(player_character.posible_positions_tile_cache, TileType.Red_Dot);
    }

    public override void Start(PlayerCharacter p)
    {
        base.Start(p);
        GD.Print("Changed to use action");
    }

    public PlayerUseActivity() : base() { }
}