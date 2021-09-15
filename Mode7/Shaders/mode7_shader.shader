shader_type canvas_item;

uniform int mode:hint_range(0,3) = 0 ;
uniform mat4 transform ;
uniform vec2 depth ;
uniform bool repeat_x = false ;
uniform bool repeat_y = false ;

void fragment() 
{
	vec2 flip_uvs = UV ;
	// bottom of the sprite is leaving the UVs alone
	// top of the sprite
	if ( mode == 1 )
	{
		flip_uvs.y = 1f - UV.y ;
	}
	// left of the sprite
	else if ( mode == 2 )
	{
		flip_uvs.x = UV.y ;
		flip_uvs.y = 1f - UV.x ;
	}
	// right of the sprite
	else if ( mode == 3 )
	{
		flip_uvs.x = UV.y ;
		flip_uvs.y = UV.x ;	
	}
	// Create the matrix. A workaround is used to modify the matrix's W column
	// because Godot's transforms are 3x4, not 4x4.
	mat4 mat = mat4(1.0) ;
	mat[0].w = depth.x ;
	mat[1].w = depth.y ;
	// Transform UV into [-1, 1] range
	vec2 uv = flip_uvs * 2.0 - vec2(1, 1) ;
	// Turn position into 4d vector
	vec4 pos = vec4(uv, 1f, 1f ) ;
	pos = mat * pos ;
	pos.xy = pos.xy / pos.w ;
	// Apply transformation to position
	float w = pos.w;
	pos.z = 0f ;
	pos.w = 1f ;
	// Apply depth to position
	pos = transform * pos ;
	// divide position by w coordinate; this applies perspective
	// Set UV to position; transform its range to [0, 1]
	uv = (pos.xy + vec2(1, 1)) * 0.5;
	
	// renders UVs for debugging
	//COLOR = uv.x >= 0.0 && uv.x <= 1.0 ? vec4(uv, 0.0, 1.0) : vec4(vec3(0.0), 1.0);
	
	// Determine if uv is in range or repeating pattern
	if ((( uv.x >= 0.0 && uv.x <= 1.0) || repeat_x ) && 
		(( uv.y >= 0.0 && uv.y <= 1.0) || repeat_y ) && 
		 ( w >= 0.0 ) )
	{
		// Apply texture
		//uv = mod(uv, 1f) ;
		COLOR = texture( TEXTURE, uv ) ;
	} 
	else
	{
		COLOR = vec4(0, 0, 0, 0) ;
	}
}