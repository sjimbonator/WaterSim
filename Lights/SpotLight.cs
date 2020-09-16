using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;

namespace WaterSim.Lights
{
    class SpotLight : PointLight
    {
        private Vector3 _direction;

        private float _cutOff = (float)Math.Cos(MathHelper.DegreesToRadians(12.5f));
        private float _outerCutOff = (float)Math.Cos(MathHelper.DegreesToRadians(17.5f));

        private Camera _wrappedCamera = null;

        public SpotLight(Vector3 position, Vector3 direction) : base(position)
        {
            _direction = direction;
        }

        public SpotLight(Camera wrappedCamera) : base(wrappedCamera.Position)
        {
            _direction = wrappedCamera.CameraFront;
            _wrappedCamera = wrappedCamera;
        }

        public override void SetUniforms(Shader shader, String namePrefix = "spotLight.")
        {
            if (namePrefix == "") namePrefix = "spotLight."; //https://stackoverflow.com/questions/8909811/c-sharp-optional-parameters-on-overridden-methods

            base.SetUniforms(shader, namePrefix);
            if (_wrappedCamera != null)
            {
                shader.SetUniform(namePrefix + "position", _wrappedCamera.Position);
                shader.SetUniform(namePrefix + "direction", _wrappedCamera.CameraFront);
                _direction = _wrappedCamera.CameraFront;
            }
            else
            {
                shader.SetUniform(namePrefix + "direction", _direction);
            }
            shader.SetUniform(namePrefix + "cutOff", _cutOff);
            shader.SetUniform(namePrefix + "outerCutOff", _outerCutOff);
        }
    }
}
