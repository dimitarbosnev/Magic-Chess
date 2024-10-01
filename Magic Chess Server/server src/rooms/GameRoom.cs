using System;
using System.Collections.Generic;

namespace server
{
    /**
	 * This room runs a single Game (at a time). 
	 * 
	 * The 'Game' is very simple at the moment:
	 *	- all client moves are broadcasted to all clients
	 *	
	 * The game has no end yet (that is up to you), in other words:
	 * all players that are added to this room, stay in here indefinitely.
	 */
    class GameRoom : Room
    {
        public bool IsGameInPlay { get; private set; }

        //wraps the board to play on...
        private GameBoard _board = new GameBoard();

        public GameRoom(TCPGameServer pOwner) : base(pOwner)
        {
        }
        private void TerminateGame()
        {
            Log.LogInfo("People left the game...", this);
            while (memberCount > 0){
                _server.GetLobbyRoom().AddMember(getMemberAt(0));
                removeMember(getMemberAt(0));
            }
            
            _server.CloseGameRoom(this);
        }
        public void StartGame(TcpMessageChannel pPlayer1, TcpMessageChannel pPlayer2)
        {
            if (IsGameInPlay) throw new Exception("Programmer error duuuude.");

            IsGameInPlay = true;
            PlayerInfo player1 = _server.GetPlayerInfo(pPlayer1);
            PlayerInfo player2 = _server.GetPlayerInfo(pPlayer2);
            GameStartEvent gameStart1 = new GameStartEvent(player1.playerName, player2.playerName, Team.Blue);
            GameStartEvent gameStart2 = new GameStartEvent(player2.playerName, player1.playerName, Team.Red);

            addMember(pPlayer1);
            addMember(pPlayer2);

            pPlayer1.SendMessage(gameStart1);
            pPlayer2.SendMessage(gameStart2);
        }

        protected override void addMember(TcpMessageChannel pMember)
        {
            base.addMember(pMember);

            //notify client he has joined a game room 
            RoomJoinedEvent roomJoinedEvent = new RoomJoinedEvent();
            roomJoinedEvent.room = RoomJoinedEvent.Room.GAME_ROOM;
            pMember.SendMessage(roomJoinedEvent);
        }

        public override void Update()
        {
            //demo of how we can tell people have left the game...
            int oldMemberCount = memberCount;
            base.Update();
            CheckForGameEnd();
            int newMemberCount = memberCount;

            if (oldMemberCount != newMemberCount)
            {
                //If a player lost connection terminate game
                TerminateGame();
            }
        }

        private void CheckForGameEnd()
        {
            /*int index = _board.GetBoardData().WhoHasWon();
            if (index != 0)
            {
                LobbyRoom lobbyRoom = _server.GetLobbyRoom();
                PlayerInfo winnerInfo = _server.GetPlayerInfo(getMemberAt(index - 1));
                List<TcpMessageChannel> listOfMembers = GetMemberList();

                TerminateGame();

                lobbyRoom.sendGameResult(listOfMembers, winnerInfo);
            }*/

        }
        protected override void handleNetworkMessage(ISerializable pMessage, TcpMessageChannel pSender)
        {
            if (pMessage is MakeMoveRequest) handleMakeMoveRequest(pMessage as MakeMoveRequest, pSender);
        }
        private void handleMakeMoveRequest(MakeMoveRequest pMessage, TcpMessageChannel pSender)
        {
            //if (_board.teamTurn !=_server.GetPlayerInfo(pSender).playerTeam) return;
            //we have two players, so index of sender is 0 or 1, which means playerID becomes 1 or 2
            //make the requested move (0-8) on the board for the player
            //_board.MakeMove(pMessage.move, playerID);

            //and send the result of the boardstate back to all clients
            //Board check

            //if(!_board.GetBoardData().CheckBoard(pMessage.chessBoardData.board))
            //{

            //}

            //TODO: command check

            //switch turn
                //_board.NextTurn();
            MakeMoveResult makeMoveResult = new MakeMoveResult(pMessage.command);
            //makeMoveResult.boardData = _board.GetBoardData();
            sendToAll(makeMoveResult);
        }

    }
}
