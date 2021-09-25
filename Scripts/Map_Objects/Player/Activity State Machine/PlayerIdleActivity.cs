using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public class PlayerIdleActivity : PlayerActivityBase
{
    public override void DoPlayerAction()
    {

    }

    public override void Start(PlayerCharacter p)
    {
        base.Start(p);
        GD.Print("Changed to idle action");
    }

    public PlayerIdleActivity() : base() { }
}

