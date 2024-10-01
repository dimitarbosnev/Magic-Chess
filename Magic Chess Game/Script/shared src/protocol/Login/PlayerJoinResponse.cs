
    /**
     * Send from SERVER to CLIENT to let the client know whether it was allowed to join or not.
     * Currently the only possible result is accepted.
     */
    public class PlayerJoinResponse : ISerializable
    {
        public enum RequestResult { ACCEPTED, REJECTED}; //can add different result states if you want
        public RequestResult result{get; private set;}

        public string errorMsg{get; private set;}
        public PlayerJoinResponse(){}

        public PlayerJoinResponse(RequestResult pResult, string pErrorMsg = ""){
            result = pResult;
            errorMsg = pErrorMsg;
        }


        public void Serialize(Packet packet)
        {
           packet.Write((int)result);
           packet.Write(errorMsg);        
        }

        public void Deserialize(Packet packet)
        {
            result = (RequestResult)packet.ReadInt();
            errorMsg = packet.ReadString();
        }
    }

