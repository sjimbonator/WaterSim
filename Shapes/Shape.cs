using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using System.Text;
using WaterSim.Materials;
using OpenTK;

namespace WaterSim
{
    abstract class Shape
    {
        protected Material _material;

        protected float[] _vertices;
        protected int[] _indices;

        protected int VAO, VBO, EBO;
        protected bool _useEBO;
        public Matrix4 ModelMatrix { get => _material.ModelMatrix; set => _material.ModelMatrix = value; }

        public Shape(Material material)
        {
            _material = material;
        }

        public Shape(Material material, Matrix4 modelMatrix)
        {
            _material = material;
            ModelMatrix = modelMatrix;
        }

        public void Draw()
        {
            //apply material
            _material.Apply();

            // draw mesh
            GL.BindVertexArray(VAO);
            if (_useEBO) GL.DrawElements(BeginMode.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
            else GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices.Length);
            GL.BindVertexArray(0);
        }

        protected void Setup()
        {
            VAO = GL.GenVertexArray();
            VBO = GL.GenBuffer();
            if (_useEBO) EBO = GL.GenBuffer();

            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            if (_useEBO)
            {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
                GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(int), _indices, BufferUsageHint.StaticDraw);
            }

            // position attribute
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // normal attribute
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            // texCoord attribute
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
            GL.EnableVertexAttribArray(2);

            // unbind VAO
            GL.BindVertexArray(0);
        }
    }
}
