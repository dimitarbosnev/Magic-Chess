using Godot;
using System.Collections.Generic;

public partial class RectQueen : Queen
{
    public override List<Vector2I> GetAvailableMoves(ref BoardStruct[][] board)
	{	
		Vector2I coordinates = ChessBoard.Instance.GetBoardStruct(this).tile.coordinates;
		List<Vector2I> r = new List<Vector2I>();
				//right up
		{
		Vector2I direction = new Vector2I(1,1);
		loopMoveCheck(coordinates,direction,ref board,ref r);	
		}
		//right down
		{
		Vector2I direction = new Vector2I(1,-1);
		loopMoveCheck(coordinates,direction,ref board,ref r);	
		}

		//left up
		{
		Vector2I direction = new Vector2I(-1,1);
		loopMoveCheck(coordinates,direction,ref board,ref r);	
		}

		//left down
		{
		Vector2I direction = new Vector2I(-1,-1);
		loopMoveCheck(coordinates,direction,ref board,ref r);	
		}
		// positive vertical
		{
		Vector2I direction = new Vector2I(0,1);
		loopMoveCheck(coordinates,direction,ref board,ref r);	
		}
		// negative vertical
		{
		Vector2I direction = new Vector2I(0,-1);
		loopMoveCheck(coordinates,direction,ref board,ref r);	
		}

		// positive horizontal
		{
		Vector2I direction = new Vector2I(1,0);
		loopMoveCheck(coordinates,direction,ref board,ref r);	
		}

		// negative horizontal
		{
		Vector2I direction = new Vector2I(-1,0);
		loopMoveCheck(coordinates,direction,ref board,ref r);	
		}
		return r;
	}

	public override List<Vector2I> GetAbilityMoves(ref BoardStruct[][] board)
	{
		List<Vector2I> r = new List<Vector2I>();
		return r;
	}
}
