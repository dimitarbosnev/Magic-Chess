using Godot;
using System.Collections.Generic;
public partial class RectShapeshifter : Shapeshifter
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
		return r;
	}
	public override List<Vector2I> GetAbilityMoves(ref BoardStruct[][] board)
	{
		List<Vector2I> r = new List<Vector2I>();
		foreach(BoardStruct[] row in board)
			foreach(BoardStruct tile in row)
				if(tile.piece != null && tile.piece != this &&
				   tile.piece.PieceType != PieceType.RectRedKing && tile.piece.PieceType != PieceType.RectBlueKing)
					r.Add(tile.tile.coordinates);
		return r;
	}
}
