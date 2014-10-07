uniform mat4 u_MVP;
uniform mat4 u_MVMatrix;
uniform mat4 u_Model;
uniform vec3 u_LightPos;
uniform float u_IsFloor;

attribute vec4 a_Position;
attribute vec4 a_Color;
attribute vec3 a_Normal;
attribute vec2 a_texcoord;

varying vec4 v_Color;
varying vec3 v_Grid;
varying float v_isFloor;
varying vec2 f_texcoord;

void main()
{
   vec3 modelVertex = vec3(u_Model * a_Position);
   v_Grid = modelVertex;

   vec3 modelViewVertex = vec3(u_MVMatrix * a_Position);
   vec3 modelViewNormal = vec3(u_MVMatrix * vec4(a_Normal, 0.0));
   float distance = length(u_LightPos - modelViewVertex);
   vec3 lightVector = normalize(u_LightPos - modelViewVertex);
   float diffuse = max(dot(modelViewNormal, lightVector), 0.5);
   diffuse = diffuse * (1.0 / (1.0 + (0.00001 * distance * distance)));
   gl_Position = u_MVP * a_Position;
   if (u_IsFloor > 0.5) {
   	v_Color = a_Color * diffuse;
   } else {
	v_Color = vec4(0.0);
   }

   v_isFloor = u_IsFloor;
   f_texcoord = a_texcoord;
}