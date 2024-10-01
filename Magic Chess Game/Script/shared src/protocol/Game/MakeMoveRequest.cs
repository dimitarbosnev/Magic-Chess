
    /**
     * Send from CLIENT to SERVER to indicate the move the client would like to make.
     * Since the board is just an array of cells, move is a simple index.
     */
    public class MakeMoveRequest : ISerializable
    {
        public Command command {get; private set;}
        //public ChessBoardData chessBoardData;

        public MakeMoveRequest(){}

        public MakeMoveRequest(Command pCommand){
            command = pCommand;
        }
        public void Serialize(Packet pPacket)
        {
            pPacket.Write(command);
            //pPacket.Write(chessBoardData);
        }

        public void Deserialize(Packet pPacket)
        {
            command = pPacket.Read<Command>();
            //chessBoardData = pPacket.Read<ChessBoardData>();
        }
    }

