using Godot;

/**
 * This is the base class for any ApplicationState.
 * You can subclass it to implement a specific application state (e.g. logging in, in the lobby, playing a game, etc).
 * 
 * Each state has access to the finite state machine (fsm) it is a part of, so it can switch state and communicate.
 */
public abstract class ApplicationState 
{
	//provides access to the fsm for subclasses
	protected ApplicationFSM fsm { get; private set; }       

	/**
	 * Provide a handle to the FSM to this state.
	 */
	public virtual void Initialize(ApplicationFSM pApplicationFSM)
	{
		fsm = pApplicationFSM;
	}

	/**
	 * Tells this state to enable itself.
	 */
	public virtual void EnterState()
	{
		GD.Print("Entering application state " + this);
	}

	/**
	 * Tells this state to disable itself.
	 */
	public virtual void ExitState()
	{
		GD.Print("Exiting application state " + this);
	}

	public abstract void Update();
	/**
	 * Receives all messages from the server and calls the ABSTRACT handleNetworkMessage.
	 * 
	 * This method needs to be called EXPLICITLY (for example in the Update loop),
	 * whenever you are able/ready to receive and process network messages.
	 * 
	 * Implement handleNetworkMessage in your subclass to actually handle the received message.
	 */
	virtual protected void receiveAndProcessNetworkMessages()
	{
		if (!fsm.channel.Connected)
		{
			GD.PushWarning("Trying to receive network messages, but we are no longer connected.");
			return;
		}

		//while there are messages, we have no issues AAAND we haven't been disabled (important!!):
		//we need to check for gameObject.activeSelf because after sending a message and switching state,
		//we might get an immediate reply from the server. If we don't add this, the wrong state will be processing the message
		while (fsm.channel.HasMessage())
		{
			ISerializable message = fsm.channel.ReceiveMessage();
			handleNetworkMessage(message);
		}
	}

	/**
	 * Override/implement in a subclass
	 */
	abstract protected void handleNetworkMessage(ISerializable pMessage);

}
