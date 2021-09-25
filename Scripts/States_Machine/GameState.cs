using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public abstract class GameState
{
    protected static Game_Manager game_manager = Main.game_Manager;

    public virtual bool CanSelect { get; set; } = true;

    protected bool allowWorldInput = true;

    public virtual bool AllowWorldInput
    {
        get
        {
            if (game_manager != null) { return allowWorldInput && !game_manager.DebugConsoleOpen; } //game_manager.ContextMenuOpen; }
            return allowWorldInput;
        }
        set => allowWorldInput = value;
    }


    #region READY UPDATE EXIT 
    public virtual void ReadyState() { GD.Print("Entered " + GetType().ToString() + " state"); }
    public virtual void UpdateState() { Game_Manager.CurrentSelection?.HandleBeingSelected(); }
    public virtual void ExitState() { }
    #endregion


}
