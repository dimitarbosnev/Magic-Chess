using Godot;
public partial class Soldier : ChessPiece
{
    public Soldier() : base(){
		Ability = false;
	}
    public override Command AbilityMove(BoardStruct target)
    {
        return new MoveCommand(SharedUtils.SetTileStruct(this), SharedUtils.SetTileStruct(target));
    }
}
