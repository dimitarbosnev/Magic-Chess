using Godot;
using System.Collections.Generic;
public partial class ChessPiece : Node3D
{
	private PieceType _pieceType;
	private Team _team;
	public bool _ability = true;
	public bool frozen = false;

	public int id {get; private set;}
	public virtual bool Ability {
		get { return _ability; }
		set { _ability = value; }
	}
	public Team Team {
		get { return _team; }
	}

	public PieceType PieceType {
		get { return _pieceType; }
	}
	public Vector3 targetPosition;
	public Vector3 targetScale;

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Ready()
    {
    }
    public override void _Process(double delta)
	{
		Position = Position.Lerp(targetPosition, (float)delta * 10);
	}
	public void SetID(int pId){
		id = pId;
	}
	public virtual List<Vector2I> GetAvailableMoves(ref BoardStruct[][] board)
	{
		return null;
	}

	public virtual List<Vector2I> GetAbilityMoves(ref BoardStruct[][] board)
	{

		return null;
	}
	public virtual Command AbilityMove(BoardStruct target){
		return null;
	}
	//public abstract void 
	public virtual void SetPosition(Vector3 position, bool force = false)
	{
		targetPosition = position;
		if (force)
			Position = targetPosition;
	}
	public virtual void SetScale(Vector3 scale, bool force = false)
	{
		targetScale = scale;
		if (force)
			Scale = targetScale;
	}

	public void InitPiece(PieceType type, Team team)
	{
		_pieceType = type;
		_team = team;
	}

	public virtual void OnHoverUpdate(PlayerFSM playerFSM){
		targetPosition = Vector3.Up * 0.2f;
	}

		public virtual void OnHoverExit(PlayerFSM playerFSM){
		targetPosition = Vector3.Zero;
	}

	public virtual void OnHoverInput(PlayerFSM playerFSM, InputEventMouseButton mouseEvent){
		if(mouseEvent.ButtonIndex == MouseButton.Left)
			if(mouseEvent.DoubleClick  && playerFSM.hoverTile.piece.Ability)
				playerFSM.TransitToState(typeof(PlayerSpecialHoldState));
			else if(mouseEvent.Pressed)
				playerFSM.TransitToState(typeof(PlayerNormalHoldState));
	}
	public virtual void OnNormalHoldUpdate(PlayerFSM playerFSM){
		targetPosition = playerFSM.collisionPoint - GetParentNode3D().Position + Vector3.Up * 0.5f;
	}

	public virtual void OnNormalHoldInput(PlayerFSM playerFSM, InputEventMouseButton mouseEvent){
		if(mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.IsReleased()) {
			if(playerFSM.hoverTile.tile != null && playerFSM.normalMoves.Contains(playerFSM.hoverTile.tile.coordinates))
				playerFSM.TransitToState(typeof(PlayerNormalReleaseState));
			else
				playerFSM.TransitToState(typeof(PlayerInvalidReleaseState));
		}
	}
	
	public virtual void OnSpecialHoldUpdate(PlayerFSM playerFSM){
		targetPosition = playerFSM.collisionPoint - GetParentNode3D().Position + Vector3.Up * 0.5f;

		
	}
	public virtual void OnSpecialHoldInput(PlayerFSM playerFSM, InputEventMouseButton mouseEvent){
		if(mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.IsReleased()) 
			if(playerFSM.hoverTile.tile != null && playerFSM.abilityMoves.Contains(playerFSM.hoverTile.tile.coordinates))
				playerFSM.TransitToState(typeof(PlayerSpecialReleaseState));
			else
				playerFSM.TransitToState(typeof(PlayerInvalidReleaseState));
	}

	protected bool boundryCheck(Vector2I move,BoardStruct[][] board){
		if( move.Y >= 0 && move.Y < board.Length && move.X >= 0 && move.X < board[move.Y].Length)
			return true;
		return false;
	}

	protected ref BoardStruct getTile(Vector2I cordinates,BoardStruct[][] board){

		return ref board[cordinates.Y][cordinates.X];
	}

	protected void loopMoveCheck(Vector2I coordinates,Vector2I direction, ref BoardStruct[][] board, ref List<Vector2I> list){
		Vector2I move = coordinates + direction;

		while(boundryCheck(move,board)){
			BoardStruct tile = getTile(move,board);
			if(tile.piece == null){
				list.Add(move);
			}
			else if(tile.piece != null && tile.piece.Team != Team){
				list.Add(move);
				break;
			}
			else if(tile.piece != null && tile.piece.Team == Team)
				break;
			move += direction;
		}
	}

	protected void singleMoveCheck(Vector2I move, ref BoardStruct[][] board, ref List<Vector2I> list){
		if(!boundryCheck(move,board)) return;
		BoardStruct tile = getTile(move,board);
		if(tile.piece == null)
			list.Add(move);
		else if(tile.piece != null && tile.piece.Team != Team)
			list.Add(move);
	}

	protected void singleMoveCheck_NoTake(Vector2I move, ref BoardStruct[][] board, ref List<Vector2I> list){
		if(!boundryCheck(move,board)) return;
		BoardStruct tile = getTile(move,board);
		if(tile.piece == null)
			list.Add(move);
	}

	protected void singleMoveCheck_OnlyTake(Vector2I move, ref BoardStruct[][] board, ref List<Vector2I> list){
		if(!boundryCheck(move,board)) return;
		BoardStruct tile = getTile(move,board);
		if(tile.piece != null && tile.piece.Team != Team)
			list.Add(move);
	}
}
