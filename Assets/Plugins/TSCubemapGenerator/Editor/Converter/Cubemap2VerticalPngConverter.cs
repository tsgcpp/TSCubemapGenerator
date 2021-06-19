using UnityEngine;

namespace TSCubemapGenerator
{
    public class Cubemap2VerticalPngConverter : ICubemapConverter
    {
        public string FileExtension => "png";

        public byte[] ConvertFrom(Cubemap cubemap)
        {
            int width = cubemap.width;
            int height = cubemap.height;

            Texture2D texture = new Texture2D(
                width, height * 6,
                TextureFormat.RGB24, false);

            // SetPixelsが画像の左下から右上に描画していくため、
            // 下から上の順番でコピー
            var faces = new CubemapFace[] {
                CubemapFace.NegativeZ,
                CubemapFace.PositiveZ,
                CubemapFace.NegativeY,
                CubemapFace.PositiveY,
                CubemapFace.NegativeX,
                CubemapFace.PositiveX,
            };

            int y = 0;
            foreach (var face in faces)
            {
                texture.SetPixels(0, y, width, height, cubemap, face);
                y += height;
            }
            texture.Apply();

            return ImageConversion.EncodeToPNG(texture);
        }
    }
}
