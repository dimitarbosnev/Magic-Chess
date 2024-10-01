using Godot;
using System.Net;

/**
 * Wraps all elements and functionality required for the LoginView.
 */
public partial class LoginView : View
{
    [Export] private LineEdit _ipField;
    [Export] private string defaultIP = IPAddress.Loopback.ToString();
    public string IPaddress { get {if(_ipField.Text == "") return defaultIP; else return _ipField.Text;}}
    
    [Export] private LineEdit _textField;
    public string userName { get => _textField.Text;}

    [Export] private Label _errorField;
    public Label errorField => _errorField;

    [Export] private Button _joinButton;
    public Button joinButton => _joinButton;

    public override void _Ready()
    {
        errorField.Visible = true;
    }
}
