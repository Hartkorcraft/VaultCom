
public class StartState : GameState
{

    public override void ReadyState()
    {
        base.ReadyState();
        game_manager.SetState(new PlayerTurnState());
    }
}
