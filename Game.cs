using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using WaterSim.Lights;

namespace WaterSim
{
    class Game : GameWindow
    {
        private readonly Vector3[] cubePositions =
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

        private readonly Vector3[] pointLightPositions =
        {
            new Vector3(0.7f, 0.2f, 2.0f),
            new Vector3(2.3f, -3.3f, -4.0f),
            new Vector3(-4.0f, 2.0f, -12.0f),
            new Vector3(0.0f, 0.0f, -3.0f)
        };

        private Light[] lights;

        private int DiffuseMap;
        private int SpecularMap;
        private Shader shader;
        private Shader lightCubeShader;
        private Camera camera;

        private double time;
        private float deltaTime = 0.0f;
        private float lastFrame = 0.0f;

        private Matrix4 projection;

        private Plane testPlane;
        private Cube container;
        private Cube lightCube;

        private Dictionary<String, dynamic> containerUniforms;
        private Dictionary<String, dynamic> lightCubeUniforms;

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

            DiffuseMap = ContentPipe.LoadTexture("container2.png", TextureUnit.Texture0);
            SpecularMap = ContentPipe.LoadTexture("container2_specular.png", TextureUnit.Texture1);

            shader = new Shader("shaders/cube/shader.vert", "shaders/cube/shader.frag");
            shader.Use();
            lightCubeShader = new Shader("shaders/light/shader.vert", "shaders/light/shader.frag");
            lightCubeShader.Use();

            camera = new Camera(new Vector3(10, 10, 10), 0.1f, 5f);
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), Width / (float)Height, 0.1f, 100.0f);

            testPlane = new Plane(lightCubeShader, new Dictionary<String, dynamic>());
            container = new Cube(shader, new Dictionary<String, dynamic>());
            lightCube = new Cube(lightCubeShader, new Dictionary<String, dynamic>());

            lights = new Light[] {
                new DirectionalLight(new Vector3(-0.2f, -1.0f, -0.3f)),
                new PointLight( pointLightPositions[0]),
                new PointLight( pointLightPositions[1]),
                new PointLight( pointLightPositions[2]),
                new PointLight( pointLightPositions[3]),
                new SpotLight(camera)
            };

            containerUniforms = new Dictionary<string, dynamic>();
            containerUniforms["material.diffuse"] = 0;
            containerUniforms["material.specular"] = 1;
            containerUniforms["material.shininess"] = 32f;

            containerUniforms["viewPos"] = camera.Position;
            containerUniforms["view"] = camera.ViewMatrix;
            containerUniforms["projection"] = projection;
            containerUniforms["model"] = Matrix4.Identity;
            container.Uniforms = containerUniforms;

            lightCubeUniforms = new Dictionary<string, dynamic>();
            lightCubeUniforms["view"] = camera.ViewMatrix;
            lightCubeUniforms["projection"] = projection;
            lightCubeUniforms["model"] = Matrix4.Identity;
            lightCubeUniforms["lightColor"] = new Vector3(1.0f, 1.0f, 1.0f);
            lightCube.Uniforms = lightCubeUniforms;

            base.OnLoad(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line); //Wireframe Mode

            var planeUniforms = new Dictionary<String, dynamic>();
            var planeModel = Matrix4.Identity;
            planeModel *= Matrix4.CreateTranslation(new Vector3(-100, -10, -100));

            planeUniforms["model"] = planeModel;
            planeUniforms["view"] = camera.ViewMatrix;
            planeUniforms["projection"] = projection;
            planeUniforms["lightColor"] = new Vector3(1.0f, 0.0f, 1.0f);
            testPlane.Uniforms = planeUniforms;
            testPlane.Draw();

            containerUniforms["view"] = camera.ViewMatrix;
            containerUniforms["spotLight.position"] = camera.Position;
            containerUniforms["spotLight.direction"] = camera.CameraFront;
            for (int i = 0; i < cubePositions.Length; i++)
            {
                float angle = 20.0f * i;

                var model = Matrix4.Identity;
                model *= Matrix4.CreateFromAxisAngle(new Vector3(1.0f, 0.3f, 0.5f), MathHelper.DegreesToRadians(angle));
                model *= Matrix4.CreateTranslation(cubePositions[i]);
                containerUniforms["model"] = model;
                container.Uniforms = containerUniforms;

                container.Draw();
            }

            lightCubeUniforms["view"] = camera.ViewMatrix;
            for (int i = 0; i < pointLightPositions.Length; i++)
            {
                var lightCubeModel = Matrix4.Identity;
                lightCubeModel *= Matrix4.CreateScale(0.2f);
                lightCubeModel *= Matrix4.CreateTranslation(pointLightPositions[i]);
                lightCubeUniforms["model"] = lightCubeModel;
                lightCube.Uniforms = lightCubeUniforms;

                lightCube.Draw();
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
            shader.Dispose();

            base.OnUnload(e);
        }
    }
}
