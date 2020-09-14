using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace WaterSim
{
    class Game : GameWindow
    {
        private readonly float[] vertices = {
            // positions          // normals           // texture coords
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 0.0f,
             0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 0.0f,

            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f, 1.0f,   0.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  0.0f,  0.0f, 1.0f,   1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f, 1.0f,   1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f, 1.0f,   1.0f, 1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  0.0f, 1.0f,   0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f, 1.0f,   0.0f, 0.0f,

            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  1.0f, 0.0f,
            -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  1.0f, 1.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 0.0f,
            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  1.0f, 0.0f,

             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 0.0f,

            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  1.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  1.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  1.0f, 0.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 0.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 1.0f,

            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  1.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 1.0f
        };

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

        private int VertexBufferObject, VertexArrayObject;
        private int lightVao;

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

            VertexBufferObject = GL.GenBuffer();
            VertexArrayObject = GL.GenVertexArray();

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.BindVertexArray(VertexArrayObject);

            // position attribute
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // normal attribute
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            // texCoord attribute
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
            GL.EnableVertexAttribArray(2);

            lightVao = GL.GenVertexArray();
            GL.BindVertexArray(lightVao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);

            // position attribute
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            DiffuseMap = ContentPipe.LoadTexture("container2.png", TextureUnit.Texture0);
            SpecularMap = ContentPipe.LoadTexture("container2_specular.png", TextureUnit.Texture1);

            shader = new Shader("shaders/cube/shader.vert", "shaders/cube/shader.frag");
            shader.Use();
            lightCubeShader = new Shader("shaders/light/shader.vert", "shaders/light/shader.frag");
            lightCubeShader.Use();

            camera = new Camera(new Vector3(0, 0, 3), 0.1f, 5f);
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), Width / (float)Height, 0.1f, 100.0f);

            var test = new Dictionary<String, dynamic>();
            testPlane = new Plane(lightCubeShader, test);

            base.OnLoad(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line); //Wireframe Mode

            shader.Use();

            shader.SetUniform("material.diffuse", 0);
            shader.SetUniform("material.specular", 1);
            shader.SetUniform("material.shininess", 32f);

            //lightuniforms

            // Directional light
            shader.SetUniform("dirLight.direction", new Vector3(-0.2f, -1.0f, -0.3f));
            shader.SetUniform("dirLight.ambient", new Vector3(0.05f, 0.05f, 0.05f));
            shader.SetUniform("dirLight.diffuse", new Vector3(0.4f, 0.4f, 0.4f));
            shader.SetUniform("dirLight.specular", new Vector3(0.5f, 0.5f, 0.5f));

            // Point lights
            for (int i = 0; i < pointLightPositions.Length; i++)
            {
                shader.SetUniform($"pointLights[{i}].position", pointLightPositions[i]);
                shader.SetUniform($"pointLights[{i}].ambient", new Vector3(0.05f, 0.05f, 0.05f));
                shader.SetUniform($"pointLights[{i}].diffuse", new Vector3(0.8f, 0.8f, 0.8f));
                shader.SetUniform($"pointLights[{i}].specular", new Vector3(1.0f, 1.0f, 1.0f));
                shader.SetUniform($"pointLights[{i}].constant", 1.0f);
                shader.SetUniform($"pointLights[{i}].linear", 0.09f);
                shader.SetUniform($"pointLights[{i}].quadratic", 0.032f);
            }

            // Spot light
            shader.SetUniform("spotLight.position", camera.Position);
            shader.SetUniform("spotLight.direction", camera.CameraFront);
            shader.SetUniform("spotLight.ambient", new Vector3(0.0f, 0.0f, 0.0f));
            shader.SetUniform("spotLight.diffuse", new Vector3(1.0f, 1.0f, 1.0f));
            shader.SetUniform("spotLight.specular", new Vector3(1.0f, 1.0f, 1.0f));
            shader.SetUniform("spotLight.constant", 1.0f);
            shader.SetUniform("spotLight.linear", 0.09f);
            shader.SetUniform("spotLight.quadratic", 0.032f);
            shader.SetUniform("spotLight.cutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f)));
            shader.SetUniform("spotLight.outerCutOff", (float)Math.Cos(MathHelper.DegreesToRadians(17.5f)));

            //lightuniforms

            shader.SetUniform("viewPos", camera.Position);

            shader.SetUniform("view", camera.ViewMatrix);
            shader.SetUniform("projection", projection);
            shader.SetUniform("model", Matrix4.Identity);

            GL.BindVertexArray(VertexArrayObject);
            for (int i = 0; i < cubePositions.Length; i++)
            {
                var model = Matrix4.Identity;

                float angle = 20.0f * i;
                model *= Matrix4.CreateFromAxisAngle(new Vector3(1.0f, 0.3f, 0.5f), MathHelper.DegreesToRadians(angle));
                model *= Matrix4.CreateTranslation(cubePositions[i]);

                //model *= Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(angle));
                //model *= Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(0.3 * angle));
                //model *= Matrix4.CreateRotationZ((float)MathHelper.DegreesToRadians(0.5 * angle));


                shader.SetUniform("model", model);
                GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            }

            lightCubeShader.Use();

            lightCubeShader.SetUniform("view", camera.ViewMatrix);
            lightCubeShader.SetUniform("projection", projection);

            for (int i = 0; i < pointLightPositions.Length; i++)
            {
                var lightCubeModel = Matrix4.Identity;
                lightCubeModel *= Matrix4.CreateScale(0.2f);
                lightCubeModel *= Matrix4.CreateTranslation(pointLightPositions[i]);
                lightCubeShader.SetUniform("model", lightCubeModel);
                lightCubeShader.SetUniform("lightColor", new Vector3(1.0f, 1.0f, 1.0f));

                GL.BindVertexArray(lightVao);
                GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            }

            var test = new Dictionary<String, dynamic>();
            test.Add("model", Matrix4.Identity);
            test.Add("view", camera.ViewMatrix);
            test.Add("projection", projection);
            test.Add("lightColor", new Vector3(1.0f, 1.0f, 1.0f));

            testPlane.Draw();
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
