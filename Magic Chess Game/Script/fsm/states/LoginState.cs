
using Godot;

/**
 * Starting state where you can connect to the server.
 */
public class LoginState : ApplicationStateWithView<LoginView>
{
    private int _serverPort = 55555;

    public override void EnterState()
    {
        base.EnterState();
        //If flagged, generate a random name and connect automatically
        view.joinButton.Pressed += Connect;
    }

    public override void ExitState ()
    {
        view.joinButton.Pressed -= Connect;
        base.ExitState();

        //stop listening to button cicks
    }

    /**
     * Connect to the server (with some client side validation)
     */
    private void Connect()
    {
        if (view.userName == "")
        {
            view.errorField.Text = "Please enter a name first";
            return;
        }

        //connect to the server and on success try to join the lobby
        if (fsm.channel.Connect(view.IPaddress, _serverPort))
        {
            tryToJoinLobby();
        } else
        {
            view.errorField.Text = "Oops, couldn't connect:"+string.Join("\n", fsm.channel.GetErrors());
        }
    }

    private void tryToJoinLobby()
    {
        //Construct a player join request based on the user name 
        PlayerJoinRequest playerJoinRequest = new PlayerJoinRequest(view.userName);
        fsm.channel.SendMessage(playerJoinRequest);
    }

    /// //////////////////////////////////////////////////////////////////
    ///                     NETWORK MESSAGE PROCESSING
    /// //////////////////////////////////////////////////////////////////

    public override void Update()
    {
        //if we are connected, start processing messages
        if (fsm.channel.Connected) receiveAndProcessNetworkMessages();
    }

    
    protected override void handleNetworkMessage(ISerializable pMessage)
    {
        //GD.Print("Recived a message!" + pMessage.GetType().Name);
        if (pMessage is PlayerJoinResponse) handlePlayerJoinResponse (pMessage as PlayerJoinResponse);
        else if (pMessage is RoomJoinedEvent) handleRoomJoinedEvent (pMessage as RoomJoinedEvent);
    }
    

    private void handlePlayerJoinResponse(PlayerJoinResponse pMessage)
    {
        //Dont do anything with this info at the moment, just leave it to the RoomJoinedEvent
        //We could handle duplicate name messages, get player info etc here
        
        if (pMessage.result == PlayerJoinResponse.RequestResult.REJECTED)
        {
            view.errorField.Text = pMessage.errorMsg;
        }
        
    }

    private void handleRoomJoinedEvent (RoomJoinedEvent pMessage)
    {
        if (pMessage.room == RoomJoinedEvent.Room.LOBBY_ROOM)
        {
            fsm.ChangeState<LobbyState>();
        } 
    }

}

