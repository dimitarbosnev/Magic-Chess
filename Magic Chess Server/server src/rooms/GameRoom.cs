﻿using System;
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

        public void StartGame(TcpMessageChannel pPlayer1, TcpMessageChannel pPlayer2)
        {
            if (IsGameInPlay) throw new Exception("Programmer error duuuude.");

            IsGameInPlay = true;
            addMember(pPlayer1);
            addMember(pPlayer2);

            GameStartEvent gameStart = new GameStartEvent();
            gameStart.player1Name = _server.GetPlayerInfo(pPlayer1).playerName;
            gameStart.player2Name = _server.GetPlayerInfo(pPlayer2).playerName;
            sendToAll(gameStart);
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
                Log.LogInfo("People left the game...", this);
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

                while (memberCount > 0)
                    removeMember(getMemberAt(0));

                lobbyRoom.sendGameResult(listOfMembers, winnerInfo);
               _server.CloseGameRoom(this);
            }*/

        }
        protected override void handleNetworkMessage(ISerializable pMessage, TcpMessageChannel pSender)
        {
            if (pMessage is MakeMoveRequest)
            {
                handleMakeMoveRequest(pMessage as MakeMoveRequest, pSender);
            }
        }

        private void handleMakeMoveRequest(MakeMoveRequest pMessage, TcpMessageChannel pSender)
        {
            //we have two players, so index of sender is 0 or 1, which means playerID becomes 1 or 2
            int playerID = indexOfMember(pSender) + 1;
            //make the requested move (0-8) on the board for the player
            //_board.MakeMove(pMessage.move, playerID);

            //and send the result of the boardstate back to all clients
            MakeMoveResult makeMoveResult = new MakeMoveResult();
            makeMoveResult.whoMadeTheMove = playerID;
            makeMoveResult.boardData = _board.GetBoardData();
            sendToAll(makeMoveResult);
        }

    }
}
