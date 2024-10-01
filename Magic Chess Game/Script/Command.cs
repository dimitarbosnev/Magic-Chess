using Godot;

public class PieceStruct : ISerializable{
    public Vector2I cord{get; private set;}
    public PieceType pieceType{get; private set;}
    public Team team{get; private set;}
    public int pieceID{get; private set;}
    
    public PieceStruct(){}
    public PieceStruct(ChessPiece piece){
        cord = piece.coordinates;
        pieceID = piece.pieceID;
        pieceType = piece.PieceType;
        team = piece.Team;
    }
    public PieceStruct(PieceStruct pStruct){
        cord = pStruct.cord;
        pieceID = pStruct.pieceID;
        pieceType = pStruct.pieceType;
        team = pStruct.team;
    }
    public bool PieceEqual(PieceStruct obj)
    {
       return this.cord == obj.cord && this.pieceID == obj.pieceID && this.team == obj.team && this.pieceType == obj.pieceType;
    }
    
    public void Serialize(Packet packet){
        packet.Write(cord);
        packet.Write(pieceID);
    }
    public void Deserialize(Packet packet){
        cord = packet.ReadVector2I();
        pieceID = packet.ReadInt();
    }
}

public class TileStruct: ISerializable{
    public Vector2I cord{get; private set;}
    public PieceStruct piece{get; private set;}

    public TileStruct(){}
    public TileStruct(TileStruct pStruct){
        cord = pStruct.cord;
        if(pStruct.piece != null)
            piece = new PieceStruct(pStruct.piece);
    }
    public TileStruct(Tile tile){
        cord = tile.coordinates;
        if(tile.piece != null)
            piece = new PieceStruct(tile.piece);
    }
    public void Serialize(Packet packet){
        packet.Write(cord);
        packet.Write(piece != null);
        if(piece != null)
            packet.Write(piece);
    }
    public void Deserialize(Packet packet){
        cord = packet.ReadVector2I();
        bool isPieceNull = packet.ReadBool();
        if(isPieceNull)
            piece = packet.Read<PieceStruct>();
        
    }
}
public abstract class Command : ISerializable{
    public abstract void execute(ref Tile[][] board);
    public abstract void reverse(ref Tile[][] board);
    public abstract void Serialize(Packet packet);
    public abstract void Deserialize(Packet packet);
}
public class MoveCommand : Command {
    public PieceStruct pickup;
    public TileStruct target;
    private ChessPiece killedPiece;
    private ChessPiece chessPiece;
    public MoveCommand() : base(){}
    public MoveCommand(ChessPiece pPickup, Tile pTarget) {
        pickup = new PieceStruct(pPickup);
        target = new TileStruct(pTarget);
    }
    public override void execute(ref Tile[][] board) {
        Tile targetTile = board[target.cord.Y][target.cord.X];
        if (targetTile.piece != null){
            killedPiece = targetTile.piece;
            EventBus<PieceKillEvent>.Invoke(new PieceKillEvent(targetTile.piece));
        }
        chessPiece = board[pickup.cord.Y][pickup.cord.X].piece;
        ChessBoard.AssignPiece(chessPiece,targetTile);
        EventBus<NewTurnEvent>.Invoke(new NewTurnEvent(targetTile.piece.Team));
    }
    public override void reverse(ref Tile[][] board) {
        GD.Print("Command Reversing");
        Tile targetTile = board[target.cord.Y][target.cord.X];
        if (killedPiece != null)
            ChessBoard.AssignPiece(killedPiece, targetTile);
        ChessBoard.AssignPiece(chessPiece, board[pickup.cord.Y][pickup.cord.X]);
    }

    public override void Serialize(Packet packet) {
        packet.Write(pickup);
        packet.Write(target);
    }

    public override void Deserialize(Packet packet) {
        pickup = packet.Read<PieceStruct>();
        target = packet.Read<TileStruct>();
    }
}


