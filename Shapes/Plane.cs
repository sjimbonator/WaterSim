using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace WaterSim
{
    class Plane : Shape
    {
        public Plane(Shader shader, Dictionary<String, dynamic> uniforms, float size = 200, int VertexCount = 32)
        {
            _shader = shader;
            _uniforms = uniforms;

            GeneratePlane(size, VertexCount);
            Setup();
        }

        private void GeneratePlane(float size, int VertexCount)
        {
            // generate vertices
            _vertices = new float[(VertexCount * VertexCount) * 8];
            int vertexPointer = 0;
            int stride = 8;
            for (int i = 0; i < VertexCount; i++)
            {
                for (int j = 0; j < VertexCount; j++)
                {
                    // positions
                    _vertices[vertexPointer * stride] = (float)j / ((float)VertexCount - 1) * size;
                    _vertices[vertexPointer * stride + 1] = 0;
                    _vertices[vertexPointer * stride + 2] = (float)i / ((float)VertexCount - 1) * size;
                    // normals
                    _vertices[vertexPointer * stride + 3] = 0;
                    _vertices[vertexPointer * stride + 4] = 1;
                    _vertices[vertexPointer * stride + 5] = 0;
                    // texcoords
                    _vertices[vertexPointer * stride + 6] = (float)j / ((float)VertexCount - 1);
                    _vertices[vertexPointer * stride + 7] = (float)i / ((float)VertexCount - 1);

                    vertexPointer++;
                }
            }

            // generate indices
            _useEBO = true;
            _indices = new int[6 * (VertexCount - 1) * (VertexCount - 1)];
            int indicesPointer = 0;
            for (int i = 0; i < VertexCount - 1; i++)
            {
                for (int j = 0; j < VertexCount - 1; j++)
                {
                    int topLeft = (i * VertexCount) + j;
                    int topRight = topLeft + 1;
                    int bottomLeft = ((i + 1) * VertexCount) + j;
                    int bottomRight = bottomLeft + 1;
                    _indices[indicesPointer++] = topLeft;
                    _indices[indicesPointer++] = bottomLeft;
                    _indices[indicesPointer++] = topRight;
                    _indices[indicesPointer++] = topRight;
                    _indices[indicesPointer++] = bottomLeft;
                    _indices[indicesPointer++] = bottomRight;
                }
            }
        }
    }
}
