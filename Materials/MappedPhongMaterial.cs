using System;
using System.Collections.Generic;
using System.Text;

namespace WaterSim.Materials
{
    class MappedPhongMaterial : Material
    {
        public Texture DiffuseMap { get; private set; }
        public Texture SpecularMap { get; private set; }
        public float Shininess { get; private set; }

        public MappedPhongMaterial(Texture diffuseMap, Texture specularMap, float shininess) : base(new Shader("Content/shaders/cube/shader.vert", "Content/shaders/cube/shader.frag"))
        {
            DiffuseMap = diffuseMap;
            SpecularMap = specularMap;
            Shininess = shininess;
        }

        public override void Apply()
        {
            _shader.Use();

            _shader.SetUniform("viewPos", Game.CameraPosition); //TODO: find out where to best put this

            ApplyMatrices();
            ApplyLights();

            _shader.SetUniform("material.diffuse", 0);
            _shader.SetUniform("material.specular", 1);
            _shader.SetUniform("material.shininess", Shininess);
        }
    }
}
