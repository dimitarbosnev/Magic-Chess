using Godot;
using System.Collections.Generic;
public partial class HexBlueKing : BlueKing
{
    public override List<Vector2I> GetAvailableMoves(ref BoardStruct[][] board)
	{
		Vector2I coordinates = ChessBoard.Instance.GetBoardStruct(this).tile.coordinates;
		
		//Big fat mess do not touch unless you have nerves to fix it after
		List<Vector2I> r = new List<Vector2I>();
		int y;
		int x;

		//"+" straight
		#region diagonals
		y = coordinates.Y + 2;
		x = coordinates.X + (coordinates.Y % 2 == 0? -1:1);
		if(y < board.Length && 
		(board[y][coordinates.X].piece == null || board[y][coordinates.X].piece.Team != Team) &&
			((coordinates.X < board[y-1].Length && board[y-1][coordinates.X].piece == null) || board[y-1][x].piece == null))
			r.Add(new Vector2I(coordinates.X,y));
	
		
		//"-" straight
		y = coordinates.Y - 2;
		if(y >= 0 && (board[y][coordinates.X].piece == null || board[y][coordinates.X].piece.Team != Team) &&
			((coordinates.X < board[y+1].Length && board[y+1][coordinates.X].piece == null) || board[y+1][x].piece == null))
			r.Add(new Vector2I(coordinates.X,y));

		//bottom 
		int dirX = coordinates.X + (coordinates.Y % 2 == 0? -1:0);
		y = coordinates.Y + 1;
		if(y < board.Length){
			//bottom left
			x = coordinates.X + (coordinates.Y % 2 == 0?-2:-1);
			if(x >= 0 && (board[coordinates.Y][coordinates.X-1].piece == null || board[y][dirX].piece == null) &&
			(board[y][x].piece == null || board[y][x].piece.Team != Team))
				r.Add(new Vector2I(x,y));
			
			//bottom right
			dirX = coordinates.X + (coordinates.Y % 2 == 0? 0:1);
			x = coordinates.X + (coordinates.Y % 2 == 0? 1:2);
			if(x <board[y].Length && (board[coordinates.Y][coordinates.X+1].piece == null || board[y][dirX].piece == null) && 
			(board[y][x].piece == null || board[y][x].piece.Team != Team))
				r.Add(new Vector2I(x,y));
		}

		//top
		y = coordinates.Y - 1;
		if(y >= 0){
			//top left
			dirX = coordinates.X + (coordinates.Y % 2 == 0? -1:0);
			x = coordinates.X + (coordinates.Y % 2 == 0?-2:-1);
			if(x >= 0 && (board[coordinates.Y][coordinates.X-1].piece == null || board[y][dirX].piece == null) &&
			(board[coordinates.Y][coordinates.X-1].piece == null || board[y][x].piece.Team != Team))
				r.Add(new Vector2I(x,y));
			//top right
			dirX = coordinates.X + (coordinates.Y % 2 == 0? 0:1);
			x = coordinates.X + (coordinates.Y % 2 == 0? 1:2);
			if(x <board[y].Length && (board[coordinates.Y][coordinates.X+1].piece == null || board[y][dirX].piece == null) && 
			(board[y][x].piece == null || board[y][x].piece.Team != Team))
				r.Add(new Vector2I(x,y));
		}
		#endregion
		#region neibouring
		//bottom 
		y = coordinates.Y + 1;
		dirX = coordinates.X + (coordinates.Y % 2 == 0? -1:1);
		if(y < board.Length){
			if(coordinates.X < board[y].Length && (board[y][coordinates.X].piece == null || board[y][coordinates.X].piece.Team != Team))
				r.Add(new Vector2I(coordinates.X,y));
			if(dirX >= 0 && dirX <= board[y].Length && (board[y][dirX].piece == null || board[y][dirX].piece.Team != Team))
				r.Add(new Vector2I(dirX,y));
		}
		//top
		y = coordinates.Y - 1;
		if(y >= 0){
			if(coordinates.X < board[y].Length && (board[y][coordinates.X].piece == null || board[y][coordinates.X].piece.Team != Team))
				r.Add(new Vector2I(coordinates.X,y));
			if(dirX >= 0 && dirX <= board[y].Length && (board[y][dirX].piece == null || board[y][dirX].piece.Team != Team))
				r.Add(new Vector2I(dirX,y));
		}
		//left
		 if(coordinates.X - 1 >= 0 && 
		 (board[coordinates.Y][coordinates.X-1].piece == null || board[coordinates.Y][coordinates.X-1].piece.Team != Team))
			r.Add(new Vector2I(coordinates.X-1,coordinates.Y));

		//right
		if(coordinates.X + 1 < board[coordinates.Y].Length && 
		 (board[coordinates.Y][coordinates.X+1].piece == null || board[coordinates.Y][coordinates.X+1].piece.Team != Team))
			r.Add(new Vector2I(coordinates.X+1,coordinates.Y));
		
		#endregion
		return r;
	}

	public override List<Vector2I> GetAbilityMoves(ref BoardStruct[][] board)
	{
		Vector2I coordinates = ChessBoard.Instance.GetBoardStruct(this).tile.coordinates;

		List<Vector2I> r = new List<Vector2I>();
		foreach(BoardStruct[] row in board)
			foreach(BoardStruct tile in row)
				if(tile.piece != null && tile.piece.GetAvailableMoves(ref board).Contains(coordinates))
					r.Add(coordinates);

		return r;
	}
  
}
