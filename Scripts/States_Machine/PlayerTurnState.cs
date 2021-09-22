
public class PlayerTurnState : GameState
{


    // public override void ExitState()
    // {
    //     base.ExitState();
    //     game_manager.CurrentSelection = null;
    // }

    public PlayerTurnState()
    {
        allowWorldInput = true;
        game_manager.StartPlayerTurn();
    }

}
