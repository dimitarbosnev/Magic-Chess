using Godot;
using System.Collections.Generic;
public partial class Shapeshifter : ChessPiece
{
	public override Command AbilityMove(BoardStruct target)
    {
        return new ShapeshiftCommand(SharedUtils.SetTileStruct(this),target.piece.PieceType);
    }
	public override void OnSpecialHoldUpdate(PlayerFSM playerFSM){
		//Shapeshifter menu logic
		targetPosition = Vector3.Up * 0.5f;
	}
	public override void OnSpecialHoldInput(PlayerFSM playerFSM, InputEventMouseButton mouseEvent){
		//Shapeshifter logic
		if(mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
			if(playerFSM.hoverTile.tile != null && playerFSM.abilityMoves.Contains(playerFSM.hoverTile.tile.coordinates)){
				playerFSM.TransitToState(typeof(PlayerSpecialReleaseState));
			}
			else
				playerFSM.TransitToState(typeof(PlayerInvalidReleaseState));
		
	}
	//TODO: Has to be seperated into 2 for poth rect and hex chess
	//Currently works only for rect chess
	//Might Change it to include the selected piece for better check
	public class ShapeshiftCommand : Command{
		public TileStruct pickup{get; protected set;}
    	public PieceType pieceType{get; protected set;}
		public PieceType oldType{get; protected set;}
		private ChessPiece newPiece;
		private ChessPiece oldPiece;
		public ShapeshiftCommand() : base(){}
    	public ShapeshiftCommand(TileStruct pPickup, PieceType pPieceType){
        	pickup = pPickup;
        	pieceType = pPieceType;
			oldType = pPickup.piece.pieceType;
    	}

		public override void execute(ref BoardStruct[][] board){
			if(pickup.piece != null){
				oldPiece = ChessBoard.Instance.GetPieceById(pickup.piece.pieceID);
				oldPiece.Visible = false;
				oldPiece.Ability = false;
				newPiece = ChessBoard.Instance.SpawnFigure(pickup.x,pickup.y,pickup.piece);
				newPiece.SetPosition(Vector3.Up,true);
				newPiece.SetPosition(Vector3.Zero);
				EventBus<NewTurnEvent>.OnEvent += OnNewTurn;
			}
		}

        public override void reverse(ref BoardStruct[][] board){
			if(pickup.piece != null){
				oldPiece.Visible = true;
				oldPiece.Ability = true;
				ChessBoard.Instance.AssignPiece(oldPiece,board[pickup.x][pickup.y]);
				ChessBoard.Instance.DeletePiece(newPiece);
			}
        }

		public override bool validateMove(ref PieceStruct[][] boardData){
			//Just return true, no need for a check now
        	return true;
    	}
    	public override void updateBoardData(ref PieceStruct[][] boardData){
			//We have a problem there is no way yet o
			boardData[pickup.y][pickup.x].pieceType = pieceType;
		}
    	public override void revertBoardData(ref PieceStruct[][] boardData){
			boardData[pickup.y][pickup.x].pieceType = oldType;
		}
        private void OnNewTurn(NewTurnEvent turnEvent){
			oldPiece.Visible = true;
			ChessBoard.Instance.AssignPiece(oldPiece,ChessBoard.Instance.GetBoardStruct(pickup.x,pickup.y));
			oldPiece.SetPosition(Vector3.Up,true);
			oldPiece.SetPosition(Vector3.Zero);
			ChessBoard.Instance.DeletePiece(newPiece);
			EventBus<NewTurnEvent>.OnEvent -= OnNewTurn;
		}

		public override void Serialize(Packet packet) {
        	packet.Write(pickup);
        	packet.Write((int)pieceType);
    	}

    	public override void Deserialize(Packet packet) {
  	      	pickup = packet.Read<TileStruct>();
        	pieceType = (PieceType)packet.ReadInt();
    	}
	}
}
