
    /**
     * Send from SERVER to all CLIENTS in response to a client's MakeMoveRequest
     */
    public class MakeMoveResult : ISerializable
    {
        public Command command;
        //public ChessBoardData boardData;

        public MakeMoveResult(){}

        public MakeMoveResult(Command pCommand){
            command = pCommand;
        }
        public void Serialize(Packet pPacket)
        {
            pPacket.Write(command);
            //pPacket.Write(boardData);
        }

        public void Deserialize(Packet pPacket)
        {
            command = pPacket.Read<Command>();
            //boardData = pPacket.Read<ChessBoardData>();
        }
    }

