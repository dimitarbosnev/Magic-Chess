using Godot;
using System.Collections.Generic;
public partial class HexShapeshifter : Shapeshifter
{
    public override List<Vector2I> GetAvailableMoves(ref BoardStruct[][] board)
	{
		Vector2I coordinates = ChessBoard.Instance.GetBoardStruct(this).tile.coordinates;

		List<Vector2I> r = new List<Vector2I>();

		if(coordinates.X > 0)
		for (int a = coordinates.X - 1; a >=0; a--){
			if(board[coordinates.Y][a].piece == null)
				r.Add(new Vector2I(a,coordinates.Y));
			else if(board[coordinates.Y][a].piece.Team != Team){
				r.Add(new Vector2I(a, coordinates.Y));
				break;
			}
			else
				break;
		}

		if(coordinates.X < board[coordinates.Y].Length)
		for (int a = coordinates.X + 1; a < board[coordinates.Y].Length; a++){
			if(board[coordinates.Y][a].piece == null)
				r.Add(new Vector2I(a,coordinates.Y));
			else if(board[coordinates.Y][a].piece.Team != Team){
				r.Add(new Vector2I(a, coordinates.Y));
				break;
			}
			else
				break;
		}

		int x = coordinates.X;
		for (int y = coordinates.Y-1; y >= 0; y--)
		{
			x += y % 2 == 0?1:0;
			if(x < board[y].Length){
				if(board[y][x].piece == null)
					r.Add(new Vector2I(x, y));
				else if( board[y][x].piece.Team != Team){
					r.Add(new Vector2I(x, y));
					break;
				}
				else
					break;
			}
			else
				break;
		}

		x = coordinates.X;
		for (int y = coordinates.Y-1; y >= 0; y--)
		{
			x += y % 2 == 0?0:-1;
			if( x >= 0){
				if(board[y][x].piece == null)
					r.Add(new Vector2I(x, y));
				else if( board[y][x].piece.Team != Team){
					r.Add(new Vector2I(x, y));
					break;
				}
				else
					break;
			}
			else
				break;
		}

		x = coordinates.X;
		for (int y = coordinates.Y+1; y < board.Length; y++)
		{
			x += y % 2 == 0?0:-1;
			if( x >= 0){
				if(board[y][x].piece == null)
					r.Add(new Vector2I(x, y));
				else if( board[y][x].piece.Team != Team){
					r.Add(new Vector2I(x, y));
					break;
				}
				else
					break;
			}
			else
				break;
		}

		x = coordinates.X;
		for (int y = coordinates.Y+1; y < board.Length; y++)
		{
			x += y % 2 == 0?1:0;
			if(x < board[y].Length){
				if(board[y][x].piece == null)
					r.Add(new Vector2I(x, y));
				else if( board[y][x].piece.Team != Team){
					r.Add(new Vector2I(x, y));
					break;
				}
				else
					break;
			}
			else
				break;
		}
		
		return r;
	}
	public override List<Vector2I> GetAbilityMoves(ref BoardStruct[][] board)
	{
		List<Vector2I> r = new List<Vector2I>();
		foreach(BoardStruct[] row in board)
			foreach(BoardStruct tile in row)
				if(tile.piece != null && tile.piece != this &&
				   tile.piece.PieceType != PieceType.HexRedKing && tile.piece.PieceType != PieceType.HexBlueKing)
					r.Add(tile.tile.coordinates);
		return r;
	}

}
