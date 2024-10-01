using System;
using System.Collections.Generic;
using Godot;

public enum PickupType{
    Normal = 0,
    Ability = 1,
}
public abstract class Event{}
public class EventBus<T> where T : Event
{
    public static event Action<T> OnEvent;
    public static void Invoke(T pEvent){
        OnEvent?.Invoke(pEvent);
    }
}
public class PiecePickUpEvent : Event{
    public ChessPiece chessPiece {get; private set;}
    public List<Vector2I> moves{get; private set;}
    public PiecePickUpEvent(ChessPiece pChessPiece, List<Vector2I> pMoves){
        chessPiece = pChessPiece;
        moves = pMoves;
    }
}
public class PieceReleaseEvent : Event{
    public ChessPiece chessPiece {get; private set;}
    public Command command{get; private set;}
    public PieceReleaseEvent(ChessPiece pChessPiece, Command pCommand){
        chessPiece = pChessPiece;
        command = pCommand;
    }
}
public class PieceKillEvent : Event{
    public ChessPiece chessPiece {get; private set;}
    public PieceKillEvent(ChessPiece pChessPiece){
        chessPiece = pChessPiece;
    }
}

public class NewTurnEvent : Event{
    public Team team{get; private set;}
    public NewTurnEvent(Team pTeam){
        team = pTeam == Team.Red? Team.Blue : Team.Red;
    }
}
public class GameSetupEvent : Event{
     public Team team{get; private set;}

     public GameSetupEvent(Team pTeam){
        team = pTeam;
     }
}
public class CommandMessageRecived : Event{

    public Command command {get; private set;}
    public CommandMessageRecived(Command pCommand){ command = pCommand;}
}