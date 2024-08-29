using Godot;
using System.Collections.Generic;

public partial class Centaur : ChessPiece
{
    public override List<Vector2I> GetAvailableMoves(ref Tile[][] board)
	{
		List<Vector2I> r = new List<Vector2I>();
		int y;
		int dirX = coordinates.Y % 2 == 0? -1 : 1;
		int x = coordinates.X;
		
		//"+" straight
		y = coordinates.Y + 3;
		if(y < board.Length){
		 if(x >= 0 && x < board[y].Length && (board[y][x].piece == null || board[y][x].piece.Team != Team))
			r.Add(new Vector2I(x,y));
		if(x + dirX >= 0 && x + dirX < board[y].Length && (board[y][x].piece == null || board[y][x].piece.Team != Team))
			r.Add(new Vector2I(x+dirX,y));
		}

		//"-" straight
		y = coordinates.Y -3;
		if(y >= 0){
		 if(x >= 0 && x < board[y].Length && (board[y][x].piece == null || board[y][x].piece.Team != Team))
			r.Add(new Vector2I(x,y));
		if(x + dirX >= 0 && x + dirX < board[y].Length && (board[y][x].piece == null || board[y][x].piece.Team != Team))
			r.Add(new Vector2I(x+dirX,y));
		}

		// "+" diagnal
		y = coordinates.Y + 1;
		if(y < board.Length){
			dirX = coordinates.Y % 2 == 0? 2 : 3;
			if(x + dirX < board[y].Length && (board[y][x].piece == null || board[y][x].piece.Team != Team)) 
				r.Add(new Vector2I(x+dirX,y));
			dirX = coordinates.Y % 2 == 0? -3 : -2;
			if(x + dirX >= 0 && (board[y][x].piece == null || board[y][x].piece.Team != Team))
				r.Add(new Vector2I(x+dirX,y));
		}

		y = coordinates.Y + 2;
		if(y < board.Length){
			dirX = 2;
			if(x + dirX < board[y].Length && (board[y][x].piece == null || board[y][x].piece.Team != Team))
				r.Add(new Vector2I(x+dirX,y));
			dirX = -2;
			if(x + dirX >= 0 && (board[y][x].piece == null || board[y][x].piece.Team != Team))
				r.Add(new Vector2I(x+dirX,y));
		}
		// "-" diagnal
		y = coordinates.Y - 1;
		if(y >= 0){
			dirX = coordinates.Y % 2 == 0? 2 : 3;
			if(x + dirX < board[y].Length && (board[y][x].piece == null || board[y][x].piece.Team != Team))
				r.Add(new Vector2I(x+dirX,y));
			dirX = coordinates.Y % 2 == 0? -3 : -2;
			if(x + dirX >= 0 && (board[y][x].piece == null || board[y][x].piece.Team != Team))
				r.Add(new Vector2I(x+dirX,y));
		}

		y = coordinates.Y - 2;
		if(y >= 0){
		dirX = 2;
		if(x + dirX < board[y].Length && (board[y][x].piece == null || board[y][x].piece.Team != Team))
			r.Add(new Vector2I(x+dirX,y));
		dirX = -2;
		if(x + dirX >= 0 && (board[y][x].piece == null || board[y][x].piece.Team != Team)) 
			r.Add(new Vector2I(x+dirX,y));
		}
		return r;
	}

	public override List<Vector2I> GetAbilityMoves(ref Tile[][] board)
	{
		List<Vector2I> r = new List<Vector2I>();
		int y;
		int x;

		//"+" straight
		y =  coordinates.Y + 2;
		x = coordinates.X;
		if(y < board.Length && board[y][x].piece == null)
			r.Add(new Vector2I(x,y));
		//"-" straight
		y = coordinates.Y - 2;
		if(y >= 0 && board[y][x].piece == null)
			r.Add(new Vector2I(x,y));
		
		y = coordinates.Y + 1;

		if(y < board.Length){
			x = coordinates.X + (coordinates.Y % 2 == 0?-2:-1);
			if(x >= 0 && board[y][x].piece == null)
				r.Add(new Vector2I(x,y));

			x = coordinates.X + (coordinates.Y % 2 == 0? 1:2);
			if(x >= 0 && board[y][x].piece == null)
				r.Add(new Vector2I(x,y));
		}

		y = coordinates.Y - 1;
		if(y >= 0){
			x = coordinates.X + (coordinates.Y % 2 == 0?-2:-1);
			if(x >= 0 && board[y][x].piece == null)
				r.Add(new Vector2I(x,y));

			x = coordinates.X + (coordinates.Y % 2 == 0? 1:2);
			if(x >= 0 && board[y][x].piece == null)
				r.Add(new Vector2I(x,y));
		}

		return r;
	}
    public override Command AbilityMove(Tile target){
        return new RepositionCommand(this, target);
    }

	public class RepositionCommand : Command{
		public PieceStruct pickup{get; protected set;}
		public TileStruct target{get; protected set;}
		private ChessPiece chessPiece;
		public RepositionCommand() : base(){}
    	public RepositionCommand(ChessPiece pPickup, Tile pTarget){
        	pickup = new PieceStruct(pPickup);
        	target = new TileStruct(pTarget);
    	}

		public override void execute(ref Tile[][] board){
			chessPiece = board[pickup.cord.Y][pickup.cord.X].piece;
			chessPiece.Ability = false;
			ChessBoard.AssignPiece(chessPiece, board[target.cord.Y][target.cord.X]);
			EventBus<NewTurnEvent>.Invoke(new NewTurnEvent(chessPiece.Team));
		}

        public override void reverse(ref Tile[][] board){
            chessPiece.Ability = true;
			ChessBoard.AssignPiece(chessPiece,board[pickup.cord.Y][pickup.cord.X]);
        }

		public override void Serialize(Packet packet) {
        	packet.Write(pickup);
        	packet.Write(target);
    	}

    	public override void Deserialize(Packet packet) {
  	      	pickup = packet.Read<PieceStruct>();
        	target = packet.Read<TileStruct>();
    	}
	}
}
