
    /**
     * Send from SERVER to all CLIENTS in response to a client's MakeMoveRequest
     */
    public class MakeMoveResult : ISerializable
    {
        public int whoMadeTheMove;
        public ServerChessBoard boardData;

        public void Serialize(Packet pPacket)
        {
            pPacket.Write(whoMadeTheMove);
            pPacket.Write(boardData);
        }

        public void Deserialize(Packet pPacket)
        {
            whoMadeTheMove = pPacket.ReadInt();
            boardData = pPacket.Read<ServerChessBoard>();
        }
    }

