using Godot;
using System.Collections.Generic;
public partial class Mage : ChessPiece
{
    public override List<Vector2I> GetAvailableMoves(ref Tile[][] board)
	{
		List<Vector2I> r = new List<Vector2I>();

		if(coordinates.X > 0)
		for (int a = coordinates.X - 1; a >=0; a--){
			if(board[coordinates.Y][a].piece == null)
				r.Add(new Vector2I(a,coordinates.Y));
			else if(board[coordinates.Y][a].piece.Team != Team){
				r.Add(new Vector2I(a, coordinates.Y));
				break;
			}
			else
				break;
		}

		if(coordinates.X < board[coordinates.Y].Length)
		for (int a = coordinates.X + 1; a < board[coordinates.Y].Length; a++){
			if(board[coordinates.Y][a].piece == null)
				r.Add(new Vector2I(a,coordinates.Y));
			else if(board[coordinates.Y][a].piece.Team != Team){
				r.Add(new Vector2I(a, coordinates.Y));
				break;
			}
			else
				break;
		}

		int x = coordinates.X;
		for (int y = coordinates.Y-1; y >= 0; y--)
		{
			x += y % 2 == 0?1:0;
			if(x < board[y].Length){
				if(board[y][x].piece == null)
					r.Add(new Vector2I(x, y));
				else if(board[y][x].piece.Team != Team){
					r.Add(new Vector2I(x, y));
					break;
				}
				else
					break;
			}
			else
				break;
		}

		x = coordinates.X;
		for (int y = coordinates.Y-1; y >= 0; y--)
		{
			x += y % 2 == 0?0:-1;
			if( x >= 0){
				if(board[y][x].piece == null)
					r.Add(new Vector2I(x, y));
				else if( board[y][x].piece.Team != Team){
					r.Add(new Vector2I(x, y));
					break;
				}
				else
					break;
			}
			else
				break;
		}

		x = coordinates.X;
		for (int y = coordinates.Y+1; y < board.Length; y++)
		{
			x += y % 2 == 0?0:-1;
			if( x >= 0){
				if(board[y][x].piece == null)
					r.Add(new Vector2I(x, y));
				else if( board[y][x].piece.Team != Team){
					r.Add(new Vector2I(x, y));
					break;
				}
				else
					break;
			}
			else
				break;
		}

		x = coordinates.X;
		for (int y = coordinates.Y+1; y < board.Length; y++)
		{
			x += y % 2 == 0?1:0;
			if(x < board[y].Length){
				if(board[y][x].piece == null)
					r.Add(new Vector2I(x, y));
				else if( board[y][x].piece.Team != Team){
					r.Add(new Vector2I(x, y));
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
				if(tile.piece != null && tile.piece.Team != this.Team && 
				tile.piece.PieceType != PieceType.RedKing && tile.piece.PieceType != PieceType.BlueKing)
					r.Add(tile.coordinates);
		return r;
	}
    public override Command AbilityMove(Tile target){
        return new FreezeCommand(this, target.piece);
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

	public class FreezeCommand : Command{
		public PieceStruct pickup{get; protected set;}
		public PieceStruct target{get; protected set;}
		private ChessPiece pickupPiece;
		private ChessPiece targetPiece;

		public FreezeCommand() : base(){}
    	public FreezeCommand(ChessPiece pPickup, ChessPiece pTarget){
        	pickup = new PieceStruct(pPickup);
        	target = new PieceStruct(pTarget);
    	}

		public override void execute(ref Tile[][] board) {
			pickupPiece = board[pickup.cord.Y][pickup.cord.X].piece;
			targetPiece = board[target.cord.Y][target.cord.X].piece;
			pickupPiece.Ability = false;
			targetPiece.frozen = true;
			pickupPiece.targetPosition = Vector3.Zero;
			EventBus<NewTurnEvent>.OnEvent += OnNewTurn;
		}

        public override void reverse(ref Tile[][] board) {
            pickupPiece.Ability = true;
			targetPiece.frozen = false;
        }
        private void OnNewTurn(NewTurnEvent turnEvent){
			if(pickupPiece.Team != turnEvent.team) return;

			targetPiece.frozen = false;
			GD.Print("unfreeze");
			EventBus<NewTurnEvent>.OnEvent -= OnNewTurn;
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
