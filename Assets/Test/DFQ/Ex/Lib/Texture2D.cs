using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Core.Lib
{
    public static class Texture2D
    {

        /// <summary>
        ///     将rgb数组转换为Bitmap
        /// </summary> bitmap的内存顺序是bgra
        /// <param name="data"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static UnityEngine.Texture2D FromArray(byte[] data, Size size)
        {
            var texture = new UnityEngine.Texture2D(size.Width, size.Height, UnityEngine.TextureFormat.RGBA32, false);

            var len = size.Width * size.Height * 4;
            var bytes = new byte[len];
            var index = 0;
            for (int i = size.Height - 1; i >= 0; i--)
            {
                for (int j = 0; j < size.Width; j++)
                {
                    var startIndex = i * size.Width * 4 + j * 4;
                    bytes[index] = data[startIndex + 2];
                    bytes[index + 1] = data[startIndex + 1];
                    bytes[index + 2] = data[startIndex + 0];
                    bytes[index + 3] = data[startIndex + 3];
                    index += 4;
                }
            }
            texture.LoadRawTextureData(bytes);
            texture.wrapMode = UnityEngine.TextureWrapMode.Clamp;
            texture.filterMode = UnityEngine.FilterMode.Point;
            texture.Apply();
            return texture;
        }

        /// <summary>
        ///     将rgb数组转换为Bitmap
        /// </summary>
        /// <param name="data"></param>
        /// <param name="size"></param>
        /// <param name="bits"></param>
        /// <returns></returns>
        public static UnityEngine.Texture2D FromArray(byte[] data, Size size, ColorBits bits)
        {
            var ms = new MemoryStream(data);
            data = new byte[size.Width * size.Height * 4];
            for (var i = 0; i < data.Length; i += 4)
            {
                Colors.ReadColor(ms, bits, data, i);
            }
            ms.Close();
            return FromArray(data, size);
        }


    }
}