using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;

namespace WaterSim.Materials
{
    class PhongMaterial : Material
    {
        public Vector3 AmbientColor { get; private set; }
        public Vector3 DiffuseColor { get; private set; }
        public Vector3 SpecularColor { get; private set; }
        public float Shininess { get; private set; }

        private PhongMaterial(Vector3 ambient, Vector3 diffuse, Vector3 specular, float shininess) : base(new Shader("NOT YET", "IMPLEMENTED"))
        {
            AmbientColor = ambient;
            DiffuseColor = diffuse;
            SpecularColor = specular;
            Shininess = shininess;
        }

        public override void Apply()
        {
            _shader.Use();

            _shader.SetUniform("viewPos", Game.CameraPosition); //TODO: find out where to best put this

            ApplyMatrices();
            ApplyLights();

            _shader.SetUniform("material.ambient", AmbientColor);
            _shader.SetUniform("material.diffuse", DiffuseColor);
            _shader.SetUniform("material.specular", SpecularColor);
            _shader.SetUniform("material.shininess", Shininess);
        }
    }
}
