     public enum PieceType
    {
        None = 0,
        HexSoldier = 1,
        HexRedKing = 2,
        HexBlueKing = 3,
        HexQueen = 4,
        HexAssassin = 5,
        HexCentaur = 6,
        HexShapeshifter = 7,
        HexMage = 8,

        RectSoldier = 9,
        RectRedKing = 10,
        RectBlueKing = 11,
        RectQueen = 12,
        RectAssassin = 13,
        RectCentaur = 14,
        RectShapeshifter = 15,
        RectMage = 16,

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

