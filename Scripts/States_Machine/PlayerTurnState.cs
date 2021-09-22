
public class PlayerTurnState : GameState
{
    public PlayerTurnState()
    {
        allowWorldInput = true;
        game_manager.StartPlayerTurn();
    }

}
