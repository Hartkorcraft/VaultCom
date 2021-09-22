
public class NpcTurnState : GameState
{
    public NpcTurnState()
    {
        allowWorldInput = false;
        game_manager.UpdateTurnObjects();
    }
}
