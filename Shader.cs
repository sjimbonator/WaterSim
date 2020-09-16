using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.IO;
using System.Text;
using System.Numerics;

namespace WaterSim
{
    class Shader
    {
        int Handle;

        public Shader(string vertexPath, string fragmentPath)
        {
            int VertexShader;
            int FragmentShader;

            string VertexShaderSource;

            using (StreamReader reader = new StreamReader(vertexPath, Encoding.UTF8))
            {
                VertexShaderSource = reader.ReadToEnd();
            }

            string FragmentShaderSource;

            using (StreamReader reader = new StreamReader(fragmentPath, Encoding.UTF8))
            {
                FragmentShaderSource = reader.ReadToEnd();
            }

            VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, VertexShaderSource);

            FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, FragmentShaderSource);

            GL.CompileShader(VertexShader);

            string infoLogVert = GL.GetShaderInfoLog(VertexShader);
            if (infoLogVert != System.String.Empty)
                System.Console.WriteLine(infoLogVert);

            GL.CompileShader(FragmentShader);

            string infoLogFrag = GL.GetShaderInfoLog(FragmentShader);

            if (infoLogFrag != System.String.Empty)
                System.Console.WriteLine(infoLogFrag);

            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, VertexShader);
            GL.AttachShader(Handle, FragmentShader);

            GL.LinkProgram(Handle);

            GL.DetachShader(Handle, VertexShader);
            GL.DetachShader(Handle, FragmentShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(Handle);

                disposedValue = true;
            }
        }

        public void SetUniform(string name, bool value) => GL.Uniform1(GL.GetUniformLocation(Handle, name), Convert.ToInt32(value));

        public void SetUniform(string name, int value) => GL.Uniform1(GL.GetUniformLocation(Handle, name), value);

        public void SetUniform(string name, float value) => GL.Uniform1(GL.GetUniformLocation(Handle, name), value);

        public void SetUniform(string name, OpenTK.Vector2 value) => GL.Uniform2(GL.GetUniformLocation(Handle, name), value);

        public void SetUniform(string name, OpenTK.Vector3 value) => GL.Uniform3(GL.GetUniformLocation(Handle, name), value);

        public void SetUniform(string name, OpenTK.Vector4 value) => GL.Uniform4(GL.GetUniformLocation(Handle, name), value);

        public void SetUniform(string name, OpenTK.Matrix4 value) => GL.UniformMatrix4(GL.GetUniformLocation(Handle, name), true, ref value);

        //TODO: support SetUniform for Uniform Buffer Objects
        //TODO: support SetUniform for the texture class directly instead of via int?

        ~Shader()
        {
            GL.DeleteProgram(Handle);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
