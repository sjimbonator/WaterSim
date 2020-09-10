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
    class ContentPipe
    {
        public static int LoadTexture(string path, TextureUnit activeTextureUnit)
        {
            if (!File.Exists($"Content/{path}")) throw new FileNotFoundException($"File not found at 'Content/{path}'");

            int id = GL.GenTexture();
            GL.ActiveTexture(activeTextureUnit);
            GL.BindTexture(TextureTarget.Texture2D, id);

            Bitmap bmp = new Bitmap($"Content/{path}");
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            return id;
        }
    }
}
