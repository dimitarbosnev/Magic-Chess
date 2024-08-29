using Godot;
using System.Collections.Generic;

public partial class Queen : ChessPiece
{
    public override List<Vector2I> GetAvailableMoves(ref Tile[][] board)
	{
		List<Vector2I> r = new List<Vector2I>();
		for (int y = coordinates.Y-2; y >= 0; y-=2)
		{
			if( y >= 0){ 
				if(board[y+1][coordinates.X].piece != null && board[y+1][coordinates.X-1].piece != null)
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
				if(board[y-1][coordinates.X].piece != null && board[y-1][coordinates.X-1].piece != null)
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

		int x = coordinates.X;
		for (int y = coordinates.Y+1; y < board.Length; y++)
		{
			x += y % 2 == 0?-1:-2;
			if(x >= 0 && y < board.Length){ 
				if(board[y-1][x + (y % 2 == 0?0:1)].piece != null && board[y][x+1].piece != null)
					break;
				else if(board[y][x].piece == null){
					r.Add(new Vector2I(x, y));
				}
				else if (board[y][x].piece.Team != Team){
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
			x += y % 2 == 0?2:1;
			if(x < board[y].Length && y >= 0){ 
				if(board[ y+1][x + (y % 2 == 0?-1:0)].piece != null && board[y][x-1].piece != null)
					break;
				else if(board[y][x].piece == null){
					r.Add(new Vector2I(x, y));
				}
				else if (board[y][x].piece.Team != Team){
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
			x += y % 2 == 0?2:1;
			if(x < board[y].Length && y < board.Length){ 
				if(board[y-1][x + (y % 2 == 0?-1:0)].piece != null && board[y][x-1].piece != null)
					break;
				else if(board[y][x].piece == null){
					r.Add(new Vector2I(x, y));
				}
				else if (board[y][x].piece.Team != Team){
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
			x += y % 2 == 0?-1:-2;
			if(x >= 0 && y >= 0){ 
				if(board[ y+1][x + (y % 2 == 0?0:1)].piece != null && board[y][x+1].piece != null)
					break;
				else if(board[y][x].piece == null){
					r.Add(new Vector2I(x, y));
				}
				else if (board[y][x].piece.Team != Team){
					r.Add(new Vector2I(x, y));
					break;
				}
				else
					break;
			}
			else
				break;
		}

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

		x = coordinates.X;
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

	public override List<Vector2I> GetAbilityMoves(ref Tile[][] board)
	{
		List<Vector2I> r = new List<Vector2I>();
		return r;
	}
    public override Command AbilityMove(Tile target){
        return new MoveCommand(this, target);
    }
	public override void OnHoverInput(PlayerFSM playerFSM, InputEventMouseButton mouseEvent){
		if(mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
				playerFSM.TransitToState(typeof(PlayerNormalHoldState));
	}
}
