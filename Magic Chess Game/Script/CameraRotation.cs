using Godot;

public partial class CameraRotation : Camera3D
{
	public Vector3 targetPosition;
	float baseFOV = 90;
	//float baseSize = 10;
	float zoomScale = 0.7f;
	float zoomMargin = 0.01f;
	[Export] float targetDistance = 7;
	Vector2 previousPosition;
	[Export] float rotationSpeed = .5f;

	public Vector3 baseRotation;
	public override void _Ready()
	{
		SetPerspective(baseFOV*zoomScale,0.01f,1000);
	}
	public void AdaptCamera(Team team){
		baseRotation = team == Team.Blue? new Vector3(-45,180,0) : new Vector3(-45,0,0);
		RotationDegrees = baseRotation;
		RotateCamera(Vector2.Zero,Vector2.Zero,0);
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 mousePos = GetViewport().GetMousePosition();
		if(Input.IsMouseButtonPressed(MouseButton.Right) && Input.IsKeyPressed(Key.Shift)) RotateCamera(mousePos,previousPosition,(float)delta);
		previousPosition = mousePos;
	}

	public void RotateCamera(Vector2 mousePos,Vector2 prevPosition, float delta){
		Vector2 deltaPos = prevPosition - mousePos;
		Position = targetPosition;
		GlobalRotate(Vector3.Right,deltaPos.Y*rotationSpeed*delta);
		GlobalRotate(Vector3.Up,deltaPos.X*rotationSpeed*delta);
		Rotation = new Vector3(Rotation.X,Rotation.Y,0);
		TranslateObjectLocal(Vector3.Back * targetDistance);

	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey eventKey && eventKey.Pressed)
			switch(eventKey.Keycode) {
				case Key.Space: {
					Rotation = baseRotation;
					zoomScale = 0.7f;
					RotateCamera(Vector2.Zero,Vector2.Zero,0);
				}
				break;
				/*case Key.Y:
				switch (Projection)
				{
					case ProjectionType.Perspective:
						SetOrthogonal(baseSize*zoomScale,0.01f,1000);
					break;

					case ProjectionType.Orthogonal:
						SetPerspective(baseFOV*zoomScale,0.01f,1000);
					break;

					default:
					break;
				}
				break;*/
			}
		if(@event is InputEventMouseButton mouseButton)
			switch(mouseButton.ButtonIndex) {
				case MouseButton.WheelUp:
				if(zoomScale > 0){
					zoomScale -= zoomMargin;
					SetPerspective(baseFOV*zoomScale,0.01f,1000);
				}
				break;
				case MouseButton.WheelDown:
				if(zoomScale < 1){
					zoomScale += zoomMargin;
					SetPerspective(baseFOV*zoomScale,0.01f,1000);
				}
				break;
			}
	}
}
