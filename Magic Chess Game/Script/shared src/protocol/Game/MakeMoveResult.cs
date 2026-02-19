
    /**
     * Send from SERVER to all CLIENTS in response to a client's MakeMoveRequest
     */
    public class MakeMoveResult : ISerializable
    {
        public Command command;

        public MakeMoveResult(){}

        public MakeMoveResult(Command pCommand){
            command = pCommand;
        }
        public void Serialize(Packet pPacket)
        {
            pPacket.Write(command);
        }

        public void Deserialize(Packet pPacket)
        {
            command = pPacket.Read<Command>();
        }
    }

