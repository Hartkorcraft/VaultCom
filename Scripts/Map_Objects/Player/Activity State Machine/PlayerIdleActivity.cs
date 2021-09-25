using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public class PlayerIdleActivity : PlayerActivityBase
{
    public override void Start()
    {
        if (LogChangesInGodot) { GD.Print("Changed to idle action"); }
    }
    public override void DoPlayerActionProcess() { }

    public override void ShowCurrentDisplay() { }

    public override void UpdateCalculations() { }

    public PlayerIdleActivity(PlayerCharacter p) : base(p) { }
}

