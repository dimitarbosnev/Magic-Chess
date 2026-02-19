using Godot;
using System.Collections.Generic;

public partial class RectCentaur : Centaur
{
    public override List<Vector2I> GetAvailableMoves(ref BoardStruct[][] board)
	{
		Vector2I coordinates = ChessBoard.Instance.GetBoardStruct(this).tile.coordinates;
		List<Vector2I> r = new List<Vector2I>();
		Vector2I move;
		
		//up right
		move = coordinates + new Vector2I(1,2);
			singleMoveCheck(move,ref board,ref r);

		//up left
		move = coordinates + new Vector2I(-1,2);
			singleMoveCheck(move,ref board,ref r);

		//down right
		move = coordinates + new Vector2I(1,-2);
			singleMoveCheck(move,ref board,ref r);


		//down left
		move = coordinates + new Vector2I(-1,-2);
			singleMoveCheck(move,ref board,ref r);

		//right up
		move = coordinates + new Vector2I(2,1);
			singleMoveCheck(move,ref board,ref r);

		//right down
		move = coordinates + new Vector2I(2,-1);
			singleMoveCheck(move,ref board,ref r);

		//left up
		move = coordinates + new Vector2I(-2,1);
			singleMoveCheck(move,ref board,ref r);


		// left down
		move = coordinates + new Vector2I(-2,-1);
			singleMoveCheck(move,ref board,ref r);
					
		return r;
	}

	public override List<Vector2I> GetAbilityMoves(ref BoardStruct[][] board)
	{
		Vector2I coordinates = ChessBoard.Instance.GetBoardStruct(this).tile.coordinates;
		List<Vector2I> r = new List<Vector2I>();
		Vector2I move;
		//up
		move = coordinates + new Vector2I(0,1);
			singleMoveCheck_NoTake(move,ref board,ref r);

		//down
		move = coordinates + new Vector2I(0,-1);
			singleMoveCheck_NoTake(move,ref board,ref r);

		//right
		move = coordinates + new Vector2I(1,0);
			singleMoveCheck_NoTake(move,ref board,ref r);


		//left
		move = coordinates + new Vector2I(-1,0);
			singleMoveCheck_NoTake(move,ref board,ref r);

		//up right
		move = coordinates + new Vector2I(1,1);
			singleMoveCheck_NoTake(move,ref board,ref r);

		//down right
		move = coordinates + new Vector2I(1,-1);
			singleMoveCheck_NoTake(move,ref board,ref r);

		//up left
		move = coordinates + new Vector2I(-1,1);
			singleMoveCheck_NoTake(move,ref board,ref r);


		//down left
		move = coordinates + new Vector2I(-1,-1);
			singleMoveCheck_NoTake(move,ref board,ref r);
		return r;
	}
}
