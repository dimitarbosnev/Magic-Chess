using System;
	/**
	 * Super simple board model for TicTacToe that contains the minimal data to actually represent the board. 
	 * It doesn't say anything about whose turn it is, whether the game is finished etc.
	 * IF you want to actually implement a REAL Tic Tac Toe, that means you will have to add the data required for that (and serialize it!).
	 */
	public class ChessBoardData : ISerializable
	{
		//board representation in 1d array, one element for each cell
		//0 is empty, 1 is player 1, 2 is player 2
		//might be that for your game, a 2d array is actually better
		public PieceStruct[][] board = {new PieceStruct[9],new PieceStruct[8],new PieceStruct[9],
                              			new PieceStruct[8],new PieceStruct[9],new PieceStruct[8],
                              			new PieceStruct[9],new PieceStruct[8],new PieceStruct[9]};

        /**
		 * Returns who has won.
		 * 
		 * If there are any 0 on the board, noone has won yet (return 0).
		 * If there are only 1's on the board, player 1 has won (return 1).
		 * If there are only 2's on the board, player 2 has won (return 2).
		 */
		 public bool CheckBoard(PieceStruct[][] newBoard){
			for (int y = 0; y < board.Length; y++)
				for (int x = 0; x <	board[y].Length; x++)
						if(!board[y][x].PieceEqual(newBoard[x][y]))
							return false;

			return true;
				
		 }
        public Team WhoHasWon()
		{

			//Win condition
			//this is just an example of a possible win condition, 
			//but not the 'real' tictactoe win condition.
			//int total = 1;
			//foreach (int cell in board) total *= cell;

			//if (total == 1)		return 1;       //1*1*1*1*1*1*1*1*1
			//if (total == 512)	return 2;		//2*2*2*2*2*2*2*2*2
			return Team.None;							//noone has one yet
		}
		
		public void Serialize(Packet pPacket)
		{
			foreach(PieceStruct[] row in board)
				foreach(PieceStruct tile in row)
					pPacket.Write(tile);
		}

		public void Deserialize(Packet pPacket)
		{
			for (int y = 0; y < board.Length; y++)
				for (int x = 0; x <	board[y].Length; x++)
					board[y][x] = pPacket.Read<PieceStruct>();
		}

		public override string ToString()
		{
			return GetType().Name;
		}
	}


