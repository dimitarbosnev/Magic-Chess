using Godot;
using System.Collections.Generic;
public partial class Shapeshifter : ChessPiece
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
				if(tile.piece != null && tile.piece != this &&
				   tile.piece.PieceType != PieceType.RedKing && tile.piece.PieceType != PieceType.BlueKing)
					r.Add(tile.coordinates);
		return r;
	}

	public override Command AbilityMove(Tile target)
    {
        return new ShapeshiftCommand(this,target.piece.PieceType);
    }
	public override void OnSpecialHoldUpdate(PlayerFSM playerFSM){
		//Shapeshifter menu logic
		targetPosition = Vector3.Up * 0.5f;
	}
	public override void OnSpecialHoldInput(PlayerFSM playerFSM, InputEventMouseButton mouseEvent){
		//Shapeshifter logic
		if(mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
			if(playerFSM.hoverTile != null && playerFSM.abilityMoves.Contains(playerFSM.hoverTile.coordinates)){
				playerFSM.TransitToState(typeof(PlayerSpecialReleaseState));
			}
			else
				playerFSM.TransitToState(typeof(PlayerInvalidReleaseState));
		
	}

	public class ShapeshiftCommand : Command{
		public PieceStruct pickup{get; protected set;}
    	public PieceType pieceType{get; protected set;}
		private ChessPiece pickupPiece;
		private ChessPiece newPiece;

		public ShapeshiftCommand() : base(){}
    	public ShapeshiftCommand(ChessPiece pPickup, PieceType pPieceType){
        	pickup = new PieceStruct(pPickup);
        	pieceType = pPieceType;
    	}

		public override void execute(ref Tile[][] board){
			if(pickup == null)
				GD.Print("Why is the pieceStruct null?!?!");
			if(board[pickup.cord.Y][pickup.cord.X].piece == null)
				GD.Print("Why is the piece null?!?!");
			pickupPiece = board[pickup.cord.Y][pickup.cord.X].piece;
			pickupPiece.Visible = false;
			pickupPiece.Ability = false;
			newPiece = Resources.pieceCollection[pieceType].Instantiate() as ChessPiece;
			pickupPiece.tile.AddChild(newPiece);
			newPiece.InitPiece(pieceType, pickupPiece.Team);
			ChessBoard.AssignPiece(newPiece,pickupPiece.tile);
			newPiece.SetPosition(Vector3.Up,true);
			newPiece.SetPosition(Vector3.Zero);
			if(newPiece.Team == Team.Blue)
				newPiece.RotateY(Mathf.DegToRad(180));

			EventBus<NewTurnEvent>.OnEvent += OnNewTurn;
		}

        public override void reverse(ref Tile[][] board){
			pickupPiece.Visible = true;
			pickupPiece.Ability = true;
			ChessBoard.AssignPiece(pickupPiece,newPiece.tile);
			newPiece.Free();

        }
        private void OnNewTurn(NewTurnEvent turnEvent){
			ChessBoard.AssignPiece(pickupPiece,newPiece.tile);
			pickupPiece.SetPosition(Vector3.Up,true);
			pickupPiece.SetPosition(Vector3.Zero);
			newPiece.Free();
			newPiece = null;
			pickupPiece.Visible = true;
			EventBus<NewTurnEvent>.OnEvent -= OnNewTurn;
		}

		public override void Serialize(Packet packet) {
        	packet.Write(pickup);
        	packet.Write((int)pieceType);
    	}

    	public override void Deserialize(Packet packet) {
  	      	pickup = packet.Read<PieceStruct>();
        	pieceType = (PieceType)packet.ReadInt();
    	}
	}
}
