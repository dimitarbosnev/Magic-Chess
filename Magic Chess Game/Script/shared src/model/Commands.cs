using System.Collections.Generic;

public struct TileStruct : ISerializable{
    public int x,y;
    public PieceStruct? piece;
    public void Serialize(Packet packet){
        packet.Write(x);
        packet.Write(y);
        packet.Write(piece != null);
        if(piece != null)
            packet.Write(piece);
    }
    public void Deserialize(Packet packet){
        x = packet.ReadInt();
        y = packet.ReadInt();
        if(packet.ReadBool()) piece = packet.Read<PieceStruct>();
        else piece = null;
    }
}
public class PieceStruct : ISerializable{
    public PieceType pieceType;
    public Team team;
    public int pieceID;
    public bool ability;

    public void Serialize(Packet packet){
        packet.Write(pieceID);
        packet.Write((int)team);
        packet.Write((int)pieceType);
        packet.Write(ability);
    }
    public void Deserialize(Packet packet){
        pieceID = packet.ReadInt();
        team = (Team)packet.ReadInt();
        pieceType = (PieceType)packet.ReadInt();
        ability = packet.ReadBool();
    }
}
public abstract class Command : ISerializable{
    public abstract void execute(ref BoardStruct[][] board);
    public abstract void reverse(ref BoardStruct[][] board);
    public abstract bool validateMove(ref PieceStruct[][] boardData);
    public abstract void updateBoardData(ref PieceStruct[][] boardData);
    public abstract void revertBoardData(ref PieceStruct[][] boardData);
    public abstract void Serialize(Packet packet);
    public abstract void Deserialize(Packet packet);
}

public class MoveCommand : Command {
    public TileStruct pickup;
    public TileStruct target;
    public MoveCommand() : base(){}
    public MoveCommand(TileStruct pPickup, TileStruct pTarget) {
        pickup = pPickup;
        target = pTarget;
    }
    public override void execute(ref BoardStruct[][] board) {
		BoardStruct targetTile = board[target.y][target.x];

        if(pickup.piece != null){
            ChessPiece chessPiece1 = ChessBoard.Instance.GetPieceById(pickup.piece.pieceID);
            ChessBoard.Instance.AssignPiece(chessPiece1, targetTile);
            board[pickup.y][pickup.x].piece = null;

            if(target.piece != null){
                ChessPiece chessPiece2 = ChessBoard.Instance.GetPieceById(target.piece.pieceID);
                EventBus<PieceKillEvent>.Invoke(new PieceKillEvent(chessPiece2));
            }
            EventBus<NewTurnEvent>.Invoke(new NewTurnEvent(chessPiece1.Team));
        }
    }
    public override void reverse(ref BoardStruct[][] board) {
        BoardStruct pickupTile = board[pickup.y][pickup.x];
		BoardStruct targetTile = board[target.y][target.x];

        if(pickup.piece != null){
            ChessPiece chessPiece1 = ChessBoard.Instance.GetPieceById(pickup.piece.pieceID);
            ChessBoard.Instance.AssignPiece(chessPiece1, pickupTile);
        
            if(target.piece != null){
                ChessPiece chessPiece2 = ChessBoard.Instance.GetPieceById(target.piece.pieceID);
                ChessBoard.Instance.AssignPiece(chessPiece2, targetTile);
            }
        }
        
    }
    public override bool validateMove(ref PieceStruct[][] boardData){
        bool valid1 = false;
        bool valid2 = false;

        if(pickup.piece == null || boardData[pickup.y][pickup.x] == null) return false;
        
        if(SharedUtils.ComparePieceStructs(pickup.piece,boardData[pickup.y][pickup.x]))
            valid1 = true;
        if(SharedUtils.ComparePieceStructs(target.piece,boardData[target.y][target.x]))
            valid2 = true;

        return valid1 && valid2;
    }
    public override void updateBoardData(ref PieceStruct[][] boardData){
        boardData[pickup.y][pickup.x] = null;
        boardData[target.y][target.x] = pickup.piece;
    }
    
    public override void revertBoardData(ref PieceStruct[][] boardData){
        boardData[pickup.y][pickup.x] = pickup.piece;
        boardData[target.y][target.x] = target.piece;
    }

    public override void Serialize(Packet packet) {
        packet.Write(pickup);
        packet.Write(target);
    }

    public override void Deserialize(Packet packet) {
        pickup = packet.Read<TileStruct>();
        target = packet.Read<TileStruct>();
    }
}

public class ChangeBoardCommand : Command {
    public ChessBoardData chessBoardData;
    public ChangeBoardCommand() : base(){}
    public ChangeBoardCommand(ChessBoardData pChessBoardData) {
        chessBoardData = pChessBoardData;
    }
    public override void execute(ref BoardStruct[][] board) {
        PieceStruct[][] boardData = chessBoardData.board;
		for (int y = 0; y < board.Length; y++)
			for (int x = 0; x <	board[y].Length; x++){
                ChessBoard.Instance.RespawnPiece(x,y,boardData[y][x]);
            }
    }
    public override void reverse(ref BoardStruct[][] board) {
        
    }
    public override bool validateMove(ref PieceStruct[][] boardData){
        return true;
    }
    public override void updateBoardData(ref PieceStruct[][] boardData){}
    public override void revertBoardData(ref PieceStruct[][] boardData){}

    public override void Serialize(Packet packet) {
        packet.Write(chessBoardData);
    }

    public override void Deserialize(Packet packet) {
        chessBoardData = packet.Read<ChessBoardData>();
    }
}