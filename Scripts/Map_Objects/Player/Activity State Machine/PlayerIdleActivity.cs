using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public class PlayerIdleActivity : PlayerActivityBase
{
    public override void DoPlayerActionProcess()
    {

    }

    public override void Start()
    {
        GD.Print("Changed to idle action");
    }

    public PlayerIdleActivity(PlayerCharacter p) : base(p) { }
}

