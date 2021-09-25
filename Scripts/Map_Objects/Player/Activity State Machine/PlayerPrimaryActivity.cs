using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;


public class PlayerPrimaryActivity : PlayerActivityBase
{
    public override void DoPlayerActionProcess()
    {

    }

    public override void ShowCurrentDisplay()
    {
        Main.map?.Update_Higthlight_Display(player_character.Get_Posible_Moves(), TileType.Transparent_Orange);
    }

    public override void Start()
    {
        GD.Print("Changed to primary action");
    }

    public PlayerPrimaryActivity(PlayerCharacter p) : base(p) { }
}


