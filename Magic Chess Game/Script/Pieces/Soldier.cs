using Godot;
using System.Collections.Generic;
public partial class Soldier : ChessPiece
{
    public override bool Ability{
		get{ return Team == Team.Blue ? coordinates.Y == 1? true:false : coordinates.Y == 7? true:false ; }
		set{} 
	}
    public override List<Vector2I> GetAvailableMoves(ref Tile[][] board)
	{
		int directionY = Team == Team.Blue ? 1 : -1;
		int directionX = coordinates.Y % 2 == 0?-1:1;
		List<Vector2I> r = new List<Vector2I>();

		Vector2I move = coordinates + new Vector2I(0,directionY);
		if(move.Y >= 0 && move.Y <= 8 && move.X <= 7 && (board[move.Y][move.X].piece == null || board[move.Y][move.X].piece.Team != Team))
			r.Add(move);
		
		move = coordinates + new Vector2I(directionX,directionY);
		if(move.Y >= 0 && move.Y <= 8 && move.X >= 0 && (board[move.Y][move.X].piece == null || board[move.Y][move.X].piece.Team != Team))
			r.Add(move);
		
		return r;
	}

	public override List<Vector2I> GetAbilityMoves(ref Tile[][] board)
	{
		int directionY = Team == Team.Blue ? 1 : -1;
		int directionX = coordinates.Y % 2 == 0?-1:1;
		Vector2I move = coordinates + new Vector2I(0,directionY);
		Vector2I move2 = coordinates + new Vector2I(directionX,directionY);
		List<Vector2I> r = new List<Vector2I>();
		if(board[move.Y][move.X].piece == null && board[move2.Y][move2.X].piece == null)
			r.Add(coordinates + new Vector2I(0,directionY+directionY));
		return r;
	}

    public override Command AbilityMove(Tile target)
    {
        return new MoveCommand(this, target);
    }
}
