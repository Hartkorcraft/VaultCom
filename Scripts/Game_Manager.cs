using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public class Game_Manager : Node2D
{
    public GameState Current_State { get; private set; }
    public GameState Previous_State { get; private set; }
    public bool ContextMenuOpen { get { return Main.context_menu.Visible; } }

    public void SetState(GameState new_game_state)
    {
        if (Current_State != null) { Current_State.ExitState(); }
        Previous_State = Current_State;
        Current_State = new_game_state;
        Current_State.ReadyState();
        Main.debug_Manager.UpdateLog("GameStates", "Current: " + Current_State.GetType().ToString() + " Previous: " + Previous_State.GetType().ToString());

    }

    //End sprite move transition and return to previous state
    public void EndSpriteMapObjectTransition()
    {
        if (Current_State is TransitionState)
        {
            SetState(Previous_State);
            Main.map.PathfindingTiles.Clear();
        }
        else { throw new Exception("Not transitioning?!"); }
    }

    public bool AllowWorldInput
    {
        get
        {
            return Current_State.AllowWorldInput;
        }
    }

    #region SELECTION

    private ISelectable currentSelection;
    public ISelectable CurrentSelection
    {
        get { return currentSelection; }
        set
        {
            if (Current_State.AllowWorldInput is false) { return; };
            currentSelection = value;

            string name = Helpers.NameAndType(currentSelection as INameable);
            if (value is null) { name = "null"; }

            GD.Print("Selected: ", name);
            Main.debug_Manager.UpdateLog("Current_Selection", name);
        }
    }

    public void Select(ISelectable selection, bool unSelectIfSame = false)
    {
        Current_State.Select(selection, unSelectIfSame);
    }

    #endregion

    #region MOUSE OVER

    public HashSet<IMouseable> Mouseover { get; private set; } = new HashSet<IMouseable>();

    public void AddMouseOverObject(IMouseable imouseable) { Mouseover.Add(imouseable); }
    public bool RemoveMouseOverObject(IMouseable imouseable) { return Mouseover.Remove(imouseable); }
    public string GetMouseOverHashSetString()
    {
        string imouseableObjects = "";
        foreach (var imouseable in Mouseover)
        {
            imouseableObjects += imouseable.ToString() + " " + imouseable.GetType().Name + ", ";
        }
        return imouseableObjects;
    }

    #endregion

    #region ENTER TREE, READY, PROCESS, INPUT etc.

    public override void _EnterTree()
    {

    }
    public override void _Ready()
    {
        Main.debug_Manager.AddLog(new DebugInfo("Current_Selection"));
        Main.debug_Manager.AddLog(new DebugInfo("GameStates"));
        //Main.debug_Manager.AddLog(new DebugInfo("GameStates"));
        SetState(new StartState());
    }
    public override void _Process(float delta)
    {
        Current_State.UpdateState();
    }
    #endregion
}
