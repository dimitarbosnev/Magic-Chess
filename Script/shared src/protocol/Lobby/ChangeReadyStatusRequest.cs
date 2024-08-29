
    /**
     * Send from CLIENT to SERVER to request enabling/disabling the ready status.
     */
    public class ChangeReadyStatusRequest : ISerializable
    {
        public bool ready = false;

        public void Serialize(Packet pPacket)
        {
            pPacket.Write(ready);
        }

        public void Deserialize(Packet pPacket)
        {
            ready = pPacket.ReadBool();
        }
    }

