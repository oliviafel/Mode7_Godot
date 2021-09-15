using Godot;
using System;

// doing this because we don't want the sprite to rotate along with the kinematic body

public class SpriteFollow : Sprite
{
    [Export] public string _targetName = "Player" ;
    private KinematicBody2D _body = null ;
    public override void _Ready()
    {
        _body = (KinematicBody2D) GetTree().Root.FindNode(_targetName, true, false ) ;
    }
    public override void _Process(float delta)
    {
        GlobalPosition = _body.Position ;
    }
}
