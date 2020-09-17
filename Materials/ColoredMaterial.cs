using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;

namespace WaterSim.Materials
{
    class ColoredMaterial : Material
    {
        public Vector3 Color { get; private set; }

        public ColoredMaterial(Vector3 color) : base(new Shader("Content/shaders/light/shader.vert", "Content/shaders/light/shader.frag"))
        {
            Color = color;
        }

        public override void Apply()
        {
            _shader.Use();

            ApplyMatrices();

            _shader.SetUniform("color", Color);
        }
    }
}
