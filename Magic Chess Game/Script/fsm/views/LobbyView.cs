using System;
using Godot;

/**
 * Wraps all elements and functionality required for the LobbyView.
 */
public partial class LobbyView : View
{
    //all components that need to be hooked up
    [Export] private Button _findMatch;
    public event Action<string> onMessageSend;
    [Export] private Label _statusMessage;
    public Label statusMessage => _statusMessage;
    [Export] private Label _playersInLobby;
    public Label playersInLobby => _playersInLobby;
    [Export] private Label _playersInQueue;
    public Label playersInQueue => _playersInQueue;
    [Export] private RichTextLabel _chatWindow;
    public RichTextLabel chatWindow => _chatWindow;
    [Export] private TextEdit _messageWindow;
    [Export] private Button _sendMessage;
    public event Action<bool> onFindButton;
    private bool inQueue = false;
    public override void _Ready()
    {
        _statusMessage.Visible = false;
        _sendMessage.Pressed += SendMessage;
        _findMatch.Pressed += MatchButton;
    }
    private void SendMessage(){
        onMessageSend.Invoke(_messageWindow.Text);
        _messageWindow.Text = "";
    }

    private void MatchButton(){
        inQueue = !inQueue;
        statusMessage.Visible = inQueue;
        onFindButton.Invoke(inQueue);
    }

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent && keyEvent.Pressed){
			switch(keyEvent.Keycode){
				case Key.Enter:
					//SendMessage();
				break;
			}
		}
	}

    public override void _ExitTree()
    {
        base._ExitTree();
        _sendMessage.Pressed -= SendMessage;
        _findMatch.Pressed -= MatchButton;
    }
}
