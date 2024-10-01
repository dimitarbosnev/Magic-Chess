using Godot;
using System.Collections.Generic;

public partial class ChessBoard : Node3D
{
	[Export] private Node3D graveyardBlue = null;
	[Export] private Node3D graveyardRed = null;
	public static Tile[][] board = { new Tile[9],new Tile[8],new Tile[9],
							  new Tile[8],new Tile[9],new Tile[8],
							  new Tile[9],new Tile[8],new Tile[9]};					
	private List<ChessPiece> deadPieces = new List<ChessPiece>(); // TODO: handle for reversing a command
	private static Stack<Command> executedCommands = new Stack<Command>();
	private float tileXoffset = .865f;
	private float tileYoffset = .75f;
	public static Team teamTurn{get; private set;}
	
	public override void _Ready()
	{
		GenerateAllTiles();
		for (int x = 0; x < board.Length; x++)
			for (int y = 0; y < board[x].Length; y++)
				GD.Print("Tile[" + x +"][" + y +"]");
		LoadMaterials();
		SpawnBoard();
		//TestBoard();
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
	public override void _Process(double delta)
	{
	}

	public static void AssignPiece(ChessPiece piece, Tile tile){
		tile.piece = piece;
	}

	private void ExecuteCommand(Command command){
		command.execute(ref board);
		executedCommands.Push(command);
	}

	public static Command GetCommandPeek(){
		return executedCommands.Peek();
	}

	private void OnCommandRecived(CommandMessageRecived e){
		ClearTiles();
		ExecuteCommand(e.command);
	}

	private void ClearTiles(){
		for (int y = 0; y < board.Length; y++)
			for (int x = 0; x < board[y].Length; x++)
				board[y][x].RemoveHighlight();
	}
	private Vector3 GetTileCenter(int x, int y)
	{
		if (y % 2 != 0)
			return new Vector3((x * tileXoffset) + 0.433f, 0, y * tileYoffset);
		else
			return new Vector3(x * tileXoffset, 0, y * tileYoffset);
	}

	private Vector3 GetTilePos(int x, int y)
	{
		return board[x][y].Position;
	}
	private Tile GenerateSingleTiles(int x, int y)
	{
		var tile = Resources.tileScene.Instantiate() as Tile;
		tile.Position = GetTileCenter(x, y);
		tile.InitTile(x, y);
		AddChild(tile);
		return tile;
	}

	private void GenerateAllTiles()
	{
		for (int y = 0; y < board.Length; y++)
			for (int x = 0; x < board[y].Length; x++)
				board[y][x] = GenerateSingleTiles(x, y);			
	}

	private void LoadMaterials()
	{
		int i = 0;
		for (int y = 0; y < board.Length; y ++) {
			for (int x = 0; x < board[y].Length; x++) {
				board[y][x].MaterialOverride = Resources.material[i];
				i += x != board[y].Length - 1? 1 : 0;
				i = i == Resources.material.Length? 0 : i;
			}
		}
	}
	private void SpawnFigure(int X, int Y, PieceType type, Team team)
	{
		var piece = Resources.pieceCollection[type].Instantiate() as ChessPiece;
		AddChild(piece);
		piece.InitPiece(type, team);
		AssignPiece(piece,board[Y][X]);
		if(team == Team.Blue)
			piece.RotateY(Mathf.DegToRad(180));
	}

	public void KillPiece(PieceKillEvent killEvent){
		ChessPiece piece = killEvent.chessPiece;
		deadPieces.Add(piece);
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
		piece.tile = null;
		piece.targetPosition = Vector3.Zero;
	}
	private void SpawnBoard(){
		//Team Red
		SpawnFigure(0, 8, PieceType.Mage, Team.Red);
		SpawnFigure(1, 8, PieceType.Centaur, Team.Red);
		SpawnFigure(2, 8, PieceType.Assassin, Team.Red);
		SpawnFigure(3, 8, PieceType.RedKing, Team.Red);
		SpawnFigure(4, 8, PieceType.Assassin, Team.Red);
		SpawnFigure(5, 8, PieceType.Queen, Team.Red);
		SpawnFigure(6, 8, PieceType.Assassin, Team.Red);
		SpawnFigure(7, 8, PieceType.Centaur, Team.Red);
		SpawnFigure(8, 8, PieceType.Shapeshifter, Team.Red);
		SpawnFigure(0, 7, PieceType.Soldier, Team.Red);
		SpawnFigure(1, 7, PieceType.Soldier, Team.Red);
		SpawnFigure(2, 7, PieceType.Soldier, Team.Red);
		SpawnFigure(3, 7, PieceType.Soldier, Team.Red);
		SpawnFigure(4, 7, PieceType.Soldier, Team.Red);
		SpawnFigure(5, 7, PieceType.Soldier, Team.Red);
		SpawnFigure(6, 7, PieceType.Soldier, Team.Red);
		SpawnFigure(7, 7, PieceType.Soldier, Team.Red);

		//Team Blue
		SpawnFigure(0, 0, PieceType.Mage, Team.Blue);
		SpawnFigure(1, 0, PieceType.Centaur, Team.Blue);
		SpawnFigure(2, 0, PieceType.Assassin, Team.Blue);
		SpawnFigure(3, 0, PieceType.BlueKing, Team.Blue);
		SpawnFigure(4, 0, PieceType.Assassin, Team.Blue);
		SpawnFigure(5, 0, PieceType.Queen, Team.Blue);
		SpawnFigure(6, 0, PieceType.Assassin, Team.Blue);
		SpawnFigure(7, 0, PieceType.Centaur, Team.Blue);
		SpawnFigure(8, 0, PieceType.Shapeshifter, Team.Blue);
		SpawnFigure(0, 1, PieceType.Soldier, Team.Blue);
		SpawnFigure(1, 1, PieceType.Soldier, Team.Blue);
		SpawnFigure(2, 1, PieceType.Soldier, Team.Blue);
		SpawnFigure(3, 1, PieceType.Soldier, Team.Blue);
		SpawnFigure(4, 1, PieceType.Soldier, Team.Blue);
		SpawnFigure(5, 1, PieceType.Soldier, Team.Blue);
		SpawnFigure(6, 1, PieceType.Soldier, Team.Blue);
		SpawnFigure(7, 1, PieceType.Soldier, Team.Blue);
	}

	private void TestBoard(){
		//Team Red
		//OG: 0,8
		//SpawnFigure(0,8, PieceType.Mage, Team.Red);
		//SpawnFigure(4, 4, PieceType.Centaur, Team.Red);
		//SpawnFigure(2, 8, PieceType.Assasin, Team.Red);
		//SpawnFigure(4, 4, PieceType.RedKing, Team.Red);
		SpawnFigure(4, 4, PieceType.BlueKing, Team.Red);
		//SpawnFigure(4, 8, PieceType.Assasin, Team.Red);
		//SpawnFigure(5, 8, PieceType.Queen, Team.Red);
		//OG:6,8
		//SpawnFigure(4,4, PieceType.Assasin, Team.Red);
		//SpawnFigure(3, 3, PieceType.Soldier, Team.Red);
		//SpawnFigure(4, 3, PieceType.Soldier, Team.Red);
		//SpawnFigure(3, 4, PieceType.Soldier, Team.Red);
		//SpawnFigure(3, 5, PieceType.Soldier, Team.Red);
		//SpawnFigure(4, 5, PieceType.Soldier, Team.Red);
		//SpawnFigure(5, 4, PieceType.Soldier, Team.Red);
		//SpawnFigure(6, 7, PieceType.Soldier, Team.Red);
		//SpawnFigure(7, 7, PieceType.Soldier, Team.Red);

		//SpawnFigure(7, 8, PieceType.Centaur, Team.Red);
		//SpawnFigure(8, 8, PieceType.Shapeshifter, Team.Red);
		//SpawnFigure(3, 3, PieceType.Soldier, Team.Red);
		//SpawnFigure(4, 3, PieceType.Soldier, Team.Red);
		//SpawnFigure(2, 7, PieceType.Soldier, Team.Red);
		//SpawnFigure(3, 7, PieceType.Soldier, Team.Red);
		//SpawnFigure(4, 7, PieceType.Soldier, Team.Red);
		//SpawnFigure(5, 7, PieceType.Soldier, Team.Red);
		//SpawnFigure(6, 7, PieceType.Soldier, Team.Red);
		//SpawnFigure(7, 7, PieceType.Soldier, Team.Red);

		////Team Blue
		//SpawnFigure(0, 0, PieceType.Mage, Team.Blue);
		//SpawnFigure(1, 0, PieceType.Centaur, Team.Blue);
		//SpawnFigure(2, 0, PieceType.Assasin, Team.Blue);
		//SpawnFigure(3, 0, PieceType.BlueKing, Team.Blue);
		//SpawnFigure(4, 0, PieceType.Assasin, Team.Blue);
		SpawnFigure(5, 0, PieceType.Queen, Team.Blue);
		//SpawnFigure(6, 0, PieceType.Assasin, Team.Blue);
		//SpawnFigure(7, 0, PieceType.Centaur, Team.Blue);
		//SpawnFigure(8, 0, PieceType.Shapeshifter, Team.Blue);
		//SpawnFigure(0, 1, PieceType.Soldier, Team.Blue);
		//SpawnFigure(1, 1, PieceType.Soldier, Team.Blue);
		//SpawnFigure(2, 1, PieceType.Soldier, Team.Blue);
		//SpawnFigure(3, 1, PieceType.Soldier, Team.Blue);
		//SpawnFigure(4, 1, PieceType.Soldier, Team.Blue);
		//SpawnFigure(5, 1, PieceType.Soldier, Team.Blue);
		//SpawnFigure(6, 1, PieceType.Soldier, Team.Blue);
		//SpawnFigure(7, 1, PieceType.Soldier, Team.Blue);
	}

	public static List<Vector2I> FilterMoves(List<Vector2I> moves)
	{
		List<Vector2I> movesToRemove = new List<Vector2I>();
		foreach(Vector2I move in moves){
			Tile tile = board[move.Y][move.X];
			if(tile.piece != null && tile.piece.PieceType == PieceType.BlueKing && (tile.piece as BlueKing).protection)
				movesToRemove.Add(tile.coordinates);		
		}


		
		foreach(Vector2I move in movesToRemove)
			moves.Remove(move);
		return moves;
	}	

	/*public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey eventKey && eventKey.Pressed)
			switch (eventKey.Keycode){
				case Key.Z:
					executedCommands.Pop().reverse();//ref board);
				break;
				case Key.Escape:
					GetTree().Quit();
				break;
				case Key.X:
					EventBus<NewTurnEvent>.Invoke(new NewTurnEvent(teamTurn));
				break;
			}
	}*/
}