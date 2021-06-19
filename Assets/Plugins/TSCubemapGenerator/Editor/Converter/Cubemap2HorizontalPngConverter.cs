using UnityEngine;

namespace TSCubemapGenerator
{
    public class Cubemap2HorizontalPngConverter : ICubemapConverter
    {
        public string FileExtension => "png";

        public byte[] ConvertFrom(Cubemap cubemap)
        {
            int width = cubemap.width;
            int height = cubemap.height;

            Texture2D texture = new Texture2D(
                width * 6, height,
                TextureFormat.RGB24, false);

            var faces = new CubemapFace[] {
                CubemapFace.PositiveX,
                CubemapFace.NegativeX,
                CubemapFace.PositiveY,
                CubemapFace.NegativeY,
                CubemapFace.PositiveZ,
                CubemapFace.NegativeZ,
            };

            int x = 0;
            foreach (var face in faces)
            {
                texture.SetPixels(x, 0, width, height, cubemap, face);
                x += width;
            }
            texture.Apply();

            return ImageConversion.EncodeToPNG(texture);
        }
    }
}
