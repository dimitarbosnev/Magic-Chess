
	/**
	 * BIDIRECTIONAL Chat message for the lobby
	 */
	public class ChatMessage : ISerializable
	{
		public string message;

		public void Serialize(Packet pPacket)
		{
			pPacket.Write(message);
		}

		public void Deserialize(Packet pPacket)
		{
			message = pPacket.ReadString();
		}
	}

