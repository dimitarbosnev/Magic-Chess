using Godot;
using System.Collections.Generic;
public partial class BlueKing : ChessPiece
{
	public bool protection = false;
    public override List<Vector2I> GetAvailableMoves(ref Tile[][] board)
	{
		//Big fat mess do not touch unless you have nerves to fix it after
		List<Vector2I> r = new List<Vector2I>();
		int y;
		int x;

		//"+" straight
		#region diagonals
		y = coordinates.Y + 2;
		x = coordinates.X + (coordinates.Y % 2 == 0? -1:1);
		if(y < board.Length && 
		(board[y][coordinates.X].piece == null || board[y][coordinates.X].piece.Team != Team) &&
			((coordinates.X < board[y-1].Length && board[y-1][coordinates.X].piece == null) || board[y-1][x].piece == null))
			r.Add(new Vector2I(coordinates.X,y));
	
		
		//"-" straight
		y = coordinates.Y - 2;
		if(y >= 0 && (board[y][coordinates.X].piece == null || board[y][coordinates.X].piece.Team != Team) &&
			((coordinates.X < board[y+1].Length && board[y+1][coordinates.X].piece == null) || board[y+1][x].piece == null))
			r.Add(new Vector2I(coordinates.X,y));

		//bottom 
		int dirX = coordinates.X + (coordinates.Y % 2 == 0? -1:0);
		y = coordinates.Y + 1;
		if(y < board.Length){
			//bottom left
			x = coordinates.X + (coordinates.Y % 2 == 0?-2:-1);
			if(x >= 0 && (board[coordinates.Y][coordinates.X-1].piece == null || board[y][dirX].piece == null) &&
			(board[y][x].piece == null || board[y][x].piece.Team != Team))
				r.Add(new Vector2I(x,y));
			
			//bottom right
			dirX = coordinates.X + (coordinates.Y % 2 == 0? 0:1);
			x = coordinates.X + (coordinates.Y % 2 == 0? 1:2);
			if(x <board[y].Length && (board[coordinates.Y][coordinates.X+1].piece == null || board[y][dirX].piece == null) && 
			(board[y][x].piece == null || board[y][x].piece.Team != Team))
				r.Add(new Vector2I(x,y));
		}

		//top
		y = coordinates.Y - 1;
		if(y >= 0){
			//top left
			dirX = coordinates.X + (coordinates.Y % 2 == 0? -1:0);
			x = coordinates.X + (coordinates.Y % 2 == 0?-2:-1);
			if(x >= 0 && (board[coordinates.Y][coordinates.X-1].piece == null || board[y][dirX].piece == null) &&
			(board[coordinates.Y][coordinates.X-1].piece == null || board[y][x].piece.Team != Team))
				r.Add(new Vector2I(x,y));
			//top right
			dirX = coordinates.X + (coordinates.Y % 2 == 0? 0:1);
			x = coordinates.X + (coordinates.Y % 2 == 0? 1:2);
			if(x <board[y].Length && (board[coordinates.Y][coordinates.X+1].piece == null || board[y][dirX].piece == null) && 
			(board[y][x].piece == null || board[y][x].piece.Team != Team))
				r.Add(new Vector2I(x,y));
		}
		#endregion
		#region neibouring
		//bottom 
		y = coordinates.Y + 1;
		dirX = coordinates.X + (coordinates.Y % 2 == 0? -1:1);
		if(y < board.Length){
			if(coordinates.X < board[y].Length && (board[y][coordinates.X].piece == null || board[y][coordinates.X].piece.Team != Team))
				r.Add(new Vector2I(coordinates.X,y));
			if(dirX >= 0 && dirX <= board[y].Length && (board[y][dirX].piece == null || board[y][dirX].piece.Team != Team))
				r.Add(new Vector2I(dirX,y));
		}
		//top
		y = coordinates.Y - 1;
		if(y >= 0){
			if(coordinates.X < board[y].Length && (board[y][coordinates.X].piece == null || board[y][coordinates.X].piece.Team != Team))
				r.Add(new Vector2I(coordinates.X,y));
			if(dirX >= 0 && dirX <= board[y].Length && (board[y][dirX].piece == null || board[y][dirX].piece.Team != Team))
				r.Add(new Vector2I(dirX,y));
		}
		//left
		 if(coordinates.X - 1 >= 0 && 
		 (board[coordinates.Y][coordinates.X-1].piece == null || board[coordinates.Y][coordinates.X-1].piece.Team != Team))
			r.Add(new Vector2I(coordinates.X-1,coordinates.Y));

		//right
		if(coordinates.X + 1 < board[coordinates.Y].Length && 
		 (board[coordinates.Y][coordinates.X+1].piece == null || board[coordinates.Y][coordinates.X+1].piece.Team != Team))
			r.Add(new Vector2I(coordinates.X+1,coordinates.Y));
		
		#endregion
		return r;
	}

	public override List<Vector2I> GetAbilityMoves(ref Tile[][] board)
	{
		List<Vector2I> r = new List<Vector2I>();
		foreach(Tile[] row in board)
			foreach(Tile tile in row)
				if(tile.piece != null && tile.piece.GetAvailableMoves(ref board).Contains(coordinates))
					r.Add(coordinates);

		return r;
	}
    public override Command AbilityMove(Tile target){
        return new ProtectCommand(this);
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

public class ProtectCommand : Command{
		public PieceStruct pickup{get; protected set;}
		private BlueKing blueKing;
		public ProtectCommand() : base(){}
    	public ProtectCommand(BlueKing pPickup){
        	pickup = new PieceStruct(pPickup);
    	}

		public override void execute(ref Tile[][] board){
			blueKing = board[pickup.cord.Y][pickup.cord.X].piece as BlueKing;
			blueKing.Ability = false;
			blueKing.protection = true;
			blueKing.targetPosition = Vector3.Zero;
			EventBus<NewTurnEvent>.OnEvent += OnNewTurn;
		}

        public override void reverse(ref Tile[][] board){
           	blueKing.Ability = true;
			blueKing.protection = false;
        }
        private void OnNewTurn(NewTurnEvent turnEvent){

			if(blueKing.Team != turnEvent.team) return;
			
			blueKing.protection = false;
			EventBus<NewTurnEvent>.OnEvent -= OnNewTurn;
		}

		public override void Serialize(Packet packet) {
        	packet.Write(pickup);
    	}

    	public override void Deserialize(Packet packet) {
  	      	pickup = packet.Read<PieceStruct>();
    	}
	}
}
