
    /**
     * Send from SERVER to all CLIENTS in response to a client's MakeMoveRequest
     */
    public class GameEndEvent : ISerializable
    {
        public string winnerName;
        public Team winnerTeam;

        public void Serialize(Packet packet)
        {
            packet.Write(winnerName);
            packet.Write((int)winnerTeam);
        }

        public void Deserialize(Packet packet)
        {
            winnerName = packet.ReadString();
            winnerTeam = (Team)packet.ReadInt();
        }
    }

