using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public class Game_Manager : Node2D
{
    public GameState Current_State { get; private set; }
    public GameState Previous_State { get; private set; }
    public bool DebugConsoleOpen { get { return Main.debug_Manager.debugConsole.Visible; } }
    public bool AllowWorldInput { get { return Current_State.AllowWorldInput; } }
    public HashSet<ITurnable> PlayerTurnObjects { get; } = new HashSet<ITurnable>();
    public HashSet<ITurnable> NpcTurnObjects { get; } = new HashSet<ITurnable>();

    public static event Action<GameState> changed_state_event;
    //public bool ContextMenuOpen { get { return Main.context_menu.Visible; } }
    //public bool ContextMenuSafeCheck { get { return Main.context_menu.SafeInput; } }


    public void SetState(GameState new_game_state)
    {
        if (Current_State != null) { Current_State.ExitState(); }
        Previous_State = Current_State;
        Current_State = new_game_state;
        changed_state_event?.Invoke(new_game_state);
        Current_State.ReadyState();
        Main.debug_Manager.UpdateLog("GameStates", "Current: " + Current_State.GetType().ToString() + " Previous: " + Previous_State.GetType().ToString());
    }

    #region SELECTION
    private ISelectable currentSelection;
    public ISelectable CurrentSelection
    {
        get { return currentSelection; }
        set
        {
            if (Current_State.AllowWorldInput is false) { return; };
            CurrentSelection?.HandleBeingUnselected();
            currentSelection = value;
            CurrentSelection?.HandleBeingSelected();

            string name = Helpers.NameAndType(currentSelection as INameable);
            if (value is null) { name = "null"; }

            GD.Print("Selected: ", name);
            Main.debug_Manager.UpdateLog("Current_Selection", name);
        }
    }

    public void Select(ISelectable selection, bool unSelectIfSame = false)
    {
        if (CurrentSelection == selection && unSelectIfSame) { selection = null; }
        CurrentSelection = selection;
    }

    #endregion

    #region MOUSE OVER
    public HashSet<IMouseable> Mouseover { get; private set; } = new HashSet<IMouseable>();
    public void AddMouseOverObject(IMouseable imouseable) { Mouseover.Add(imouseable); }
    public void RemoveMouseOverObject(IMouseable imouseable) { Mouseover.Remove(imouseable); }

    public void StartPlayerTurn()
    {
        foreach (ITurnable playerObject in PlayerTurnObjects) { playerObject.StartTurn(); }
        UpdateTurnObjects();
    }

    public void StartNpcTurn()
    {
        foreach (ITurnable npcObject in NpcTurnObjects) { npcObject.StartTurn(); }
        UpdateTurnObjects();
    }

    public void UpdateTurnObjects()
    {
        foreach (ITurnable npcObject in NpcTurnObjects)
        {
            npcObject.UpdateTurnObject();
        }
        foreach (ITurnable playerObject in PlayerTurnObjects)
        {
            playerObject.UpdateTurnObject();
        }
    }

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
        SpriteMapObject.end_transition_event += EndSpriteMapObjectTransition;
        SpriteMapObject.moving_on_path_event += SetState;
        SpriteMapObject.mouse_enter_over_event += AddMouseOverObject;
        SpriteMapObject.mouse_exit_over_event += RemoveMouseOverObject;
    }
    public override void _ExitTree()
    {
        base._ExitTree();
        SpriteMapObject.end_transition_event -= EndSpriteMapObjectTransition;
        SpriteMapObject.moving_on_path_event -= SetState;
        SpriteMapObject.mouse_enter_over_event -= AddMouseOverObject;
        SpriteMapObject.mouse_exit_over_event -= RemoveMouseOverObject;
    }
    public override void _Ready()
    {
        Main.debug_Manager.AddLog(new DebugInfo("Current_Selection"));
        Main.debug_Manager.AddLog(new DebugInfo("GameStates"));
        SetState(new StartState());
    }
    public override void _Process(float delta)
    {
        Current_State.UpdateState();
    }
    #endregion

    public void _on_Next_Turn_Button_pressed()
    {
        GD.Print("Pressed next turn button!");
        NextTurn();
    }

    public void NextTurn()
    {
        Main.map.PathfindingTiles.Clear();
        if (Current_State is PlayerTurnState) { SetState(new NpcTurnState()); }
        else if (Current_State is NpcTurnState) { SetState(new PlayerTurnState()); }
        CurrentSelection = null;
    }

    //End sprite move transition and return to previous state
    public void EndSpriteMapObjectTransition(SpriteMapObject mapObject)
    {
        UpdateTurnObjects();
        if (Current_State is TransitionState)
        {
            SetState(Previous_State);
            Main.map.PathfindingTiles.Clear();
        }
        else { throw new Exception("Not transitioning?!"); }
    }

}
