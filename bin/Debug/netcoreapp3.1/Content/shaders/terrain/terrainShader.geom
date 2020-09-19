#version 330 core

layout (triangles) in;
layout (triangle_strip, max_vertices = 3) out;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

in vec3 Normal[];
in vec3 FragPos[];
in vec2 TexCoords[];
in float modifiedY[];

out vec3 geomNormal;
out vec3 geomFragPos;
out vec2 geomTexCoords;
out float geomModifiedY;

vec3 calcAvgNormal()
{
	vec3 tangent1 = gl_in[1].gl_Position.xyz - gl_in[0].gl_Position.xyz;
	vec3 tangent2 = gl_in[2].gl_Position.xyz - gl_in[0].gl_Position.xyz;
	vec3 normal = cross(tangent1, tangent2);
	return normalize(normal);
}

vec3 calcAvgFragPos()
{
	vec3 avgFragPos = (FragPos[0] + FragPos[1] + FragPos[2]) / 3;
	return avgFragPos;
}

float calcAvgModifiedY()
{
	float avgModifiedY = (modifiedY[0] + modifiedY[1] + modifiedY[2]) / 3;
	return avgModifiedY;
}

void main()
{
	for(int i=0;i<3;i++)
	{
		gl_Position = gl_in[i].gl_Position  * model * view * projection;

		// pass inputs to the fragment shader
		geomTexCoords= TexCoords[i];

		//geomNormal = Normal[i]; // Per pixel lighting
		geomNormal = calcAvgNormal(); // Per vertex lighting

		//geomFragPos= FragPos[i]; // Per pixel lighting
		geomFragPos= calcAvgFragPos(); // Per vertex lighting

		//geomModifiedY= modifiedY[i]; // Per pixel material
		geomModifiedY= calcAvgModifiedY(); // Per vertex material

		EmitVertex();
	}

	EndPrimitive();
}