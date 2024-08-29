
    /**
     * Send from SERVER to CLIENT to let the client know whether it was allowed to join or not.
     * Currently the only possible result is accepted.
     */
    public class PlayerJoinResponse : ISerializable
    {
        //public enum RequestResult { ACCEPTED, REJECTED}; //can add different result states if you want
        //public RequestResult result;
        public Team playerTeam;

        public void Serialize(Packet pPacket)
        {
           // pPacket.Write((int)result);
           pPacket.Write(playerTeam);
        }

        public void Deserialize(Packet pPacket)
        {
            //result = (RequestResult)pPacket.ReadInt();
            playerTeam = pPacket.ReadTeam();
        }
    }

