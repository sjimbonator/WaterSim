using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Diagnostics;

namespace WaterSim
{
    class Game : GameWindow
    {
        private readonly float[] vertices = {
             //positions          //normals
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
             0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,

            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  0.0f,  0.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f, 1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f, 1.0f,

            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,

             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,
             0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
             0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,

            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,
             0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,

            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f
        };

        private Vector3[] cubePositions =
{
            new Vector3( 0.0f,  0.0f,  0.0f),
            new Vector3( 2.0f,  5.0f, -15.0f),
            new Vector3(-1.5f, -2.2f, -2.5f),
            new Vector3(-3.8f, -2.0f, -12.3f),
            new Vector3( 2.4f, -0.4f, -3.5f),
            new Vector3(-1.7f,  3.0f, -7.5f),
            new Vector3( 1.3f, -2.0f, -2.5f),
            new Vector3( 1.5f,  2.0f, -2.5f),
            new Vector3( 1.5f,  0.2f, -1.5f),
            new Vector3(-1.3f,  1.0f, -1.5f)
        };

        private Vector3 lightPos = new Vector3(1.2f, 1.0f, 2.0f);

        private int VertexBufferObject, VertexArrayObject;
        private int lightVao;

        private Shader shader;
        private Shader lightCubeShader;
        private Camera camera;

        private double time;
        private float deltaTime = 0.0f;
        private float lastFrame = 0.0f;

        private Matrix4 projection;

        public Game(int width, int height, string title) : base(width, height, GraphicsMode.Default, title) { }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            KeyboardState kInput = Keyboard.GetState();
            if (kInput.IsKeyDown(Key.Escape)) Exit();

            time += e.Time;
            deltaTime = (float)time - lastFrame;
            lastFrame = (float)time;

            camera.Update(deltaTime);

            base.OnUpdateFrame(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);

            VertexBufferObject = GL.GenBuffer();
            VertexArrayObject = GL.GenVertexArray();

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.BindVertexArray(VertexArrayObject);

            // position attribute
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // normal attribute
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            lightVao = GL.GenVertexArray();
            GL.BindVertexArray(lightVao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);

            // position attribute
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            shader = new Shader("shaders/cube/shader.vert", "shaders/cube/shader.frag");
            shader.Use();
            lightCubeShader = new Shader("shaders/light/shader.vert", "shaders/light/shader.frag");
            lightCubeShader.Use();

            camera = new Camera(new Vector3(0, 0, 3), 0.1f, 5f);
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), Width / (float)Height, 0.1f, 100.0f);

            base.OnLoad(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line); //Wireframe Mode

            shader.Use();

            shader.SetUniform("objectColor", new Vector3(1.0f, 0.5f, 0.31f));
            shader.SetUniform("viewPos", camera.Position);
            shader.SetUniform("lightColor", new Vector3(1.0f, 1.0f, 1.0f));
            shader.SetUniform("lightPos", lightPos);

            shader.SetUniform("view", camera.ViewMatrix);
            shader.SetUniform("projection", projection);

            for (int i = 0; i < 10; i++)
            {
                float angle = 20.0f * i;
                var model = Matrix4.Identity;

                //model *= Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(angle * time));
                //model *= Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(0.3 * angle * time));
                //model *= Matrix4.CreateRotationZ((float)MathHelper.DegreesToRadians(0.5 * angle * time));
                model *= Matrix4.CreateTranslation(cubePositions[i]);

                shader.SetUniform("model", model);

                GL.BindVertexArray(VertexArrayObject);
                GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            }

            lightCubeShader.Use();

            var lightCubeModel = Matrix4.Identity;
            lightCubeModel *= Matrix4.CreateScale(0.2f);
            lightCubeModel *= Matrix4.CreateTranslation(lightPos);

            lightCubeShader.SetUniform("view", camera.ViewMatrix);
            lightCubeShader.SetUniform("projection", projection);
            lightCubeShader.SetUniform("model", lightCubeModel);

            GL.BindVertexArray(lightVao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);


            Context.SwapBuffers();

            base.OnRenderFrame(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            base.OnResize(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(VertexBufferObject);

            shader.Dispose();

            base.OnUnload(e);
        }
    }

}
