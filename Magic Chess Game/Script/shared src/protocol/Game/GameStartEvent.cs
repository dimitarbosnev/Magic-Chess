
    /**
     * Send from CLIENT to SERVER to indicate the move the client would like to make.
     * Since the board is just an array of cells, move is a simple index.
     */
    public class GameStartEvent : ISerializable
    {
        public string playerName{get; private set;}
        public string enemyName{get; private set;}
        public Team playerTeam{get; private set;}
        public GameStartEvent(string pPlayerName,string pEnemyName,Team pPlayerTeam){
            playerName = pPlayerName;
            enemyName = pEnemyName;
            playerTeam = pPlayerTeam;
        }
        public GameStartEvent(){}
        public void Serialize(Packet pPacket)
        {
            pPacket.Write(playerName);
            pPacket.Write(enemyName);
            pPacket.Write((int)playerTeam);
        }

        public void Deserialize(Packet pPacket)
        {
            playerName = pPacket.ReadString();
            enemyName = pPacket.ReadString();
            playerTeam = (Team)pPacket.ReadInt();
        }
    }

