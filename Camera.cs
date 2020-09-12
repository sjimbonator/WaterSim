using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace WaterSim
{
    class Camera
    {
        private Vector3 cameraFront = new Vector3(0, 0, -1);
        private Vector3 cameraUp = new Vector3(0, 1, 0);

        private float sensitivity;
        private float speed;

        private int lastX;
        private int lastY;

        private float yaw = -90.0f;
        private float pitch = 0f;
        public Matrix4 ViewMatrix { get; private set; }
        public Vector3 Position { get; private set; }

        public Camera(Vector3 Position, float sensitivity, float speed, int screenWidth = 800, int screenHeight = 600)
        {
            this.Position = Position;
            this.sensitivity = sensitivity;
            this.speed = speed;
            lastX = screenWidth / 2;
            lastY = screenHeight / 2;
        }

        /// <summary>
        /// Updates the `ViewMatrix` based on mouse and keyboard input. To see the result you still have to pass the updated `ViewMatrix` property to the vertex shader.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="mInput"></param>
        /// <param name="deltaTime"></param>
        public void Update(float deltaTime)
        {
            KeyboardState kInput = Keyboard.GetState();
            MouseState mInput = Mouse.GetState();

            float xOffset = mInput.X - lastX;
            float yOffset = mInput.Y - lastY;
            lastX = mInput.X;
            lastY = mInput.Y;

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
            direction.Y = -(float)Math.Sin(MathHelper.DegreesToRadians(pitch));
            direction.Z = (float)Math.Sin(MathHelper.DegreesToRadians(yaw)) * (float)Math.Cos(MathHelper.DegreesToRadians(pitch));
            cameraFront = Vector3.Normalize(direction);

            float adjustedSpeed = speed * deltaTime;
            if (kInput.IsKeyDown(Key.W)) Position += adjustedSpeed * cameraFront;
            if (kInput.IsKeyDown(Key.A)) Position -= Vector3.Normalize(Vector3.Cross(cameraFront, cameraUp)) * adjustedSpeed;
            if (kInput.IsKeyDown(Key.S)) Position -= adjustedSpeed * cameraFront;
            if (kInput.IsKeyDown(Key.D)) Position += Vector3.Normalize(Vector3.Cross(cameraFront, cameraUp)) * adjustedSpeed;

            ViewMatrix = Matrix4.LookAt(Position, Position + cameraFront, cameraUp);
        }

        public void LookAt(Vector3 point)
        {
            throw new Exception("This method has not been implemented yet");
        }

        public void MoveTo(Vector3 location)
        {
            throw new Exception("This method has not been implemented yet");
        }

    }
}
