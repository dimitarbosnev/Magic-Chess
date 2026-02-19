using Godot;
using System.Collections.Generic;

public partial class RedKing : ChessPiece
{
    public override Command AbilityMove(BoardStruct target){
        return new ReverseTurnCommand(SharedUtils.SetTileStruct(this), SharedUtils.SetTileStruct(target.piece));
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

	public class ReverseTurnCommand : Command{
		public TileStruct pickup{get; protected set;}
		public TileStruct target{get; protected set;}
		//The command should be retrived from the server
		public Command commandToReverse;

		public ReverseTurnCommand() : base(){}
    	public ReverseTurnCommand(TileStruct pPickup, TileStruct pTarget){
        	pickup = pPickup;
        	target = pTarget;
    	}
		//TODO: fix
		public override void execute(ref BoardStruct[][] board){
			if(pickup.piece != null && target.piece != null){
				ChessBoard.Instance.GetPieceById(pickup.piece.pieceID).Ability = false;
				commandToReverse.reverse(ref board);
			}
		}
		//TODO: fix
        public override void reverse(ref BoardStruct[][] board){
			if(pickup.piece != null && target.piece != null){
            	ChessBoard.Instance.GetPieceById(pickup.piece.pieceID).Ability = true;
				commandToReverse.execute(ref board);
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
			//Maybe add a check for if the king is in check
        	return valid1 && valid2;
    	}
    	public override void updateBoardData(ref PieceStruct[][] boardData){
			commandToReverse.revertBoardData(ref boardData);
			boardData[pickup.y][pickup.x].ability = false;
		}
    	public override void revertBoardData(ref PieceStruct[][] boardData){
			commandToReverse.updateBoardData(ref boardData);
			boardData[pickup.y][pickup.x].ability = true;
		}
		public override void Serialize(Packet packet) {
        	packet.Write(pickup);
        	packet.Write(target);
			bool command = commandToReverse != null? true : false;
			packet.Write(command);
			if(command)
				packet.Write(commandToReverse);
    	}

    	public override void Deserialize(Packet packet) {
  	      	pickup = packet.Read<TileStruct>();
        	target = packet.Read<TileStruct>();
			bool command = packet.ReadBool();
			if(command)
				commandToReverse = packet.Read<Command>();
    	}
	}
}
