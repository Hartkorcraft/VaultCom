using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public class PlayerUseActivity : PlayerActivityBase
{
    public override void DoPlayerActionProcess()
    {

    }

    public override void ShowCurrentDisplay()
    {
        Main.map?.Update_Higthlight_Display(player_character.Get_Posible_Moves(), TileType.Red_Dot);
    }

    public override void Start()
    {
        GD.Print("Changed to use action");
    }

    public PlayerUseActivity(PlayerCharacter p) : base(p) { }
}