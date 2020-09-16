using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;

namespace WaterSim
{
    class PointLight : Light
    {
        protected Vector3 _position;

        protected float _constant = 1.0f;
        protected float _linear = 0.09f;
        protected float _quadratic = 0.032f;

        public Vector3 Position { get => _position; set => _position = value; }

        public float Constant { get => _constant; set => _constant = value; }
        public float Linear { get => _linear; set => _linear = value; }
        public float Quadratic { get => _quadratic; set => _quadratic = value; }

        /// <summary>
        /// Creates a point light. To change the light's color and/or attenuation config you have to use the provided setters.
        /// </summary>
        /// <param name="direction"></param>
        public PointLight(Vector3 position)
        {
            _ambientColor = new Vector3(0.05f, 0.05f, 0.05f);
            _diffuseColor = new Vector3(0.8f, 0.8f, 0.8f);
            _specularColor = new Vector3(1.0f, 1.0f, 1.0f);

            _position = position;
        }

        public override void SetUniforms(Shader shader, String namePrefix = "pointLights[0].")
        {
            if (namePrefix == "") namePrefix = "pointLights[0]."; //https://stackoverflow.com/questions/8909811/c-sharp-optional-parameters-on-overridden-methods

            base.SetUniforms(shader, namePrefix);
            shader.SetUniform(namePrefix + "position", _position);

            shader.SetUniform(namePrefix + "constant", _constant);
            shader.SetUniform(namePrefix + "linear", _linear);
            shader.SetUniform(namePrefix + "quadratic", _quadratic);
        }
    }
}
