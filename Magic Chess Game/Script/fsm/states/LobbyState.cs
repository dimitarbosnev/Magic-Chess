
using Godot;

/**
 * 'Chat' state while you are waiting to start a game where you can signal that you are ready or not.
 */
public class LobbyState : ApplicationStateWithView<LobbyView>
{
    public override void EnterState()
    {
        base.EnterState();
        view.onMessageSend += onTextEntered;
        view.onFindButton += onReadyToggleClicked;
        while(fsm.channel.HasMessage())
			handleNetworkMessage(fsm.channel.ReceiveMessage());
    }

    public override void ExitState()
    {
        view.onMessageSend -= onTextEntered;
        view.onFindButton -= onReadyToggleClicked;
        base.ExitState();
    }

    /**
     * Called when you enter text and press enter.
     */
    private void onTextEntered(string pText)
    {
        ChatMessage chatMessage = new ChatMessage();
        chatMessage.message = pText;
        fsm.channel.SendMessage(chatMessage);
        //addOutput("(noone else will see this because I broke the chat on purpose):"+pText);        
    }

    /**
     * Called when you click on the ready checkbox
     */
    private void onReadyToggleClicked(bool pNewValue)
    {
        GD.Print(pNewValue);
        ChangeReadyStatusRequest msg = new ChangeReadyStatusRequest
        {
            ready = pNewValue
        };
        fsm.channel.SendMessage(msg);
    }

    private void addOutput(string pInfo)
    {
        view.chatWindow.AddText(pInfo + "\n");
    }

    /// //////////////////////////////////////////////////////////////////
    ///                     NETWORK MESSAGE PROCESSING
    /// //////////////////////////////////////////////////////////////////

       public override void Update()
    {
        receiveAndProcessNetworkMessages();
    }
    
    protected override void handleNetworkMessage(ISerializable pMessage)
    {
        if (pMessage is ChatMessage) handleChatMessage(pMessage as ChatMessage);
        else if (pMessage is RoomJoinedEvent) handleRoomJoinedEvent(pMessage as RoomJoinedEvent);
        else if (pMessage is LobbyInfoUpdate) handleLobbyInfoUpdate(pMessage as LobbyInfoUpdate);
    }

    private void handleChatMessage(ChatMessage pMessage)
    {
        //just show the message
       addOutput(pMessage.message);
    }
    private void handleRoomJoinedEvent(RoomJoinedEvent pMessage)
    {
        //did we move to the game room?
        if (pMessage.room == RoomJoinedEvent.Room.GAME_ROOM)
        {
            fsm.ChangeState<GameState>();
        }
    }

    private void handleLobbyInfoUpdate(LobbyInfoUpdate pMessage)
    {
        GD.Print("Players in Lobby: " + pMessage.memberCount);
        view.playersInLobby.Text = "Players in Lobby: " + pMessage.memberCount.ToString();
        GD.Print("Players in Queue: " + pMessage.readyCount);
        view.playersInQueue.Text = "Players in Queue: " + pMessage.readyCount.ToString();
    }

}
