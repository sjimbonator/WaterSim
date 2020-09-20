using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;

namespace WaterSim.Materials
{
    class TerrainMaterial : Material
    {
        private Dictionary<String, dynamic> terrainMaterials;

        public TerrainMaterial() : base(new Shader("Content/shaders/terrain/terrainShader.vert", "Content/shaders/terrain/terrainShader.frag", "Content/shaders/terrain/terrainShader.geom"))
        {
            terrainMaterials = new Dictionary<String, dynamic>();

            terrainMaterials["materials[0].ambient"] = new Vector3(74 / 255f, 144 / 255f, 226 / 255f) * 0.05f;
            terrainMaterials["materials[0].diffuse"] = new Vector3(74 / 255f, 144 / 255f, 226 / 255f);
            terrainMaterials["materials[0].specular"] = new Vector3(74 / 255f, 144 / 255f, 226 / 255f) * 0.8f;
            terrainMaterials["materials[0].shininess"] = 0.078125f * 128f;
            terrainMaterials["materials[0].lerpTreshold"] = 0.99f;

            terrainMaterials["materials[1].ambient"] = new Vector3(248 / 255f, 231 / 255f, 28 / 255f) * 0.05f;
            terrainMaterials["materials[1].diffuse"] = new Vector3(248 / 255f, 231 / 255f, 28 / 255f);
            terrainMaterials["materials[1].specular"] = new Vector3(248 / 255f, 231 / 255f, 28 / 255f) * 0.8f;
            terrainMaterials["materials[1].shininess"] = 0.078125f * 128f;
            terrainMaterials["materials[1].lerpTreshold"] = 1.00f;

            terrainMaterials["materials[2].ambient"] = new Vector3(139 / 255f, 87 / 255f, 42 / 255f) * 0.05f;
            terrainMaterials["materials[2].diffuse"] = new Vector3(139 / 255f, 87 / 255f, 42 / 255f);
            terrainMaterials["materials[2].specular"] = new Vector3(139 / 255f, 87 / 255f, 42 / 255f) * 0.8f;
            terrainMaterials["materials[2].shininess"] = 0.078125f * 128f;
            terrainMaterials["materials[2].lerpTreshold"] = 1.05f;

            terrainMaterials["materials[3].ambient"] = new Vector3(65 / 255f, 117 / 255f, 5 / 255f) * 0.05f;
            terrainMaterials["materials[3].diffuse"] = new Vector3(65 / 255f, 117 / 255f, 5 / 255f);
            terrainMaterials["materials[3].specular"] = new Vector3(65 / 255f, 117 / 255f, 5 / 255f) * 0.8f;
            terrainMaterials["materials[3].shininess"] = 0.078125f * 128f;
            terrainMaterials["materials[3].lerpTreshold"] = 1.23f;

            terrainMaterials["materials[4].ambient"] = new Vector3(50 / 255f, 50 / 255f, 50 / 255f) * 0.05f;
            terrainMaterials["materials[4].diffuse"] = new Vector3(50 / 255f, 50 / 255f, 50 / 255f);
            terrainMaterials["materials[4].specular"] = new Vector3(50 / 255f, 50 / 255f, 50 / 255f) * 1.1f;
            terrainMaterials["materials[4].shininess"] = 0.078125f * 256f;
            terrainMaterials["materials[4].lerpTreshold"] = 1.65f;

            terrainMaterials["materials[5].ambient"] = new Vector3(200 / 255f, 200 / 255f, 210 / 255f) * 0.05f;
            terrainMaterials["materials[5].diffuse"] = new Vector3(155 / 255f, 155 / 255f, 155 / 255f);
            terrainMaterials["materials[5].specular"] = new Vector3(200 / 255f, 200 / 255f, 210 / 255f) * 1.1f;
            terrainMaterials["materials[5].shininess"] = 0.078125f * 256f;
            terrainMaterials["materials[5].lerpTreshold"] = 1.99f;
        }

        public override void Apply()
        {
            _shader.Use();

            _shader.SetUniform("viewPos", Game.CameraPosition); //TODO: find out where to best put this

            ApplySkyColor();
            ApplyMatrices();
            ApplyLights();
            ApplyMaterialValues();

            _shader.SetUniform("noisiness", Game.Noisiness);
            _shader.SetUniform("heightMod", Game.HeightMod);
        }

        private void ApplyMaterialValues()
        {
            foreach (KeyValuePair<String, dynamic> uniform in terrainMaterials)
            {
                _shader.SetUniform(uniform.Key, uniform.Value);
            }
        }
    }
}
