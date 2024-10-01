using Godot;
/**
 * Wraps all elements and functionality required for the GameView.
 */
public partial class GameView : View
{
    [Export] private Label _thisPlayer;
    public Label thisPlayer => _thisPlayer;

    [Export] private Label _enemyPlayer;
    public Label enemyPlayer => _enemyPlayer;

    [Export] private ChessBoard _chessBoard;
    public ChessBoard chessBoard => _chessBoard;

    [Export] private PlayerFSM _playerFSM;
    public PlayerFSM playerFSM => _playerFSM;
}

