using System;
using System.Collections.Generic;
using System.Text;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace WaterSim
{
    class Texture
    {
        private static int _textureUnitCount = 0;

        private int _textureUnit;
        private int _id;

        public int Id { get => _id; private set => _id = value; }
        public int Unit { get => _textureUnit; private set => _textureUnit = value; }

        public Texture(string path)
        {
            if (!File.Exists($"Content/{path}")) throw new FileNotFoundException($"File not found at 'Content/{path}'");

            _textureUnit = 33984 + _textureUnitCount; //The OpenTK TextureUnit enum starts with 33984. So TextureUnit.Texture0's int value is 33984, TextureUnit.Texture1's int value is 33985 etc.
            _textureUnitCount++;

            _id = GL.GenTexture();
            GL.ActiveTexture((TextureUnit)_textureUnit);
            GL.BindTexture(TextureTarget.Texture2D, _id);

            Bitmap bmp = new Bitmap($"Content/{path}");
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }
    }
}
