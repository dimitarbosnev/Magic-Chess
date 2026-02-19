namespace server
{
    /**
     * This class wraps the actual board we are playing on.
     * 
     * Normally both logic and data would be in a single class (e.g. something like TicTacToeModel).
     * In this case I chose to split them up: we have one class TicTacToeBoard that wraps an instance
     * of the actual board data. This way we have an object we can serialize to clients (the board data) 
     * and one class that actually manages the logic of manipulating the board.
     * 
     * In this specific instance that logic is almost non existent, because I tried to keep the demo
     * as simple as possible, so we can only make a move. In an actual game, this class would implement
     * methods such as GetValidMoves(), HasWon(), GetCurrentPlayer() etc.
     */
    public class GameBoard
    {
        private ChessBoardData _board = new RectChessBoardData();
        public Stack<Command> executedCommands = new Stack<Command>();
        public Team teamTurn;
        /**
         * @param pMove     a number from 0-8 that indicates the cell we want to change
         * @param pPlayer   1 or 2 to indicate which player made the move
         */

        /**
         * Return the inner board data state so we can send it to a client.
         */
        public ChessBoardData GetBoardData()
        {
            //it would be more academically correct if we would clone this object before returning it, but anyway.
            return _board;
        }

        public bool IsBoardValid(ChessBoardData newBoardData){
            PieceStruct[][] oldBoard = _board.board;
            PieceStruct[][] newBoard = newBoardData.board;
            for (int y = 0; y < oldBoard.Length; y++)
			    for (int x = 0; x <	oldBoard[y].Length; x++){
                    if(!SharedUtils.ComparePieceStructs(oldBoard[y][x],newBoard[y][x]))
			    		return false;
                }
			return true;			
		 }

       
        public void NextTurn()
        {
            teamTurn = teamTurn == Team.Blue ? Team.Red : Team.Blue;
        }
    }
}
