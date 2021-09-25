using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;


public class PlayerPrimaryActivity : PlayerActivityBase
{
    public override void Start()
    {
        if (LogChangesInGodot) { GD.Print("Changed to primary action"); }
    }
    public override void DoPlayerActionProcess()
    {

    }

    public override void ShowCurrentDisplay()
    {
        Main.map?.Update_Higthlight_Display(player_character.Get_Posible_Moves(), TileType.Transparent_Orange);
    }

    public override void UpdateCalculations()
    {

    }
    public PlayerPrimaryActivity(PlayerCharacter p) : base(p) { }
}


