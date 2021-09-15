using Godot;
using System;
public class ViewportContainerMode7 : ViewportContainer
{
    [Export] public float _rotationSpeed = 0f ;
    [Export] public Vector2 _depth = Vector2.Zero ;

    private ShaderMaterial _shader = null ;
    private Transform2D _virtualTransform = Transform2D.Identity ;
    private Camera2D _camera = null ;
    private Basis _perspectiveMatrix = Basis.Identity ;

    public override void _Ready()
    {
        Material = (ShaderMaterial) Material ;
        _shader = (ShaderMaterial) Material ;
        _virtualTransform = Transform2D.Identity ;
        _virtualTransform.Scale = Vector2.One ;

        _camera = GetNodeOrNull<Camera2D>("Viewport/Viewport_Camera") ;

        _shader.SetShaderParam("depth", _depth ) ;
        _perspectiveMatrix = Create2DPerspectiveMatrix() ;
    }
    public override void _Process(float delta)
    {
        RectGlobalPosition = _camera.GlobalPosition - RectSize / 2  ;

        Movement( delta ) ;

        UpdateShaderParams() ;
    }
    private Basis Create2DPerspectiveMatrix()
    {
        Basis matrix = Basis.Identity ;

        matrix.Column0 = new Vector3( 1f, 0f, _depth.x ) ;
        matrix.Column1 = new Vector3( 0f, 1f, _depth.y ) ;
        matrix.Column2 = new Vector3( 0f, 0f, 1f ) ;

        return matrix ;
    }
    public Vector3 TransformPosition( Vector2 position )
    {
        Transform2D spriteTransform = Transform2D.Identity ;

        // position is based on the camera positions, work in progress will use the virtual transform for now
        // scale is based on the mode 7 backgrounds virtual transform
        // rotation is based on the mode 7 backgrounds virtual transform
        //workingTransform.origin = _camera.GlobalPosition ;

        // transform position from world space to -1 to 1 
        Vector2 worldPos = (position / (GetViewportRect().Size / 2f)) - Vector2.One ;

        // combined perspective and view transforms
        Basis viewBasis = Basis.Identity ;

        viewBasis.Column0 = new Vector3( _virtualTransform.x.x, _virtualTransform.x.y, 0f ) ;
        viewBasis.Column1 = new Vector3( _virtualTransform.y.x, _virtualTransform.y.y, 0f ) ;
        viewBasis.Column2 = new Vector3( _virtualTransform.origin.x , _virtualTransform.origin.y, 1f ) ;

        //Basis pv = _perspectiveMatrix * viewBasis ;
        Basis pv = viewBasis * _perspectiveMatrix ;

        Vector3 pos = new Vector3( worldPos.x, worldPos.y, 1f ) ;
        pos = pv.Xform( pos ) ;
        pos.x = pos.x / pos.z ;
        pos.y = pos.y / pos.z ;

        worldPos.x = pos.x ;
        worldPos.y = pos.y ;

        // transform back to worldPos
        worldPos = (worldPos + Vector2.One) * (GetViewportRect().Size / 2f) ;
        pos.x = worldPos.x ;
        pos.y = worldPos.y ;

        return pos ;
    }
    private void Movement( float delta )
    {
        Vector2 motion = Vector2.Zero ;

        Vector2 _scrollSpeed = Vector2.One ;

        // Movement is handled in the Player object
        // Uncomment this if you just want to test everything going on with the virtual transform
        /*
        if ( Input.IsActionPressed("up") )
        {
            motion.y -= _scrollSpeed.y ;
        }
        if ( Input.IsActionPressed("down") )
        {
            motion.y += _scrollSpeed.y ;
        }
        _virtualTransform.origin += ( motion * delta ) ;*/

        // Scaling here if wanted in the future
        /*
        Vector2 zoom = Vector2.Zero ;
        if ( Input.IsActionPressed("jump") )
        {
            zoom.y += _scaleSpeed.y ;
            zoom.x += _scaleSpeed.x ;
        }
        if ( Input.IsActionPressed("slide_dive") )
        {
            zoom.y -= _scaleSpeed.y ;
            zoom.x -= _scaleSpeed.x ;
        }
        _virtualTransform.Scale += ( zoom * delta ) ;*/

        // Rotations are handled here so the background lines up with the direction of player movement
        float rotate = 0f ;
        if ( Input.IsActionPressed("left") )
        {
            rotate += _rotationSpeed ;
        }
        if ( Input.IsActionPressed("right") )
        {
            rotate -= _rotationSpeed ;
        }
        _virtualTransform.Rotation += ( rotate * delta ) ;
    }
    private void UpdateShaderParams()
    {
        if ( _shader != null )
        {
            _shader.SetShaderParam("transform", _virtualTransform ) ;
        }
    }
}
