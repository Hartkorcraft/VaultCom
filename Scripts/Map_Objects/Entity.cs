using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public abstract class Entity : SpriteMapObject, ITurnable, ISelectable
{
    public int MovementPointsCap { get; set; } = 5;
    public int MovementPoints { get; set; } = 5;
    public bool CanSelect { get; set; } = true; //ISelectable

    public void ResetMovementPoints() { MovementPoints = MovementPointsCap; }  //ITurnable
    public abstract void AddITurnableToGameManager(); //ITurnable

    #region ENTER EXIT ETC.
    public override void _EnterTree()
    {
        base._EnterTree();
        AddITurnableToGameManager();
    }

    public override void _ExitTree()
    {
        base._ExitTree();
    }
    #endregion

    #region ITurnable
    public virtual void StartTurn() { ResetMovementPoints(); }
    public virtual void UpdateTurnObject() { }
    #endregion

    #region ISelectable 
    public virtual void HandleSelection() { }
    public virtual void HandleBeingSelected() { }
    public virtual void HandleUnselection() { }
    #endregion
}
