using Godot;

public partial class Queen : ChessPiece
{
	public Queen() : base(){
		Ability = false;
	}
    public override Command AbilityMove(BoardStruct target){
        return new MoveCommand(SharedUtils.SetTileStruct(this), SharedUtils.SetTileStruct(target));
    }
	public override void OnHoverInput(PlayerFSM playerFSM, InputEventMouseButton mouseEvent){
		if(mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
				playerFSM.TransitToState(typeof(PlayerNormalHoldState));
	}
}
