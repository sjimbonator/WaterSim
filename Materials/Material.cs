using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;

namespace WaterSim.Materials
{
    abstract class Material
    {
        protected Shader _shader;
        protected Matrix4 _modelMatrix = Matrix4.Identity;

        public Matrix4 ModelMatrix { get => _modelMatrix; set => _modelMatrix = value; }

        public Material(Shader shader)
        {
            _shader = shader;
        }

        public abstract void Apply();

        protected void ApplyLights()
        {
            var pointLightCount = 0;
            foreach (Light light in Game.Lights)
            {
                if (light.GetType() == typeof(PointLight))
                {
                    light.Apply(_shader, $"pointLights[{pointLightCount}].");
                    pointLightCount++;
                    continue;
                }
                light.Apply(_shader);
            }
        }

        protected void ApplyMatrices()
        {
            _shader.SetUniform("model", _modelMatrix);
            _shader.SetUniform("view", Game.View);
            _shader.SetUniform("projection", Game.Projection);
        }

        protected void ApplySkyColor()
        {
            _shader.SetUniform("skyColor", Game.SkyColor);
        }
    }
}
