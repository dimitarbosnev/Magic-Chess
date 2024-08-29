
    /**
     * Send from CLIENT to SERVER to indicate the move the client would like to make.
     * Since the board is just an array of cells, move is a simple index.
     */
    public class GameStartEvent : ISerializable
    {
        public string player1Name;
        public string player2Name;
        public void Serialize(Packet pPacket)
        {
            pPacket.Write(player1Name);
            pPacket.Write(player2Name);
        }

        public void Deserialize(Packet pPacket)
        {
            player1Name = pPacket.ReadString();
            player2Name = pPacket.ReadString();
        }
    }

