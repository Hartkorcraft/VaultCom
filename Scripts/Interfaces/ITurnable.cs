
public interface ITurnable
{
    public int MovementPointsCap { get; set; }
    public int MovementPoints { get; set; }

    public void StartTurn();
    public void ResetMovementPoints();
    public void AddITurnableToGameManager();
}
