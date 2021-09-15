using Godot;
using System;

public class CameraFollow : Camera2D
{
    [Export] public string _mainCameraName = "Camera2D" ;
    private Godot.Camera2D _cameraTarget = null ;
    public override void _Ready()
    {
        _cameraTarget = (Godot.Camera2D) GetTree().Root.FindNode(_mainCameraName, true, false ) ;
    }
    public override void _Process(float delta)
    {
        GlobalTransform = _cameraTarget.GlobalTransform ;
    }
}
