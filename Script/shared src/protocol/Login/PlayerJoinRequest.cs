
    /**
     * Send from CLIENT to SERVER to request joining the server.
     */
    public class PlayerJoinRequest : ISerializable
    {
        public string name;

        public void Serialize(Packet pPacket)
        {
            pPacket.Write(name);
        }

        public void Deserialize(Packet pPacket)
        {
            name = pPacket.ReadString();
        }
    }

