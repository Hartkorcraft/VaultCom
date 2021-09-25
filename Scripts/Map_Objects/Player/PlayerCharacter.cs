using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public class PlayerCharacter : Entity, ISelectable, IGetInfoable
{
    public static int number_of_player_characters { get; private set; } = 0;

    public Vector2i lastMouseGridPos { get; private set; }
    public PlayerActivityBase Default_Action { get; private set; }
    public PlayerPathfindingActivity player_finding_path_activity { get; protected set; }
    public PlayerPrimaryActivity player_primary_activity { get; protected set; }
    public PlayerUseActivity player_use_activity { get; protected set; }
    public PlayerIdleActivity player_idle_activity { get; protected set; }
    public List<Vector2i> path_positions_cache { get; protected set; } = new List<Vector2i>();

    private PlayerActivityBase current_action;
    private PlayerActivityBase previous_action = null;

    public PlayerActivityBase Current_Action
    {
        get => current_action;
        private set
        {
            if (current_action == value) { return; }
            previous_action = current_action;
            current_action = value;
            current_action.Start();
            if (current_action.Updated is false) { current_action.UpdateCalculations(); }
        }
    }

    public List<Vector2i> Get_Posible_Moves()
    {
        player_finding_path_activity.UpdateCalculations();
        return player_finding_path_activity.positions_cache;
    }

    #region ENTER_TREE READY ETC
    public override void _EnterTree()
    {
        base._EnterTree();
        number_of_player_characters++;
        AddIGetInfoableToGameManager();
        GridPos = new Vector2i(Position / new Vector2(Main.TILE_SIZE, Main.TILE_SIZE));

        player_finding_path_activity = new PlayerPathfindingActivity(this);
        player_primary_activity = new PlayerPrimaryActivity(this);
        player_use_activity = new PlayerUseActivity(this);
        player_idle_activity = new PlayerIdleActivity(this);

        Default_Action = player_finding_path_activity;
        UI.primary_button_pressed += TryDoPrimaryAction;
        UI.use_button_pressed += TryDoUseAction;
        ObjectName += $"_{number_of_player_characters}";
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        number_of_player_characters--;
        UI.primary_button_pressed -= TryDoPrimaryAction;
        UI.use_button_pressed -= TryDoUseAction;
    }
    public override void _Ready()
    {
        base._Ready();
        Current_Action = Default_Action;
        GD.Print("Spawned at: " + Position);
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        if (Main.game_Manager?.AllowWorldInput is true)
        {
            if (Game_Manager.CurrentSelection == this && Current_Action != null)
            {
                Current_Action.DoPlayerActionProcess();
            }
        }
        lastMouseGridPos = Main.Mouse_Grid_Pos;
    }
    #endregion

    public override void _Input(InputEvent inputEvent)
    {
        //*Movement
        if (inputEvent.IsActionPressed("Left_Mouse") && Main.game_Manager?.AllowWorldInput is true)
        {
            if (path_positions_cache.Count > 0 && path_positions_cache[path_positions_cache.Count - 1].Equals(Main.Mouse_Grid_Pos))
            {
                MovementPoints -= path_positions_cache.Count;
                MoveOnPath(path_positions_cache);
                path_positions_cache.Clear();
            }
        }
    }

    #region  IMOUSEABLE 
    public override void On_Left_Mouse_Click()
    {
        base.On_Left_Mouse_Click();
        if (Main.game_Manager?.AllowWorldInput is true)
        {
            Main.game_Manager?.Select(this, true);
        }
    }

    public override void On_Right_Mouse_Click()
    {
        base.On_Right_Mouse_Click();
    }
    #endregion

    #region ISelectable
    public override void HandleSelection()
    {
        Current_Action = Default_Action;
        Current_Action.ShowCurrentDisplay();
    }

    public override void HandleUnselection()
    {
        Current_Action.Clear_Hightlight_Display();
        Current_Action = player_idle_activity;
    }
    #endregion

    #region ITurnable
    public override void UpdateTurnObject()
    {
        base.UpdateTurnObject();
        Current_Action.UpdateCalculations();
    }

    public override void StartTurn()
    {
        base.StartTurn();
        Set_Action_Updated_Flag_To_False();
        //CalculatePossiblePositions();
        Current_Action = player_idle_activity;
    }

    public override void AddITurnableToGameManager() { Game_Manager.PlayerTurnObjects?.Add(this); }
    #endregion

    #region PlayerActivites
    public PlayerActivityBase ChangeToPreviousAction()
    {
        if (previous_action != null) { return Current_Action = previous_action; }
        else { return Current_Action = Default_Action; }
    }

    public PlayerActivityBase ChangeToDefaultAction() => Current_Action = Default_Action;

    public void Set_Action_Updated_Flag_To_False()
    {
        player_idle_activity.Updated = false;
        player_primary_activity.Updated = false;
        player_use_activity.Updated = false;
        player_finding_path_activity.Updated = false;
    }

    public void TryDoPrimaryAction()
    {
        if (Game_Manager.CurrentSelection == this)
        {
            if (Current_Action as PlayerPrimaryActivity is null) { Current_Action = player_primary_activity; }
            else { Current_Action = ChangeToDefaultAction(); }
            Current_Action?.ShowCurrentDisplay();
        }
    }

    public void TryDoUseAction()
    {
        if (Game_Manager.CurrentSelection == this)
        {
            if (Current_Action as PlayerUseActivity is null) { Current_Action = player_use_activity; }
            else { Current_Action = ChangeToDefaultAction(); }
            Current_Action?.ShowCurrentDisplay();
        }
    }
    #endregion

    protected override void EndTransition()
    {
        base.EndTransition();
        Set_Action_Updated_Flag_To_False();
        Current_Action.ShowCurrentDisplay();
    }

    #region IGetInfoable
    public string GetInfo()
    {
        return
        $@"*Name: {ObjectName}
        -Current_Action: {current_action?.ToString()}
        -GridPos: {GridPos.ToString()}
        -Health and Cap: {Health}/{HealthCap}
        -Movement points and Cap: {MovementPoints}/{MovementPointsCap}";
    }

    public void AddIGetInfoableToGameManager()
    {
        Game_Manager.infoObjects?.Add(this);
    }
    #endregion
}
