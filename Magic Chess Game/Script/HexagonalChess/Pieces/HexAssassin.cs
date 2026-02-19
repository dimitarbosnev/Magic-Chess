using Godot;
using System.Collections.Generic;
public partial class HexAssassin : Assassin
{
	public override List<Vector2I> GetAvailableMoves(ref BoardStruct[][] board)
	{
		Vector2I coordinates = ChessBoard.Instance.GetBoardStruct(this).tile.coordinates;

		List<Vector2I> r = new List<Vector2I>();
		for (int y = coordinates.Y-2; y >= 0; y-=2)
		{
			if( y >= 0){ 
				if(board[y+1][coordinates.X].piece != null && board[y+1][coordinates.X + (y % 2 == 0? -1 : 1)].piece != null)
					break;
				else if(board[y][coordinates.X].piece == null){
					r.Add(new Vector2I(coordinates.X, y));
				}
				else if (board[y][coordinates.X].piece.Team != Team){
					r.Add(new Vector2I(coordinates.X, y));
					break;
				}
				else
					break;
			}
			else
				break;
		}

		for (int y = coordinates.Y+2; y < board.Length; y+=2)
		{
			if(y < board.Length){ 
				if(board[y-1][coordinates.X].piece != null && board[y-1][coordinates.X + (y % 2 == 0? -1 : 1)].piece != null)
					break;
				else if(board[y][coordinates.X].piece == null){
					r.Add(new Vector2I(coordinates.X, y));
				}
				else if (board[y][coordinates.X].piece.Team != Team){
					r.Add(new Vector2I(coordinates.X, y));
					break;
				}
				else
					break;
			}
			else
				break;
		}

		int dirX = coordinates.X;
		for (int y = coordinates.Y+1; y < board.Length; y++)
		{
			dirX += y % 2 == 0?-1:-2;
			if(dirX >= 0 && y < board.Length){ 
				if(board[y-1][dirX + (y % 2 == 0?0:1)].piece != null && board[y][dirX+1].piece != null)
					break;
				else if(board[y][dirX].piece == null){
					r.Add(new Vector2I(dirX, y));
				}
				else if (board[y][dirX].piece.Team != Team){
					r.Add(new Vector2I(dirX, y));
					break;
				}
				else
					break;
			}
			else
				break;
		}

		dirX = coordinates.X;
		for (int y = coordinates.Y-1; y >= 0; y--)
		{
			dirX += y % 2 == 0?2:1;
			if(dirX < board[y].Length && y >= 0){ 
				if(board[ y+1][dirX + (y % 2 == 0?-1:0)].piece != null && board[y][dirX-1].piece != null)
					break;
				else if(board[y][dirX].piece == null){
					r.Add(new Vector2I(dirX, y));
				}
				else if (board[y][dirX].piece.Team != Team){
					r.Add(new Vector2I(dirX, y));
					break;
				}
				else
					break;
			}
			else
				break;
		}

		dirX = coordinates.X;
		for (int y = coordinates.Y+1; y < board.Length; y++)
		{
			dirX += y % 2 == 0?2:1;
			if(dirX < board[y].Length && y < board.Length){ 
				if(board[y-1][dirX + (y % 2 == 0?-1:0)].piece != null && board[y][dirX-1].piece != null)
					break;
				else if(board[y][dirX].piece == null){
					r.Add(new Vector2I(dirX, y));
				}
				else if (board[y][dirX].piece.Team != Team){
					r.Add(new Vector2I(dirX, y));
					break;
				}
				else
					break;
			}
			else
				break;
		}

		dirX = coordinates.X;
		for (int y = coordinates.Y-1; y >= 0; y--)
		{
			dirX += y % 2 == 0?-1:-2;
			if(dirX >= 0 && y >= 0){ 
				if(board[ y+1][dirX + (y % 2 == 0?0:1)].piece != null && board[y][dirX+1].piece != null)
					break;
				else if(board[y][dirX].piece == null){
					r.Add(new Vector2I(dirX, y));
				}
				else if (board[y][dirX].piece.Team != Team){
					r.Add(new Vector2I(dirX, y));
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
				if(tile.piece != null && tile.piece.Team == this.Team && tile.piece != this && 
				tile.piece.PieceType != PieceType.HexRedKing && tile.piece.PieceType != PieceType.HexBlueKing)
					r.Add(tile.tile.coordinates);
		return r;
	}
    
}
