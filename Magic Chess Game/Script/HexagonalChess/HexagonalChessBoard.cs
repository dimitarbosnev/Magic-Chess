using Godot;
using System.Collections.Generic;


public partial class HexagonalChessBoard : ChessBoard
{
	protected BoardStruct[][] board = { new BoardStruct[9], new BoardStruct[8], new BoardStruct[9],
							  	 		new BoardStruct[8], new BoardStruct[9], new BoardStruct[8],
							  	 		new BoardStruct[9], new BoardStruct[8], new BoardStruct[9]}; 
	public override void _Ready()
	{
		tileXoffset = .865f;
		tileYoffset = .75f;

		GenerateAllTiles();
		for (int x = 0; x < board.Length; x++)
			for (int y = 0; y < board[x].Length; y++)
				GD.Print("Tile[" + x +"][" + y +"]");
		LoadMaterials();
		SpawnBoard();
		//TestBoard();
	}

	protected override void LoadMaterials()
	{
		int i = 0;
		for (int y = 0; y < board.Length; y ++) {
			for (int x = 0; x < board[y].Length; x++) {
				board[y][x].tile.MaterialOverride = Utils.material[i];
				i += x != board[y].Length - 1? 1 : 0;
				i = i == Utils.material.Length? 0 : i;
			}
		}
	}

	protected override void SpawnBoard(){
		//Team Red
		SpawnFigure(0, 8, PieceType.HexMage, Team.Red);
		SpawnFigure(1, 8, PieceType.HexCentaur, Team.Red);
		SpawnFigure(2, 8, PieceType.HexAssassin, Team.Red);
		SpawnFigure(3, 8, PieceType.HexRedKing, Team.Red);
		SpawnFigure(4, 8, PieceType.HexAssassin, Team.Red);
		SpawnFigure(5, 8, PieceType.HexQueen, Team.Red);
		SpawnFigure(6, 8, PieceType.HexAssassin, Team.Red);
		SpawnFigure(7, 8, PieceType.HexCentaur, Team.Red);
		SpawnFigure(8, 8, PieceType.HexShapeshifter, Team.Red);
		SpawnFigure(0, 7, PieceType.HexSoldier, Team.Red);
		SpawnFigure(1, 7, PieceType.HexSoldier, Team.Red);
		SpawnFigure(2, 7, PieceType.HexSoldier, Team.Red);
		SpawnFigure(3, 7, PieceType.HexSoldier, Team.Red);
		SpawnFigure(4, 7, PieceType.HexSoldier, Team.Red);
		SpawnFigure(5, 7, PieceType.HexSoldier, Team.Red);
		SpawnFigure(6, 7, PieceType.HexSoldier, Team.Red);
		SpawnFigure(7, 7, PieceType.HexSoldier, Team.Red);

		//Team Blue
		SpawnFigure(0, 0, PieceType.HexMage, Team.Blue);
		SpawnFigure(1, 0, PieceType.HexCentaur, Team.Blue);
		SpawnFigure(2, 0, PieceType.HexAssassin, Team.Blue);
		SpawnFigure(3, 0, PieceType.HexBlueKing, Team.Blue);
		SpawnFigure(4, 0, PieceType.HexAssassin, Team.Blue);
		SpawnFigure(5, 0, PieceType.HexQueen, Team.Blue);
		SpawnFigure(6, 0, PieceType.HexAssassin, Team.Blue);
		SpawnFigure(7, 0, PieceType.HexCentaur, Team.Blue);
		SpawnFigure(8, 0, PieceType.HexShapeshifter, Team.Blue);
		SpawnFigure(0, 1, PieceType.HexSoldier, Team.Blue);
		SpawnFigure(1, 1, PieceType.HexSoldier, Team.Blue);
		SpawnFigure(2, 1, PieceType.HexSoldier, Team.Blue);
		SpawnFigure(3, 1, PieceType.HexSoldier, Team.Blue);
		SpawnFigure(4, 1, PieceType.HexSoldier, Team.Blue);
		SpawnFigure(5, 1, PieceType.HexSoldier, Team.Blue);
		SpawnFigure(6, 1, PieceType.HexSoldier, Team.Blue);
		SpawnFigure(7, 1, PieceType.HexSoldier, Team.Blue);
	}

    protected override void TestBoard() {

    	//Team Red
    	//OG: 0,8
    	//SpawnFigure(0,8, PieceType.Mage, Team.Red);
    	//SpawnFigure(4, 4, PieceType.Centaur, Team.Red);
    	//SpawnFigure(2, 8, PieceType.Assasin, Team.Red);
    	//SpawnFigure(4, 4, PieceType.RedKing, Team.Red);
    	SpawnFigure(4, 4, PieceType.HexBlueKing, Team.Red);
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
		SpawnFigure(5, 0, PieceType.HexQueen, Team.Blue);
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

	public override ref BoardStruct[][] GetBoard(){
		return ref board;
	}

	protected override Vector3 GetTileCenter(int x, int y)
	{
		if (y % 2 != 0)
			return new Vector3((x * tileXoffset) + 0.433f, 0, y * tileYoffset);
		else
			return new Vector3(x * tileXoffset, 0, y * tileYoffset);
	}

	protected override Tile GenerateSingleTiles(int x, int y)
	{
		var tile = Utils.hexTileScene.Instantiate() as Tile;
		tile.Position = GetTileCenter(x, y);
		tile.InitTile(x, y);
		AddChild(tile);
		return tile;
	}
	public override PieceStruct[][] GetBoardData(){
		BoardStruct[][] board = GetBoard();
		PieceStruct[][] boardData = { new PieceStruct[9], new PieceStruct[8], new PieceStruct[9],
							  	 	 new PieceStruct[8], new PieceStruct[9], new PieceStruct[8],
							  	 	 new PieceStruct[9], new PieceStruct[8], new PieceStruct[9] }; 
		for(int y = 0; y < board.Length; y++)
			for(int x = 0; x < board[y].Length; x++)
			   	boardData[y][x] = SharedUtils.SetPieceStruct(board[y][x]);

		return boardData;
	}
	public override List<Vector2I> FilterMoves(List<Vector2I> moves)
	{
		List<Vector2I> movesToRemove = new List<Vector2I>();
		foreach(Vector2I move in moves){
			BoardStruct boardStruct = GetBoard()[move.Y][move.X];
			if(boardStruct.piece != null && boardStruct.piece.PieceType == PieceType.HexBlueKing && (boardStruct.piece as HexBlueKing).protection)
				movesToRemove.Add(boardStruct.tile.coordinates);		
		}
		
		foreach(Vector2I move in movesToRemove)
			moves.Remove(move);
		return moves;
	}
}

