using Godot;
using System;
using System.Collections.Generic;
public partial class PlayerFSM : Node
{
	[Export] private CameraRotation camera;
	[Export] private Ray_Script ray;
	private PlayerState currentState;
	public Team playerTeam = Team.None;
	private Dictionary<Type,PlayerState> playerStates = new Dictionary<Type,PlayerState>()
	{{typeof(PlayerHoverState),new PlayerHoverState()},{typeof(PlayerNormalHoldState),new PlayerNormalHoldState()},
	{typeof(PlayerSpecialHoldState),new PlayerSpecialHoldState()},{typeof(PlayerNormalReleaseState),new PlayerNormalReleaseState()},
	{typeof(PlayerSpecialReleaseState),new PlayerSpecialReleaseState()},{typeof(PlayerInvalidReleaseState),new PlayerInvalidReleaseState()}};
	public ChessPiece currentHold;

	public ref BoardStruct[][] board{
		get { return ref ChessBoard.Instance.GetBoard(); }
	}
	public BoardStruct hoverTile{
		get{return ChessBoard.Instance.GetBoardStruct(ray.currentHover);}
	}
	public Vector3 collisionPoint{get{return ray.GetCollisionPoint();}}

	public List<Vector2I> abilityMoves{
		get{return ChessBoard.Instance.FilterMoves(currentHold.GetAbilityMoves(ref board));}
	}

	public List<Vector2I> normalMoves{
		get{return ChessBoard.Instance.FilterMoves(currentHold.GetAvailableMoves(ref board));}
	}

	public override void _Ready()
	{
		foreach(PlayerState state in playerStates.Values)
            state.Init(this);
        TransitToState(playerStates[typeof(PlayerHoverState)]);
		EventBus<GameSetupEvent>.OnEvent += OnGameStart;
	}

	private void OnGameStart(GameSetupEvent gameStart){
		playerTeam = gameStart.team;
		camera.AdaptCamera(playerTeam);
		camera.targetPosition = new Vector3(4,0,4);
		camera.RotateCamera(Vector2.Zero,Vector2.Zero,0);
		GD.Print("Team: " + playerTeam);
	}
	public override void _Process(double delta)
	{
		currentState.Handle();
	}

    private void TransitToState(PlayerState pPlayerState){
        if(currentState != null){
            currentState.OnExitState();
            //Invoke event
        }
        currentState = pPlayerState;
        currentState.OnEnterState();
        //Invoke event
    }

	public void TransitToState(Type type){
		TransitToState(playerStates[type]);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent){
			currentState.HandleInput(mouseEvent);
		}
	}

    public override void _ExitTree()
    {
        EventBus<GameSetupEvent>.OnEvent -= OnGameStart;
    }
    public void InvokeMove(ChessPiece pChessPiece, Command pCommand){
		EventBus<PieceReleaseEvent>.Invoke
		(new PieceReleaseEvent(pChessPiece,pCommand));
	}
	public bool isPieceValid(out ChessPiece outPiece){
		if(hoverTile == null){
			outPiece = null;
			return false;
		}
		
		outPiece = hoverTile.piece;
		if(outPiece == null) return false;

		return ChessBoard.teamTurn == playerTeam && hoverTile.tile != null && 
		hoverTile.piece != null && playerTeam == hoverTile.piece.Team && !hoverTile.piece.frozen;
	}
}

public abstract class PlayerState {
    public PlayerFSM playerFSM{get; private set;}

    public virtual void Init(PlayerFSM pPlayerFSM){
        playerFSM = pPlayerFSM;
    }

    public abstract void Handle();
	public virtual void HandleInput(InputEventMouseButton mouseEvent){}
    public abstract void OnEnterState();
    public abstract void OnExitState();
}

public class PlayerHoverState : PlayerState {

	private ChessPiece lastHover;
    public override void Handle(){
		if(lastHover != null){
			lastHover.OnHoverExit(playerFSM);
			lastHover = null;
		}
		if(playerFSM.isPieceValid(out ChessPiece piece)){
			piece.OnHoverUpdate(playerFSM);
			lastHover = piece;
		}
	}
    public override void OnEnterState(){

	}
    public override void OnExitState(){
		playerFSM.currentHold = playerFSM.hoverTile.piece;
	}

	public override void HandleInput(InputEventMouseButton mouseEvent){
		if(playerFSM.isPieceValid(out ChessPiece piece))
			piece.OnHoverInput(playerFSM,mouseEvent);
	}
}

public class PlayerNormalHoldState : PlayerState {

    public override void Handle(){
		foreach(Vector2I a in playerFSM.normalMoves)
			playerFSM.board[a.Y][a.X].tile.HighlightTile();
		playerFSM.currentHold.OnNormalHoldUpdate(playerFSM);
	}
    public override void OnEnterState(){
		EventBus<PiecePickUpEvent>.Invoke(new PiecePickUpEvent(playerFSM.currentHold,playerFSM.normalMoves));
	}
    public override void OnExitState(){
		foreach(Vector2I a in playerFSM.normalMoves)
			playerFSM.board[a.Y][a.X].tile.RemoveHighlight();
	}
	public override void HandleInput(InputEventMouseButton mouseEvent){
		playerFSM.currentHold.OnNormalHoldInput(playerFSM, mouseEvent);
	}
}

public class PlayerSpecialHoldState : PlayerState {

    public override void Handle(){
		foreach(Vector2I a in playerFSM.abilityMoves)
			playerFSM.board[a.Y][a.X].tile.HighlightTile();
		playerFSM.currentHold.OnSpecialHoldUpdate(playerFSM);
	}
    public override void OnEnterState(){
		EventBus<PiecePickUpEvent>.Invoke(new PiecePickUpEvent(playerFSM.currentHold,playerFSM.abilityMoves));
	}
    public override void OnExitState(){
		foreach(Vector2I a in playerFSM.abilityMoves)
			playerFSM.board[a.Y][a.X].tile.RemoveHighlight();
	}
	public override void HandleInput(InputEventMouseButton mouseEvent){
		playerFSM.currentHold.OnSpecialHoldInput(playerFSM, mouseEvent);
	}
}

public class PlayerNormalReleaseState : PlayerState {

    public override void Handle(){
		playerFSM.TransitToState(typeof(PlayerHoverState));
	}
    public override void OnEnterState(){
		playerFSM.InvokeMove(playerFSM.currentHold,new MoveCommand(SharedUtils.SetTileStruct(playerFSM.currentHold),SharedUtils.SetTileStruct(playerFSM.hoverTile)));
	}
    public override void OnExitState(){
		playerFSM.currentHold = null;
	}

}

public class PlayerSpecialReleaseState : PlayerState {

    public override void Handle(){
		playerFSM.TransitToState(typeof(PlayerHoverState));
	}
    public override void OnEnterState(){
		playerFSM.InvokeMove(playerFSM.currentHold, playerFSM.currentHold.AbilityMove(playerFSM.hoverTile));
	}
    public override void OnExitState(){
		playerFSM.currentHold = null;
	}
}

public class PlayerInvalidReleaseState : PlayerState {
    public override void Handle(){
		playerFSM.TransitToState(typeof(PlayerHoverState));
	}
    public override void OnEnterState(){
		playerFSM.currentHold.targetPosition = Vector3.Zero;	
	}
    public override void OnExitState(){
		playerFSM.currentHold = null;
	}
}
