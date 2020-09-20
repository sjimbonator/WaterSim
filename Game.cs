using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using WaterSim.Lights;
using WaterSim.Materials;

namespace WaterSim
{
    class Game : GameWindow
    {
        private List<Shape> shapes;
        private static List<Shader> shaders;

        private Camera camera;

        private double time;
        private float deltaTime = 0.0f;
        private float lastFrame = 0.0f;

        public static Matrix4 Projection { get; private set; }
        public static Matrix4 View { get; private set; }
        public static Vector3 CameraPosition { get; private set; }
        public static Vector3 CameraDirection { get; private set; }
        public static Vector4 SkyColor { get; private set; }
        public static Light[] Lights { get; private set; }
        public static void AddShader(Shader shader) => shaders.Add(shader);

        public static float Noisiness { get; private set; }
        public static float HeightMod { get; private set; }

        public Game(int width, int height, string title) : base(width, height, GraphicsMode.Default, title) { }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            KeyboardState kInput = Keyboard.GetState();
            if (kInput.IsKeyDown(Key.Escape)) Exit();

            time += e.Time;
            deltaTime = (float)time - lastFrame;
            lastFrame = (float)time;

            camera.Update(deltaTime);
            View = camera.ViewMatrix;
            CameraPosition = camera.Position;
            CameraDirection = camera.CameraFront;

            if (kInput.IsKeyDown(Key.Right)) Noisiness += 0.001f;
            if (kInput.IsKeyDown(Key.Left)) Noisiness -= 0.001f;
            if (kInput.IsKeyDown(Key.Up)) HeightMod += 0.01f;
            if (kInput.IsKeyDown(Key.Down)) HeightMod -= 0.01f;

            if (kInput.IsKeyDown(Key.Space)) Console.WriteLine($"HeightMod = {HeightMod} Noisiness = {Noisiness}");

            base.OnUpdateFrame(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            Noisiness = 0.04f;
            HeightMod = 3f;

            shapes = new List<Shape>();
            shaders = new List<Shader>();

            camera = new Camera(new Vector3(10, 10, 10), 0.1f, 150f);
            Projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), Width / (float)Height, 0.1f, 1000.0f);

            BuildWorldObjects();

            base.OnLoad(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            SkyColor = new Vector4(184.0f / 255.0f, 213.0f / 255.0f, 238.0f / 255.0f, 1.0f);
            GL.ClearColor(SkyColor.X, SkyColor.Y, SkyColor.Z, SkyColor.W);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            if (Keyboard.GetState().IsKeyDown(Key.P)) GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line); //Wireframe Mode
            else { GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill); }

            foreach (Shape shape in shapes) shape.Draw();

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
            foreach (Shader shader in shaders) shader.Dispose();
            base.OnUnload(e);
        }

        private void BuildWorldObjects()
        {
            // uncomment for the learnopengl.com tutorial objects
            Vector3[] cubePositions =
            {
                new Vector3(0.0f, 0.0f, 0.0f),
                new Vector3(2.0f, 5.0f, -15.0f),
                new Vector3(-1.5f, -2.2f, -2.5f),
                new Vector3(-3.8f, -2.0f, -12.3f),
                new Vector3(2.4f, -0.4f, -3.5f),
                new Vector3(-1.7f, 3.0f, -7.5f),
                new Vector3(1.3f, -2.0f, -2.5f),
                new Vector3(1.5f, 2.0f, -2.5f),
                new Vector3(1.5f, 0.2f, -1.5f),
                new Vector3(-1.3f, 1.0f, -1.5f)
            };

            Vector3[] pointLightPositions =
            {
                new Vector3(0.7f, 0.2f, 2.0f),
                new Vector3(2.3f, -3.3f, -4.0f),
                new Vector3(-4.0f, 2.0f, -12.0f),
                new Vector3(0.0f, 0.0f, -3.0f)
            };

            for (int i = 0; i < cubePositions.Length; i++)
            {
                float angle = 20.0f * i;
                var model = Matrix4.Identity;
                model *= Matrix4.CreateFromAxisAngle(new Vector3(1.0f, 0.3f, 0.5f), MathHelper.DegreesToRadians(angle));
                model *= Matrix4.CreateTranslation(cubePositions[i]);
                shapes.Add(new Cube(new MappedPhongMaterial(new Texture("container2.png"), new Texture("container2_specular.png"), 32f), model));
            }

            for (int i = 0; i < pointLightPositions.Length; i++)
            {
                var model = Matrix4.Identity;
                model *= Matrix4.CreateScale(0.2f);
                model *= Matrix4.CreateTranslation(pointLightPositions[i]);
                shapes.Add(new Cube(new ColoredMaterial(new Vector3(1.0f, 1.0f, 1.0f)), model));
            }

            var planeModel = Matrix4.Identity * Matrix4.CreateTranslation(new Vector3(-800f, -70f, -800f));
            shapes.Add(new Plane(new TerrainMaterial(), planeModel, 1600, 512));

            Lights = new Light[] {
                new DirectionalLight(new Vector3(-0.2f, -1.0f, -0.4f)),
                new PointLight( pointLightPositions[0]),
                new PointLight( pointLightPositions[1]),
                new PointLight( pointLightPositions[2]),
                new PointLight( pointLightPositions[3]),
                new SpotLight(camera)
            };
        }
    }

}

