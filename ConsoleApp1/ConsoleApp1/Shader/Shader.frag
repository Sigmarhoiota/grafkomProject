#version 330

out vec4 outputcolor;

in vec4 vertexcolor;

uniform vec3 objColor;

void main(){
	outputcolor = vec4(objColor,1.0);
}
