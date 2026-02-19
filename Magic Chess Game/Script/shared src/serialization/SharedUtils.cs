public static class SharedUtils{
    	public static TileStruct SetTileStruct(BoardStruct boardTile){
    TileStruct tileStruct = new TileStruct{
            x = boardTile.tile.coordinates.X,
            y = boardTile.tile.coordinates.Y
        };
        if(boardTile.piece != null)
            tileStruct.piece = new PieceStruct{
                pieceType = boardTile.piece.PieceType,
                team = boardTile.piece.Team,
                pieceID = boardTile.piece.id,
				ability = boardTile.piece.Ability
        };
    return tileStruct;
    }

	public static PieceStruct SetPieceStruct(BoardStruct boardTile){
        if(boardTile.piece != null){
			PieceStruct pieceStruct;
          	pieceStruct = new PieceStruct{
                pieceType = boardTile.piece.PieceType,
                team = boardTile.piece.Team,
                pieceID = boardTile.piece.id,
				ability = boardTile.piece.Ability
        	};
    		return pieceStruct;
		}
		else return null;
    }
	public static TileStruct SetTileStruct(ChessPiece chessPiece){
		return SetTileStruct(ChessBoard.Instance.GetBoardStruct(chessPiece));
	}
	public static TileStruct SetTileStruct(Tile tile){
		return SetTileStruct(ChessBoard.Instance.GetBoardStruct(tile));
	}

	public static TileStruct SetTileStruct(int x, int y, PieceStruct piece){
		TileStruct tile = new TileStruct();

		tile.x = x;
		tile.y = y;
		tile.piece = piece;

		return  tile;
	}
    public static bool CompareTileStructs(TileStruct a, TileStruct b){
		if(a.x != b.x && a.y != b.y) return false;
		//TODO: Check for corrupted data
		if(a.piece == null && b.piece == null) return false;

		if(!ComparePieceStructs(a.piece, b.piece)) return false;

		return true;
	}

	    public static bool ComparePieceStructs(PieceStruct a, PieceStruct b){
		if(a.pieceID != b.pieceID) return false;
		if(a.team != b.team) return false;
		if(a.pieceType != b.pieceType) return false;


		return true;
	}
}