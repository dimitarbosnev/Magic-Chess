using Godot;
using System.Collections.Generic;

public partial class Centaur : ChessPiece
{
    public override Command AbilityMove(BoardStruct target){
        return new RepositionCommand(SharedUtils.SetTileStruct(this), SharedUtils.SetTileStruct(target));
    }

	public class RepositionCommand : Command{
		public TileStruct pickup{get; protected set;}
		public TileStruct target{get; protected set;}
		private ChessPiece chessPiece;
		public RepositionCommand() : base(){}
    	public RepositionCommand(TileStruct pPickup, TileStruct pTarget){
        	pickup = pPickup;
        	target = pTarget;
    	}

		public override void execute(ref BoardStruct[][] board){
			BoardStruct targetTile = board[target.y][target.x];

			if(pickup.piece != null && target.piece != null){
            	ChessPiece chessPiece1 = ChessBoard.Instance.GetPieceById(pickup.piece.pieceID);
            	ChessBoard.Instance.AssignPiece(chessPiece1, targetTile);

				chessPiece1.Ability = false;
				EventBus<NewTurnEvent>.Invoke(new NewTurnEvent(chessPiece1.Team));
        	}
		}

        public override void reverse(ref BoardStruct[][] board){
			BoardStruct pickupTile = board[pickup.y][pickup.x];

			if(pickup.piece != null){
            	ChessPiece chessPiece1 = ChessBoard.Instance.GetPieceById(pickup.piece.pieceID);
            	ChessBoard.Instance.AssignPiece(chessPiece1, pickupTile);
        	}
        }

		public override bool validateMove(ref PieceStruct[][] boardData){
			bool valid1 = false;
        	bool valid2 = false;

			if(pickup.piece == null || boardData[pickup.y][pickup.x] == null) return false;
			if(target.piece != null || boardData[target.y][target.x] != null) return false;

        	if(SharedUtils.ComparePieceStructs(pickup.piece,boardData[pickup.y][pickup.x]))
        	    valid1 = true;
        	if(SharedUtils.ComparePieceStructs(target.piece,boardData[target.y][target.x]))
        	    valid2 = true;
				
        	return valid1 && valid2;
    	}
    	public override void updateBoardData(ref PieceStruct[][] boardData){
			boardData[pickup.y][pickup.x] = null;
        	boardData[target.y][target.x] = pickup.piece;
			boardData[pickup.y][pickup.x].ability = false;
		}
    	public override void revertBoardData(ref PieceStruct[][] boardData){
			boardData[pickup.y][pickup.x] = pickup.piece;
        	boardData[target.y][target.x] = null;
			boardData[pickup.y][pickup.x].ability = true;
		}
		public override void Serialize(Packet packet) {
        	packet.Write(pickup);
        	packet.Write(target);
    	}

    	public override void Deserialize(Packet packet) {
  	      	pickup = packet.Read<TileStruct>();
        	target = packet.Read<TileStruct>();
    	}
	}
}
