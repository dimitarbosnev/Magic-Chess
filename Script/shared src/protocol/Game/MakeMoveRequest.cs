
    /**
     * Send from CLIENT to SERVER to indicate the move the client would like to make.
     * Since the board is just an array of cells, move is a simple index.
     */
    public class MakeMoveRequest : ISerializable
    {
        public Command command;

        public void Serialize(Packet pPacket)
        {
            pPacket.Write(command);
        }

        public void Deserialize(Packet pPacket)
        {
            command = pPacket.Read<Command>();
        }
    }

