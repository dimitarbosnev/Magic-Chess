using System;
using System.Collections.Generic;
	/**
	 * Super simple board model for TicTacToe that contains the minimal data to actually represent the board. 
	 * It doesn't say anything about whose turn it is, whether the game is finished etc.
	 * IF you want to actually implement a REAL Tic Tac Toe, that means you will have to add the data required for that (and serialize it!).
	 */
	public class HexChessBoardData : ChessBoardData
	{
		//board representation in 1d array, one element for each cell
		//0 is empty, 1 is player 1, 2 is player 2
		//might be that for your game, a 2d array is actually better
		private PieceStruct[][] _board = {new PieceStruct[9],new PieceStruct[8],new PieceStruct[9],
										new PieceStruct[8],new PieceStruct[9],new PieceStruct[8],
										new PieceStruct[9],new PieceStruct[8],new PieceStruct[9]};
		
		protected override ref PieceStruct[][] getBoard(){
			return ref _board;
		}
        /**
		 * Returns who has won.
		 * 
		 * If there are any 0 on the board, noone has won yet (return 0).
		 * If there are only 1's on the board, player 1 has won (return 1).
		 * If there are only 2's on the board, player 2 has won (return 2).
		 */

		public void CopyBoard(PieceStruct[][] newBoard){
			for (int y = 0; y < _board.Length; y++)
				for (int x = 0; x <	_board[y].Length; x++)
					_board[y][x] = newBoard[y][x];
		}
    	public override void Serialize(Packet pPacket) {
			for (int y = 0; y < _board.Length; y++)
				for (int x = 0; x <	_board[y].Length; x++)
					pPacket.Write(_board[y][x]);
    	}

		public override void Deserialize(Packet pPacket){
			for (int y = 0; y < _board.Length; y++)
				for (int x = 0; x <	_board[y].Length; x++)
					_board[y][x] = pPacket.Read<PieceStruct>();
		}
	}


