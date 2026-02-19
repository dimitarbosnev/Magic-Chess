using Godot;
using System.Collections.Generic;
public partial class RectSoldier : Soldier
{
    public override List<Vector2I> GetAvailableMoves(ref BoardStruct[][] board)
	{
		Vector2I coordinates = ChessBoard.Instance.GetBoardStruct(this).tile.coordinates;

		int directionY = Team == Team.Blue ? 1 : -1;
		List<Vector2I> r = new List<Vector2I>();

		Vector2I move = coordinates + new Vector2I(0,directionY);
		singleMoveCheck_NoTake(move,ref board, ref r);
		
		move = coordinates + new Vector2I(1,directionY);
		singleMoveCheck_OnlyTake(move,ref board, ref r);

		move = coordinates + new Vector2I(-1,directionY);
		singleMoveCheck_OnlyTake(move,ref board, ref r);

		move = coordinates + new Vector2I(0,directionY*2);
		if((coordinates.Y == 1 && Team == Team.Red) || (coordinates.Y == 6 && Team == Team.Blue))
			if(boundryCheck(move,board) && board[move.Y][move.X].piece == null && board[move.Y-1][move.X].piece == null)
				r.Add(move);

		return r;
	}

	public override List<Vector2I> GetAbilityMoves(ref BoardStruct[][] board)
	{
		List<Vector2I> r = new List<Vector2I>();
		return r;
	}
}
