using Godot;
using System.Collections.Generic;
public partial class Mage : ChessPiece
{
    public override Command AbilityMove(BoardStruct target){
        return new FreezeCommand(SharedUtils.SetTileStruct(this), SharedUtils.SetTileStruct(target));
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

	public class FreezeCommand : Command{
		public TileStruct pickup{get; protected set;}
		public TileStruct target{get; protected set;}
		public FreezeCommand() : base(){}
    	public FreezeCommand(TileStruct pPickup, TileStruct pTarget){
        	pickup = pPickup;
        	target = pTarget;
    	}

		public override void execute(ref BoardStruct[][] board) {
			if(pickup.piece != null && target.piece != null){
            	ChessPiece chessPiece1 = ChessBoard.Instance.GetPieceById(pickup.piece.pieceID);
            	ChessPiece chessPiece2 = ChessBoard.Instance.GetPieceById(target.piece.pieceID);

				chessPiece1.Ability = false;
				chessPiece2.frozen = true;
				chessPiece1.SetPosition(Vector3.Zero);
				EventBus<NewTurnEvent>.OnEvent += OnNewTurn;
        	}
		}

        public override void reverse(ref BoardStruct[][] board) {
            if(pickup.piece != null && target.piece != null){
            	ChessPiece chessPiece1 = ChessBoard.Instance.GetPieceById(pickup.piece.pieceID);
            	ChessPiece chessPiece2 = ChessBoard.Instance.GetPieceById(target.piece.pieceID);

				chessPiece1.Ability = true;
				chessPiece2.frozen = false;
				chessPiece1.SetPosition(Vector3.Zero);
        	}
        }
		public override bool validateMove(ref PieceStruct[][] boardData){
        	bool valid1 = false;
        	bool valid2 = false;

			if(pickup.piece == null || boardData[pickup.y][pickup.x] == null) return false;
			if(target.piece == null || boardData[target.y][target.x] == null) return false;
			
        	if(SharedUtils.ComparePieceStructs(pickup.piece,boardData[pickup.y][pickup.x]))
        	    valid1 = true;
        	if(SharedUtils.ComparePieceStructs(target.piece,boardData[target.y][target.x]))
        	    valid2 = true;

        	return valid1 && valid2;
    	}	
    	public override void updateBoardData(ref PieceStruct[][] boardData){
			//Implement the freezing of a piece
			boardData[pickup.y][pickup.x].ability = false;
		}
    	public override void revertBoardData(ref PieceStruct[][] boardData){
			//Implement the unfreezing of a piece
			boardData[pickup.y][pickup.x].ability = true;
		}
        private void OnNewTurn(NewTurnEvent turnEvent){
			if(pickup.piece.team != turnEvent.team) return;
			ChessPiece chessPiece2 = ChessBoard.Instance.GetPieceById(target.piece.pieceID);
			chessPiece2.frozen = false;
			GD.Print("unfreeze");
			EventBus<NewTurnEvent>.OnEvent -= OnNewTurn;
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
