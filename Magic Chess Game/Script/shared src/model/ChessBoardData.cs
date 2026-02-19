using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
/**
 * Super simple board model for TicTacToe that contains the minimal data to actually represent the board. 
 * It doesn't say anything about whose turn it is, whether the game is finished etc.
 * IF you want to actually implement a REAL Tic Tac Toe, that means you will have to add the data required for that (and serialize it!).
 */
public abstract class ChessBoardData : ISerializable
	{
		//board representation in 1d array, one element for each cell
		//0 is empty, 1 is player 1, 2 is player 2
		//might be that for your game, a 2d array is actually better

		protected abstract ref PieceStruct[][] getBoard();
		public ref PieceStruct[][] board{
			get{ return ref getBoard(); }
		}
        /**
		 * Returns who has won.
		 * 
		 * If there are any 0 on the board, noone has won yet (return 0).
		 * If there are only 1's on the board, player 1 has won (return 1).
		 * If there are only 2's on the board, player 2 has won (return 2).
		 */
		 public bool IsBoardValid(ChessBoardData chessBoardData){
			for (int y = 0; y < board.Length; y++)
				for (int x = 0; x <	board[y].Length; x++){
					TileStruct tileA = SharedUtils.SetTileStruct(x,y,board[y][x]);
					TileStruct tileB = SharedUtils.SetTileStruct(x,y,chessBoardData.board[y][x]);
					if(!SharedUtils.CompareTileStructs(tileA, tileB))
						return false;
				}
			return true;
		 }
        public Team WhoHasWon(){
			return Team.None;
		}
		
		public abstract void Serialize(Packet pPacket);

		public abstract void Deserialize(Packet pPacket);

		public override string ToString()
		{
			return GetType().Name;
		}
		
	}


