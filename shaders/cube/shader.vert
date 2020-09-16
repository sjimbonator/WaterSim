#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoords;

layout (std140) uniform Matrices
{
    mat4 projection;
    mat4 view;
};
uniform mat4 model;

out vec3 Normal;
out vec3 FragPos;
out vec2 TexCoords;

void main()
{

    gl_Position = vec4(aPosition, 1.0) * model * view * projection;
    Normal = aNormal * mat3(transpose(inverse(model)));  
    FragPos = vec3(vec4(aPosition, 1.0)* model);
    TexCoords = aTexCoords;
}
