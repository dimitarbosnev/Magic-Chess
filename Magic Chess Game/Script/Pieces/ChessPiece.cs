using Godot;
using System.Collections.Generic;
public partial class ChessPiece : Node3D
{
	private PieceType _pieceType;
	private Team _team;
	public bool _ability = true;
	public bool frozen = false;
	private Tile _tile;
	private static int PieceIdCounter = 0;
	public int pieceID{get; private set;}
	public Tile tile{
		get { return _tile; }
		set {if(value != null) {Reparent(value); if(_tile != null) _tile.piece = null; } _tile = value; SetPosition(Vector3.Zero);}
	}
	public virtual bool Ability {
		get { return _ability; }
		set { _ability = value; }
	}
	public Vector2I coordinates {
		get { return _tile.coordinates; }
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
        pieceID = PieceIdCounter;
		PieceIdCounter++;
    }
    public override void _Process(double delta)
	{
		Position = Position.Lerp(targetPosition, (float)delta * 10);
	}

	public virtual List<Vector2I> GetAvailableMoves(ref Tile[][] board)
	{
		List<Vector2I> r = new List<Vector2I>();
		r.Add(new Vector2I(0, 0));
		r.Add(new Vector2I(board.Length-1, 0));
		r.Add(new Vector2I(0, board.Length-1));
		r.Add(new Vector2I(board.Length-1, board.Length-1));
		return r;
	}

	public virtual List<Vector2I> GetAbilityMoves(ref Tile[][] board)
	{
		List<Vector2I> r = new List<Vector2I>();
		for (int y = 0; y < board.Length; y++){
			for (int x = 0; x < board[y].Length; x++)
				r.Add(new Vector2I(x, y));
		}
		return r;
	}
	public virtual Command AbilityMove(Tile target){
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
			if(playerFSM.hoverTile != null && playerFSM.normalMoves.Contains(playerFSM.hoverTile.coordinates))
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
			if(playerFSM.hoverTile != null && playerFSM.abilityMoves.Contains(playerFSM.hoverTile.coordinates))
				playerFSM.TransitToState(typeof(PlayerSpecialReleaseState));
			else
				playerFSM.TransitToState(typeof(PlayerInvalidReleaseState));
	}
}
