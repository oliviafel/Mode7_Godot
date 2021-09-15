using Godot;
using System;

public class Mode7Object : Node2D
{
    private Sprite _sprite = null ;
    private ViewportContainerMode7 _vc = null ;
    private ShaderMaterial _shader = null ;
    public override void _Ready()
    {
        _sprite = GetNodeOrNull<Sprite>("Sprite") ;
        _vc = (ViewportContainerMode7) GetTree().Root.FindNode("ViewportContainer", true, false ) ;
        _shader = (ShaderMaterial) _sprite.Material ;
    }
    public override void _Process(float delta)
    {
        if ( _sprite != null && _vc != null )
        {
            Vector3 position = _vc.TransformPosition( this.GlobalPosition ) ;

            if (position.z < 0 ) _sprite.Visible = false ;
            else
            {
                if ( _sprite.Visible == false ) _sprite.Visible = true ;
                Vector2 newPos = Vector2.Zero ;
                newPos.x = position.x ;
                newPos.y = position.y ;
                _sprite.GlobalPosition = newPos ;
            }
        }
    }
}
