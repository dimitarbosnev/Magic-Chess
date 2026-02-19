using Godot;
using System.Collections.Generic;
public partial class BlueKing : ChessPiece
{
	public bool protection = false;
	public override Command AbilityMove(BoardStruct target){
        return new ProtectCommand(SharedUtils.SetTileStruct(this));
    }

	public override void OnSpecialHoldUpdate(PlayerFSM playerFSM){
		targetPosition = Vector3.Up * 0.5f;
	}
	public override void OnSpecialHoldInput(PlayerFSM playerFSM, InputEventMouseButton mouseEvent){
		if(mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed) 
			if(playerFSM.hoverTile.tile != null && playerFSM.abilityMoves.Contains(playerFSM.hoverTile.tile.coordinates))
				playerFSM.TransitToState(typeof(PlayerSpecialReleaseState));
			else
				playerFSM.TransitToState(typeof(PlayerInvalidReleaseState));
	}

public class ProtectCommand : Command{
		public TileStruct pickup{get; protected set;}
		public ProtectCommand() : base(){}
    	public ProtectCommand(TileStruct pPickup){
        	pickup = pPickup;
    	}

		public override void execute(ref BoardStruct[][] board){
			BoardStruct pickupTile = board[pickup.y][pickup.x];
			if(pickup.piece != null){
				ChessPiece chessPiece = ChessBoard.Instance.GetPieceById(pickup.piece.pieceID);
				if(chessPiece is BlueKing){
					BlueKing blueKing = chessPiece as BlueKing;
					blueKing.protection = true;
					blueKing.Ability = false;
					blueKing.SetPosition(Vector3.Zero);
					EventBus<NewTurnEvent>.OnEvent += OnNewTurn;
				}
			}
		}

        public override void reverse(ref BoardStruct[][] board){
			if(pickup.piece != null){
				ChessPiece chessPiece = ChessBoard.Instance.GetPieceById(pickup.piece.pieceID);
				if(chessPiece is BlueKing){
					BlueKing blueKing = chessPiece as BlueKing;
           			blueKing.Ability = true;
					blueKing.protection = false;
					blueKing.SetPosition(Vector3.Zero);
				}
				EventBus<NewTurnEvent>.OnEvent -= OnNewTurn;
			}
        }
		public override bool validateMove(ref PieceStruct[][] boardData){
			if(pickup.piece == null || boardData[pickup.y][pickup.x] == null) return false;
			//Maybe add a check for if the king is in check
        	return SharedUtils.ComparePieceStructs(pickup.piece,boardData[pickup.y][pickup.x]);
    	}
    	public override void updateBoardData(ref PieceStruct[][] boardData){
			boardData[pickup.y][pickup.x].ability = false;
		}
    	public override void revertBoardData(ref PieceStruct[][] boardData){
			boardData[pickup.y][pickup.x].ability = true;
		}

        private void OnNewTurn(NewTurnEvent turnEvent){
			if(pickup.piece.team != turnEvent.team) return;
			BlueKing blueKing = ChessBoard.Instance.GetPieceById(pickup.piece.pieceID)  as BlueKing;
			blueKing.protection = false;
			EventBus<NewTurnEvent>.OnEvent -= OnNewTurn;
		}

		public override void Serialize(Packet packet) {
        	packet.Write(pickup);
    	}

    	public override void Deserialize(Packet packet) {
  	      	pickup = packet.Read<TileStruct>();
    	}
	}
}
