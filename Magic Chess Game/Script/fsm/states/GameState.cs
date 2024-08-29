/**
 * This is where we 'play' a game.
 */
public class GameState : ApplicationStateWithView<GameView>
{
    public override void EnterState()
    {
        base.EnterState();
        
        //view.gameBoard.OnCellClicked += _onCellClicked;
    }

    public override void ExitState()
    {
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
        //view.playerLabel1.text = "Player 1: " + gameStartEvent.player1Name;
        //view.playerLabel2.text = "Player 2: " + gameStartEvent.player2Name;
        //view.gameBoard.SetBoardData(new TicTacToeBoardData());
    }

    private void handleMakeMoveResult(MakeMoveResult pMakeMoveResult)
    {

        //some label display
        /*if (pMakeMoveResult.whoMadeTheMove == 1)
        {
            player1MoveCount++;
            view.playerLabel1.text = $"Player 1 (Movecount: {player1MoveCount})";
        }
        if (pMakeMoveResult.whoMadeTheMove == 2)
        {
            player2MoveCount++;
            view.playerLabel2.text = $"Player 2 (Movecount: {player2MoveCount})";
        }*/

    }
        private void handleRoomJoinedEvent (RoomJoinedEvent pMessage)
    {
        if (pMessage.room == RoomJoinedEvent.Room.LOBBY_ROOM)
        {
            fsm.ChangeState<LobbyState>();
        } 
    }
}
