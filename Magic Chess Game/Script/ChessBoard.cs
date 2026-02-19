using Godot;
using System;
using System.Collections.Generic;
public class BoardStruct{
	public Tile tile;
	public ChessPiece piece;
}

public abstract partial class ChessBoard : Singelton<ChessBoard>
{
	[Export] private Node3D graveyardBlue = null;
	[Export] private Node3D graveyardRed = null;			
	private Dictionary<int,ChessPiece> piecesList = new Dictionary<int,ChessPiece>(); // TODO: handle for reversing a command
	protected float tileXoffset = 0;
	protected float tileYoffset = 0;
	public static Team teamTurn{get; private set;}
	public override void _Ready()
	{
		_instance = this;

		EventBus<CommandMessageRecived>.OnEvent += OnCommandRecived;
		EventBus<PieceKillEvent>.OnEvent += KillPiece;
		EventBus<NewTurnEvent>.OnEvent += OnNewTurn;

		EventBus<NewTurnEvent>.Invoke(new NewTurnEvent(Team.Blue));
	}
	private void OnNewTurn(NewTurnEvent newTurnEvent){
		GD.Print("Team's turn" + newTurnEvent.team);
		teamTurn = newTurnEvent.team;
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	private void ExecuteCommand(Command command){
		//TODO: move execution to the server side
		command.execute(ref GetBoard());
		//executedCommands.Push(command);
	}

	//public static Command GetCommandPeek(){
	//	//return executedCommands.Peek();
	//}

	private void OnCommandRecived(CommandMessageRecived e){
		ClearTiles();
		ExecuteCommand(e.command);
	}

	private void ClearTiles(){
		BoardStruct[][] board = GetBoard();
		for (int x = 0; x < board.Length; x++)
			for (int y = 0; y < board[y].Length; y++)
				board[y][x].tile.RemoveHighlight();
	}

	public Vector3 GetTilePos(int x, int y)
	{
		BoardStruct[][] board = GetBoard();
		return GetTilePos(board[y][x]);
	}
	public Vector3 GetTilePos(BoardStruct boardStruct)
	{
		return boardStruct.tile.Position;
	}
	protected abstract Tile GenerateSingleTiles(int x, int y);

	protected void GenerateAllTiles()
	{
		BoardStruct[][] board = GetBoard();
		for (int x = 0; x < board.Length; x++)
			for (int y = 0; y < board[x].Length; y++){
				board[y][x] = new BoardStruct();
				board[y][x].tile = GenerateSingleTiles(x, y);
			}			
	}

	public ChessPiece SpawnFigure(int X, int Y, PieceType type, Team team)
	{
		BoardStruct[][] board = GetBoard();
		var piece = Utils.pieceCollection[type].Instantiate() as ChessPiece;
		piece.InitPiece(type, team);
		piece.SetID(piecesList.Count);
		if(team == Team.Blue)
			piece.RotateY(Mathf.DegToRad(180));
		AddChild(piece);
		AssignPiece(piece, board[Y][X]);
		piecesList.Add(piecesList.Count, piece);

		return piece;
	}

	public ChessPiece SpawnFigure(int x, int y, PieceStruct pieceStruct)
	{
		if(pieceStruct == null) return null;
		BoardStruct[][] board = GetBoard();
		var piece = Utils.pieceCollection[pieceStruct.pieceType].Instantiate() as ChessPiece;
		piece.InitPiece(pieceStruct.pieceType, pieceStruct.team);
		piece.SetID(pieceStruct.pieceID);
		piece.Ability = pieceStruct.ability;
		if(piece.Team == Team.Blue)
			piece.RotateY(Mathf.DegToRad(180));

		AddChild(piece);
		AssignPiece(piece, board[y][x]);
		piecesList[piece.id] = piece;

		return piece;
	}
	public void KillPiece(PieceKillEvent killEvent){
		ChessPiece piece = killEvent.chessPiece;
		switch(piece.Team){
			case Team.Blue:
				piece.Reparent(graveyardBlue);
			break;

			case Team.Red:
				piece.Reparent(graveyardRed);
			break;

			default:
			break;
		}
		piece.SetPosition(Vector3.Zero);
	}
	public ChessPiece GetPieceById(int id){

		return piecesList[id];
	}
	private ChessPiece GetPiece(Tile tile){
		return GetBoard()[tile.coordinates.Y][tile.coordinates.X].piece;
	}
	private Tile GetTile(ChessPiece piece){
		foreach(BoardStruct[] row in GetBoard()){
			foreach(BoardStruct boardStruct in row){
				if(boardStruct.piece == piece)
					return boardStruct.tile;
			}
		}
		return null;
	}
	public bool DeletePiece(ChessPiece chessPiece){
		if(chessPiece == null) return false;
		if(!piecesList.ContainsKey(chessPiece.id)) return false;
		
		piecesList.Remove(chessPiece.id);
		chessPiece.Free();
		chessPiece = null;
		return true;
	}

	public void RespawnPiece(int x, int y, PieceStruct pieceStruct){
		if(!DeletePiece(GetPieceById(pieceStruct.pieceID)))
			SpawnFigure(x,y,pieceStruct);
	}
	public BoardStruct GetBoardStruct(int X, int Y){
		return GetBoard()[Y][X];
	}

	///Function will return null if tile == null
	public BoardStruct GetBoardStruct(Tile tile){
		if(tile == null) return null;
		return GetBoardStruct(tile.coordinates.Y, tile.coordinates.X);
	}

	public BoardStruct GetBoardStruct(ChessPiece chessPiece){
		return GetBoardStruct(GetTile(chessPiece));
	}

	public abstract PieceStruct[][] GetBoardData();
	public bool AssignPiece(ChessPiece pickup, BoardStruct end){
		try{
		end.piece = pickup;
		pickup.Reparent(end.tile);
		pickup.SetPosition(GetTilePos(end));
		return true;
		}
		catch(Exception e){
			return false;
		}
	}
	public abstract List<Vector2I> FilterMoves(List<Vector2I> moves);	
	protected abstract Vector3 GetTileCenter(int x, int y);
	protected abstract void SpawnBoard();
	protected abstract void TestBoard();
	protected abstract void LoadMaterials();
	public abstract ref BoardStruct[][] GetBoard();
}