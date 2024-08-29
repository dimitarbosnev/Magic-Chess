     public enum PieceType
    {
        None = 0,
        Soldier = 1,
        RedKing = 2,
        BlueKing = 3,
        Queen = 4,
        Assassin = 5,
        Centaur = 6,
        Shapeshifter = 7,
        Mage = 8,
    }

    public enum Team
    {
        None = 0,
        Red = 1,
        Blue = 2
    }
    /**
     * Classes that extend ASerializable can (de)serialize themselves into/out of a Packet instance. 
     * See the classes in the protocol package for an example. 
     * This base class provides a ToString method for simple (and slow) debugging.
     */
    public interface ISerializable
    {
         void Serialize(Packet pPacket);
         void Deserialize(Packet pPacket);
    }

