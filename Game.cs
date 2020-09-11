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
             //positions          //texcoords
            -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
             0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,

            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,

            -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  1.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,

            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,  0.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f
        };

        private int VertexBufferObject, VertexArrayObject;

        private int Texture1;
        private int Texture2;

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

        //camera

        private Vector3 worldspaceUp = new Vector3(0, 1, 0);

        private Vector3 cameraPos = new Vector3(0, 0, 3);
        private Vector3 cameraFront = new Vector3(0, 0, -1);
        private Vector3 cameraUp = new Vector3(0, 1, 0);
        //private Vector3 cameraTarget = new Vector3(0, 0, 0);
        //private Vector3 cameraDirection = Vector3.Normalize(cameraPos - cameraTarget);
        //private Vector3 cameraRight = Vector3.Normalize(Vector3.Cross(worldspaceUp, cameraDirection));
        // private Vector3 cameraUp = Vector3.Cross(cameraDirection, cameraRight);

        private int lastX = 400;
        private int lastY = 300;

        private bool firstMouse = true;

        private float yaw = -90.0f;
        private float pitch = 0f;

        //camera

        private Shader shader;

        private double time;
        private float deltaTime = 0.0f;
        private float lastFrame = 0.0f;

        private Matrix4 view;
        private Matrix4 projection;

        public Game(int width, int height, string title) : base(width, height, GraphicsMode.Default, title) { }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            KeyboardState input = Keyboard.GetState();
            if (input.IsKeyDown(Key.Escape)) Exit();

            base.OnUpdateFrame(e);

        }

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);

            VertexBufferObject = GL.GenBuffer();
            VertexArrayObject = GL.GenVertexArray();

            GL.BindVertexArray(VertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            // position attribute
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // texture attribute
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            Texture1 = ContentPipe.LoadTexture("wall.jpg", TextureUnit.Texture1);
            Texture2 = ContentPipe.LoadTexture("awesomeface.png", TextureUnit.Texture2);

            shader = new Shader("shader.vert", "shader.frag");

            shader.Use();

            shader.SetUniform("texture1", Texture1);
            shader.SetUniform("texture2", Texture2);

            view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), Width / (float)Height, 0.1f, 100.0f);

            base.OnLoad(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            KeyboardState input = Keyboard.GetState();
            MouseState mInput = Mouse.GetState();

            float xOffset = mInput.X - lastX;
            float yOffset = mInput.Y - lastY;
            lastX = mInput.X;
            lastY = mInput.Y;

            const float sensitivity = 0.1f;
            xOffset *= sensitivity;
            yOffset *= sensitivity;

            yaw += xOffset;
            pitch += yOffset;

            if (pitch > 89.0f)
                pitch = 89.0f;
            if (pitch < -89.0f)
                pitch = -89.0f;

            Vector3 direction;
            direction.X = (float)Math.Cos(MathHelper.DegreesToRadians(yaw)) * (float)Math.Cos(MathHelper.DegreesToRadians(pitch));
            direction.Y = (float)Math.Sin(MathHelper.DegreesToRadians(pitch));
            direction.Z = (float)Math.Sin(MathHelper.DegreesToRadians(yaw)) * (float)Math.Cos(MathHelper.DegreesToRadians(pitch));
            cameraFront = Vector3.Normalize(direction);

            time += e.Time;
            deltaTime = (float)time - lastFrame;
            lastFrame = (float)time;

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            // GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line); //Wireframe Mode

            shader.Use();

            float cameraSpeed = (float)2.5 * deltaTime;
            if (input.IsKeyDown(Key.W)) cameraPos += cameraSpeed * cameraFront;
            if (input.IsKeyDown(Key.A)) cameraPos -= Vector3.Normalize(Vector3.Cross(cameraFront, cameraUp)) * cameraSpeed;
            if (input.IsKeyDown(Key.S)) cameraPos -= cameraSpeed * cameraFront;
            if (input.IsKeyDown(Key.D)) cameraPos += Vector3.Normalize(Vector3.Cross(cameraFront, cameraUp)) * cameraSpeed;

            view = Matrix4.LookAt(cameraPos, cameraPos + cameraFront, cameraUp);

            shader.SetUniform("view", view);
            shader.SetUniform("projection", projection);

            GL.BindVertexArray(VertexArrayObject);
            for (int i = 0; i < 10; i++)
            {
                float angle = 20.0f * i;
                var model = Matrix4.Identity;

                model *= Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(angle));
                model *= Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(0.3 * angle));
                model *= Matrix4.CreateRotationZ((float)MathHelper.DegreesToRadians(0.5 * angle));
                model *= Matrix4.CreateTranslation(cubePositions[i]);

                shader.SetUniform("model", model);

                GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            }

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
