
using Godot;

/**
 * This is where we 'play' a game.
 */
public class GameState : ApplicationStateWithView<GameView>
{
    public override void EnterState()
    {
        base.EnterState();
        if(fsm.channel.HasMessage())
			handleNetworkMessage(fsm.channel.ReceiveMessage());
        EventBus<PieceReleaseEvent>.OnEvent += sendMakeMoveRequest;
    }

    public override void ExitState()
    {
        EventBus<PieceReleaseEvent>.OnEvent -= sendMakeMoveRequest;
        base.ExitState();
    }

    public override void Update()
    {
        receiveAndProcessNetworkMessages();
    }

    protected override void handleNetworkMessage(ISerializable pMessage)
    {
        if (pMessage is MakeMoveResult){ handleMakeMoveResult(pMessage as MakeMoveResult); }
        else if(pMessage is GameStartEvent){ handleGameStartEvent(pMessage as GameStartEvent); }
        else if (pMessage is RoomJoinedEvent) handleRoomJoinedEvent (pMessage as RoomJoinedEvent);
    }

    private void handleGameStartEvent(GameStartEvent gameStartEvent)
    {
        view.thisPlayer.Text = gameStartEvent.playerName + ": " + gameStartEvent.playerTeam;
        view.enemyPlayer.Text = gameStartEvent.enemyName + ": " + (gameStartEvent.playerTeam == Team.Blue? Team.Red : Team.Blue);
        EventBus<GameSetupEvent>.Invoke(new GameSetupEvent(gameStartEvent.playerTeam));
        //view.gameBoard.SetBoardData(new TicTacToeBoardData());
    }

    private void sendMakeMoveRequest(PieceReleaseEvent releaseEvent){
        GD.Print("Sending Message");
        MakeMoveRequest makeMove = new MakeMoveRequest(releaseEvent.command);
        fsm.channel.SendMessage(makeMove);
    }
    private void handleMakeMoveResult(MakeMoveResult pMakeMoveResult)
    {
        EventBus<CommandMessageRecived>.Invoke(new CommandMessageRecived(pMakeMoveResult.command));
    }
        private void handleRoomJoinedEvent (RoomJoinedEvent pMessage)
    {
        if (pMessage.room == RoomJoinedEvent.Room.LOBBY_ROOM)
        {
            fsm.ChangeState<LobbyState>();
        } 
    }
}
