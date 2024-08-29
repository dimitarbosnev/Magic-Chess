/**
 * Starting state where you can connect to the server.
 */
public class LoginState : ApplicationStateWithView<LoginView>
{
    private string _serverIP = null;
    private int _serverPort = 0;
    private bool autoConnectWithRandomName = false;

    public override void EnterState()
    {
        base.EnterState();
        //If flagged, generate a random name and connect automatically
        if (autoConnectWithRandomName)
        {
            Connect();
        }
    }

    public override void ExitState ()
    {
        base.ExitState();

        //stop listening to button clicks
    }

    /**
     * Connect to the server (with some client side validation)
     */
    private void Connect()
    {

    }

    private void tryToJoinLobby()
    {
        //Construct a player join request based on the user name 
        PlayerJoinRequest playerJoinRequest = new PlayerJoinRequest();
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
        if (pMessage is PlayerJoinResponse) handlePlayerJoinResponse (pMessage as PlayerJoinResponse);
        else if (pMessage is RoomJoinedEvent) handleRoomJoinedEvent (pMessage as RoomJoinedEvent);
    }
    

    private void handlePlayerJoinResponse(PlayerJoinResponse pMessage)
    {
        //Dont do anything with this info at the moment, just leave it to the RoomJoinedEvent
        //We could handle duplicate name messages, get player info etc here
        
        //if (pMessage.result == PlayerJoinResponse.RequestResult.REJECTED)
        //{
             //view.TextConnectResults = "Duplicate name!";
        //}
        
    }

    private void handleRoomJoinedEvent (RoomJoinedEvent pMessage)
    {
        if (pMessage.room == RoomJoinedEvent.Room.LOBBY_ROOM)
        {
            fsm.ChangeState<LobbyState>();
        } 
    }

}

