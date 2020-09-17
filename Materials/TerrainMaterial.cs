using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;

namespace WaterSim.Materials
{
    class TerrainMaterial : Material
    {


        public TerrainMaterial() : base(new Shader("Content/shaders/terrain/terrainShader.vert", "Content/shaders/terrain/terrainShader.frag"))
        {
        }

        public override void Apply()
        {
            _shader.Use();

            _shader.SetUniform("viewPos", Game.CameraPosition); //TODO: find out where to best put this

            ApplyMatrices();
            ApplyLights();

            _shader.SetUniform("noisiness", Game.Noisiness);
            _shader.SetUniform("heightMod", Game.HeightMod);

            _shader.SetUniform("material.ambient", new Vector3(0.48f / 20, 0.51f / 10, 0.31f / 20));
            _shader.SetUniform("material.diffuse", new Vector3(0.48f, 0.51f, 0.31f));
            _shader.SetUniform("material.specular", new Vector3(0.48f, 0.51f, 0.31f));
            _shader.SetUniform("material.shininess", 0.078125f * 128f);
        }
    }
}
