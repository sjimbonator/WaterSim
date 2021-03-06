﻿using OpenTK;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WaterSim
{
    class DirectionalLight : Light
    {
        private Vector3 _direction;
        public Vector3 Direction { get => _direction; set => _direction = value; }

        /// <summary>
        /// Creates a directional light shining in the specified direction. To change the light's colors you have to use the provided setters.
        /// </summary>
        /// <param name="direction"></param>
        public DirectionalLight(Vector3 direction)
        {
            _direction = direction;

            //day
            _ambientColor = new Vector3(184.0f / 1020.0f, 213.0f / 1020.0f, 238.0f / 1020.0f);
            _diffuseColor = new Vector3(255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f);
            _specularColor = new Vector3(230.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f);

            //night
            //_ambientColor = new Vector3(0.05f, 0.05f, 0.05f);
            //_diffuseColor = new Vector3(0.4f, 0.4f, 0.4f);
            //_specularColor = new Vector3(0.5f, 0.5f, 0.5f);
        }

        public override void Apply(Shader shader, String namePrefix = "dirLight.")
        {
            if (namePrefix == "") namePrefix = "dirLight."; //https://stackoverflow.com/questions/8909811/c-sharp-optional-parameters-on-overridden-methods

            base.Apply(shader, namePrefix);
            shader.SetUniform(namePrefix + "direction", _direction);
        }
    }
}
