using UnityEngine;

namespace CubemapFileGenerator
{
    public static class Texture2DExtensions
    {
        /// <summary>
        /// Texture2Dの向きに合わせてCubemapの指定されたfaceのピクセルにてSetPixelsを実施
        /// 
        /// 制限事項
        /// blockWidth, blockHeightはTexture2DおよびCubemapのwidth, height以下の値で指定すること
        /// </summary>
        public static void SetPixels(this Texture2D texture,
            int x, int y, int blockWidth, int blockHeight,
            Cubemap cubemap, CubemapFace face)
        {
            Color[] pixels = cubemap.GetPixels(face);

            // Cubemap.GetPixelsは右上から左下の順番のため、
            // SetPixelsに左下から右上の順番に合わせて反転させる
            System.Array.Reverse(pixels);
            for (int i = 0; i < blockWidth; ++i) {
                System.Array.Reverse(pixels, i * blockHeight, blockWidth);
            }

            texture.SetPixels(x, y, blockWidth, blockHeight, pixels);
        }
    }
}
