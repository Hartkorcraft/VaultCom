using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;


public class PlayerPrimaryActivity : PlayerActivityBase
{
    public override void DoPlayerAction()
    {

    }

    public override void UpdateDisplay()
    {
        Main.map?.UpdateHightligthDisplay(player_character.posible_positions_tile_cache, TileType.Transparent_Orange);
    }

    public override void Start(PlayerCharacter p)
    {
        base.Start(p);
        GD.Print("Changed to primary action");
    }

    public PlayerPrimaryActivity() : base() { }
}


