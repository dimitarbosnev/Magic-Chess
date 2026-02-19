using Godot;
using System.Collections.Generic;

public partial class HexCentaur : Centaur
{
    public override List<Vector2I> GetAvailableMoves(ref BoardStruct[][] board)
	{
		Vector2I coordinates = ChessBoard.Instance.GetBoardStruct(this).tile.coordinates;
		
		List<Vector2I> r = new List<Vector2I>();
		int y;
		int dirX = coordinates.Y % 2 == 0? -1 : 1;
		int x = coordinates.X;
		
		//"+" straight
		y = coordinates.Y + 3;
		if(y < board.Length){
		 if(x >= 0 && x < board[y].Length && (board[y][x].piece == null || board[y][x].piece.Team != Team))
			r.Add(new Vector2I(x,y));
		if(x + dirX >= 0 && x + dirX < board[y].Length && (board[y][x].piece == null || board[y][x].piece.Team != Team))
			r.Add(new Vector2I(x+dirX,y));
		}

		//"-" straight
		y = coordinates.Y -3;
		if(y >= 0){
		 if(x >= 0 && x < board[y].Length && (board[y][x].piece == null || board[y][x].piece.Team != Team))
			r.Add(new Vector2I(x,y));
		if(x + dirX >= 0 && x + dirX < board[y].Length && (board[y][x].piece == null || board[y][x].piece.Team != Team))
			r.Add(new Vector2I(x+dirX,y));
		}

		// "+" diagnal
		y = coordinates.Y + 1;
		if(y < board.Length){
			dirX = coordinates.Y % 2 == 0? 2 : 3;
			if(x + dirX < board[y].Length && (board[y][x].piece == null || board[y][x].piece.Team != Team)) 
				r.Add(new Vector2I(x+dirX,y));
			dirX = coordinates.Y % 2 == 0? -3 : -2;
			if(x + dirX >= 0 && (board[y][x].piece == null || board[y][x].piece.Team != Team))
				r.Add(new Vector2I(x+dirX,y));
		}

		y = coordinates.Y + 2;
		if(y < board.Length){
			dirX = 2;
			if(x + dirX < board[y].Length && (board[y][x].piece == null || board[y][x].piece.Team != Team))
				r.Add(new Vector2I(x+dirX,y));
			dirX = -2;
			if(x + dirX >= 0 && (board[y][x].piece == null || board[y][x].piece.Team != Team))
				r.Add(new Vector2I(x+dirX,y));
		}
		// "-" diagnal
		y = coordinates.Y - 1;
		if(y >= 0){
			dirX = coordinates.Y % 2 == 0? 2 : 3;
			if(x + dirX < board[y].Length && (board[y][x].piece == null || board[y][x].piece.Team != Team))
				r.Add(new Vector2I(x+dirX,y));
			dirX = coordinates.Y % 2 == 0? -3 : -2;
			if(x + dirX >= 0 && (board[y][x].piece == null || board[y][x].piece.Team != Team))
				r.Add(new Vector2I(x+dirX,y));
		}

		y = coordinates.Y - 2;
		if(y >= 0){
		dirX = 2;
		if(x + dirX < board[y].Length && (board[y][x].piece == null || board[y][x].piece.Team != Team))
			r.Add(new Vector2I(x+dirX,y));
		dirX = -2;
		if(x + dirX >= 0 && (board[y][x].piece == null || board[y][x].piece.Team != Team)) 
			r.Add(new Vector2I(x+dirX,y));
		}
		return r;
	}

	public override List<Vector2I> GetAbilityMoves(ref BoardStruct[][] board)
	{
		Vector2I coordinates = ChessBoard.Instance.GetBoardStruct(this).tile.coordinates;

		List<Vector2I> r = new List<Vector2I>();
		int y;
		int x;

		//"+" straight
		y =  coordinates.Y + 2;
		x = coordinates.X;
		if(y < board.Length && board[y][x].piece == null)
			r.Add(new Vector2I(x,y));
		//"-" straight
		y = coordinates.Y - 2;
		if(y >= 0 && board[y][x].piece == null)
			r.Add(new Vector2I(x,y));
		
		y = coordinates.Y + 1;

		if(y < board.Length){
			x = coordinates.X + (coordinates.Y % 2 == 0?-2:-1);
			if(x >= 0 && board[y][x].piece == null)
				r.Add(new Vector2I(x,y));

			x = coordinates.X + (coordinates.Y % 2 == 0? 1:2);
			if(x >= 0 && board[y][x].piece == null)
				r.Add(new Vector2I(x,y));
		}

		y = coordinates.Y - 1;
		if(y >= 0){
			x = coordinates.X + (coordinates.Y % 2 == 0?-2:-1);
			if(x >= 0 && board[y][x].piece == null)
				r.Add(new Vector2I(x,y));

			x = coordinates.X + (coordinates.Y % 2 == 0? 1:2);
			if(x >= 0 && board[y][x].piece == null)
				r.Add(new Vector2I(x,y));
		}

		return r;
	}

}
