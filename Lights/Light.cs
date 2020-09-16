using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;

namespace WaterSim
{
    abstract class Light
    {
        protected Vector3 _ambientColor;
        protected Vector3 _diffuseColor;
        protected Vector3 _specularColor;

        public Vector3 AmbientColor { get => _ambientColor; set => _ambientColor = value; }
        public Vector3 DiffuseColor { get => _diffuseColor; set => _diffuseColor = value; }
        public Vector3 SpecularColor { get => _specularColor; set => _specularColor = value; }

        public virtual void Apply(Shader shader, String namePrefix = "")
        {
            shader.SetUniform(namePrefix + "ambient", _ambientColor);
            shader.SetUniform(namePrefix + "diffuse", _diffuseColor);
            shader.SetUniform(namePrefix + "specular", _specularColor);
        }
    }
}
