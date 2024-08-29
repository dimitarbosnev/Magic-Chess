using Godot;
using System.Collections.Generic;
public partial class Assassin : ChessPiece
{
    public override List<Vector2I> GetAvailableMoves(ref Tile[][] board)
	{
		List<Vector2I> r = new List<Vector2I>();
		for (int y = coordinates.Y-2; y >= 0; y-=2)
		{
			if( y >= 0){ 
				if(board[y+1][coordinates.X].piece != null && board[y+1][coordinates.X + (y % 2 == 0? -1 : 1)].piece != null)
					break;
				else if(board[y][coordinates.X].piece == null){
					r.Add(new Vector2I(coordinates.X, y));
				}
				else if (board[y][coordinates.X].piece.Team != Team){
					r.Add(new Vector2I(coordinates.X, y));
					break;
				}
				else
					break;
			}
			else
				break;
		}

		for (int y = coordinates.Y+2; y < board.Length; y+=2)
		{
			if(y < board.Length){ 
				if(board[y-1][coordinates.X].piece != null && board[y-1][coordinates.X + (y % 2 == 0? -1 : 1)].piece != null)
					break;
				else if(board[y][coordinates.X].piece == null){
					r.Add(new Vector2I(coordinates.X, y));
				}
				else if (board[y][coordinates.X].piece.Team != Team){
					r.Add(new Vector2I(coordinates.X, y));
					break;
				}
				else
					break;
			}
			else
				break;
		}

		int dirX = coordinates.X;
		for (int y = coordinates.Y+1; y < board.Length; y++)
		{
			dirX += y % 2 == 0?-1:-2;
			if(dirX >= 0 && y < board.Length){ 
				if(board[y-1][dirX + (y % 2 == 0?0:1)].piece != null && board[y][dirX+1].piece != null)
					break;
				else if(board[y][dirX].piece == null){
					r.Add(new Vector2I(dirX, y));
				}
				else if (board[y][dirX].piece.Team != Team){
					r.Add(new Vector2I(dirX, y));
					break;
				}
				else
					break;
			}
			else
				break;
		}

		dirX = coordinates.X;
		for (int y = coordinates.Y-1; y >= 0; y--)
		{
			dirX += y % 2 == 0?2:1;
			if(dirX < board[y].Length && y >= 0){ 
				if(board[ y+1][dirX + (y % 2 == 0?-1:0)].piece != null && board[y][dirX-1].piece != null)
					break;
				else if(board[y][dirX].piece == null){
					r.Add(new Vector2I(dirX, y));
				}
				else if (board[y][dirX].piece.Team != Team){
					r.Add(new Vector2I(dirX, y));
					break;
				}
				else
					break;
			}
			else
				break;
		}

		dirX = coordinates.X;
		for (int y = coordinates.Y+1; y < board.Length; y++)
		{
			dirX += y % 2 == 0?2:1;
			if(dirX < board[y].Length && y < board.Length){ 
				if(board[y-1][dirX + (y % 2 == 0?-1:0)].piece != null && board[y][dirX-1].piece != null)
					break;
				else if(board[y][dirX].piece == null){
					r.Add(new Vector2I(dirX, y));
				}
				else if (board[y][dirX].piece.Team != Team){
					r.Add(new Vector2I(dirX, y));
					break;
				}
				else
					break;
			}
			else
				break;
		}

		dirX = coordinates.X;
		for (int y = coordinates.Y-1; y >= 0; y--)
		{
			dirX += y % 2 == 0?-1:-2;
			if(dirX >= 0 && y >= 0){ 
				if(board[ y+1][dirX + (y % 2 == 0?0:1)].piece != null && board[y][dirX+1].piece != null)
					break;
				else if(board[y][dirX].piece == null){
					r.Add(new Vector2I(dirX, y));
				}
				else if (board[y][dirX].piece.Team != Team){
					r.Add(new Vector2I(dirX, y));
					break;
				}
				else
					break;
			}
			else
				break;
		}
		
		return r;
	}

	public override List<Vector2I> GetAbilityMoves(ref Tile[][] board)
	{
		List<Vector2I> r = new List<Vector2I>();
		foreach(Tile[] row in board)
			foreach(Tile tile in row)
				if(tile.piece != null && tile.piece.Team == this.Team && tile.piece != this && 
				tile.piece.PieceType != PieceType.RedKing && tile.piece.PieceType != PieceType.BlueKing)
					r.Add(tile.coordinates);
		return r;
	}
    public override Command AbilityMove(Tile target){
        return new SwapCommand(this, target.piece);
    }

	public override void OnSpecialHoldUpdate(PlayerFSM playerFSM){
		targetPosition = Vector3.Up * 0.5f;
	}
	public override void OnSpecialHoldInput(PlayerFSM playerFSM, InputEventMouseButton mouseEvent){
		if(mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed) 
			if(playerFSM.hoverTile != null && playerFSM.abilityMoves.Contains(playerFSM.hoverTile.coordinates))
				playerFSM.TransitToState(typeof(PlayerSpecialReleaseState));
			else
				playerFSM.TransitToState(typeof(PlayerInvalidReleaseState));
	}

	public class SwapCommand : Command{
		public PieceStruct pickup{get; protected set;}
		public PieceStruct target{get; protected set;}
		private ChessPiece pickupPiece;
		private ChessPiece targetPiece;
    	public SwapCommand() : base(){}
    	public SwapCommand(ChessPiece pPickup, ChessPiece pTarget){
        	pickup = new PieceStruct(pPickup);
        	target = new PieceStruct(pTarget);
    	}

		public override void execute(ref Tile[][] board) {
			Tile pickupTile = board[pickup.cord.Y][pickup.cord.X];
			Tile targetTile = board[target.cord.Y][target.cord.X];

			pickupPiece = pickupTile.piece;
			targetPiece = targetTile.piece;

			ChessBoard.AssignPiece(pickupPiece, targetTile);
			ChessBoard.AssignPiece(targetPiece, pickupTile);

			pickupPiece.Ability = false;

			EventBus<NewTurnEvent>.Invoke(new NewTurnEvent(pickupPiece.Team));
		}

        public override void reverse(ref Tile[][] board) {
			Tile pickupTile = board[pickup.cord.Y][pickup.cord.X];
		   	Tile targetTile = board[target.cord.Y][target.cord.X];

			ChessBoard.AssignPiece(pickupPiece, pickupTile);
			ChessBoard.AssignPiece(targetPiece, targetTile);
           	pickupPiece.Ability = true;
        }

		public override void Serialize(Packet packet) {
        	packet.Write(pickup);
        	packet.Write(target);
    	}

    	public override void Deserialize(Packet packet) {
  	      	pickup = packet.Read<PieceStruct>();
        	target = packet.Read<PieceStruct>();
    	}
	}
}
