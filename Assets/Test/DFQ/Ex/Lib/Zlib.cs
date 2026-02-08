using System;
using System.Runtime.InteropServices;
using Codice.CM.Common.Zlib;
using Codice.CM.Common.Zlib.Win;
using Ionic.Zlib;

namespace ExtractorSharp.Core.Lib {
    /// <summary>
    ///     FreeImage图片处理库封装
    /// </summary>
    public static class Zlib {
        /// <summary>
        ///     zlib压缩
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Compress(byte[] data) {
            var size = (int) (data.LongLength * 1.001 + 12); //缓冲长度 
            var target = new byte[size];
            ZLib.Compress(target, ref size, data, data.Length);
            var temp = new byte[size];
            Buffer.BlockCopy(target, 0, temp, 0, size);
            return temp;
        }

        /// <summary>
        ///     zlib解压缩
        /// </summary>
        /// <param name="data"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] data, int size) {
            var target = new byte[size];
            ZLib.Uncompress(target, ref size, data, data.Length);
            return target;
        }

    }
}