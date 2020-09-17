using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;

namespace WaterSim.Materials
{
    class TerrainMaterial : Material
    {
        private Dictionary<String, dynamic> terrainMaterials;

        public TerrainMaterial() : base(new Shader("Content/shaders/terrain/terrainShader.vert", "Content/shaders/terrain/terrainShader.frag"))
        {
            terrainMaterials = new Dictionary<String, dynamic>();

            terrainMaterials["materials[0].ambient"] = new Vector3(201 / 255f, 178 / 255f, 99 / 255f) * 0.05f;
            terrainMaterials["materials[0].diffuse"] = new Vector3(201 / 255f, 178 / 255f, 99 / 255f);
            terrainMaterials["materials[0].specular"] = new Vector3(201 / 255f, 178 / 255f, 99 / 255f) * 0.8f;
            terrainMaterials["materials[0].shininess"] = 0.078125f * 128f;

            terrainMaterials["materials[1].ambient"] = new Vector3(135 / 255f, 184 / 255f, 82 / 255f) * 0.05f;
            terrainMaterials["materials[1].diffuse"] = new Vector3(135 / 255f, 184 / 255f, 82 / 255f);
            terrainMaterials["materials[1].specular"] = new Vector3(135 / 255f, 184 / 255f, 82 / 255f) * 0.8f;
            terrainMaterials["materials[1].shininess"] = 0.078125f * 128f;

            terrainMaterials["materials[2].ambient"] = new Vector3(80 / 255f, 171 / 255f, 93 / 255f) * 0.05f;
            terrainMaterials["materials[2].diffuse"] = new Vector3(80 / 255f, 171 / 255f, 93 / 255f);
            terrainMaterials["materials[2].specular"] = new Vector3(80 / 255f, 171 / 255f, 93 / 255f) * 0.8f;
            terrainMaterials["materials[2].shininess"] = 0.078125f * 128f;

            terrainMaterials["materials[3].ambient"] = new Vector3(120 / 255f, 120 / 255f, 120 / 255f) * 0.05f;
            terrainMaterials["materials[3].diffuse"] = new Vector3(120 / 255f, 120 / 255f, 120 / 255f);
            terrainMaterials["materials[3].specular"] = new Vector3(120 / 255f, 120 / 255f, 120 / 255f) * 1.1f;
            terrainMaterials["materials[3].shininess"] = 0.078125f * 256f;

            terrainMaterials["materials[4].ambient"] = new Vector3(200 / 255f, 200 / 255f, 210 / 255f) * 0.05f;
            terrainMaterials["materials[4].diffuse"] = new Vector3(200 / 255f, 200 / 255f, 210 / 255f);
            terrainMaterials["materials[4].specular"] = new Vector3(200 / 255f, 200 / 255f, 210 / 255f) * 1.1f;
            terrainMaterials["materials[4].shininess"] = 0.078125f * 256f;
        }

        public override void Apply()
        {
            _shader.Use();

            _shader.SetUniform("viewPos", Game.CameraPosition); //TODO: find out where to best put this

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
