
	/**
	 * Send from SERVER to CLIENT to notify that the client has joined a specific room (i.e. that it should change state).
	 */
	public class RoomJoinedEvent : ISerializable
	{
		public enum Room { LOGIN_ROOM, LOBBY_ROOM, GAME_ROOM };
		public Room room;

		public void Serialize(Packet pPacket)
		{
			pPacket.Write((int)room);
		}

		public void Deserialize(Packet pPacket)
		{
			room = (Room)pPacket.ReadInt();
		}
	}

