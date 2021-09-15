using Godot;
using System;

public class Player : KinematicBody2D
{
    [Export] public Vector2 _speed = Vector2.One ;

    private Vector2 _velocity = Vector2.Zero ;
    private Camera2D _camera = null ;

    public override void _Ready()
    {
        _camera = GetNodeOrNull<Camera2D>("Camera2D_Player") ;
    }

    public override void _Process(float delta)
    {
        _velocity = Vector2.Zero ;

        if ( Input.IsActionPressed("up") )
        {
            _velocity.y = -_speed.y ;
        }
        if ( Input.IsActionPressed("down") )
        {
            _velocity.y = _speed.y ;
        }
        if ( Input.IsActionPressed("left") )
        {
            GlobalRotation -= _speed.x * delta ;
        }
        if ( Input.IsActionPressed("right") )
        {
            GlobalRotation += _speed.x * delta ;
        }

        _velocity = _velocity.Rotated( GlobalRotation ) ;

        _velocity = MoveAndSlide( _velocity, Vector2.Up ) ;
    }
}