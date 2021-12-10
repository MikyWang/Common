using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace MilkSpun.Common
{
    public static class TextureBuilder
    {

        /// <summary>
        /// 根据颜色数组创建一个纹理对象
        /// </summary>
        /// <param name="pixels">颜色像素数组</param>
        /// <param name="textureWrapMode">纹理包裹模式</param>
        /// <param name="filterMode">过滤模式</param>
        /// <returns>生成的纹理对象</returns>
        public static Texture2D BuildTexture(
            Color[] pixels,
            TextureWrapMode textureWrapMode = TextureWrapMode.Clamp,
            FilterMode filterMode = FilterMode.Bilinear)
        {
            var width = (int)Mathf.Sqrt(pixels.Length);
            var texture2D = new Texture2D(width, width)
            {
                wrapMode = textureWrapMode,
                filterMode = filterMode
            };

            texture2D.SetPixels(pixels);
            texture2D.Apply();

            return texture2D;
        }

        /// <summary>
        /// 保存texture2DArray到asset文件中
        /// </summary>
        /// <param name="array">texture2DArray</param>
        /// <param name="fileName">要保存的文件名（不含后缀）</param>
        /// <param name="path">保存的文件路径</param>
        public static void SaveTexture2DArrayToAsset(
            Texture2DArray array,
            string fileName,
            string path = "Assets/Milkspun/ChunkTerrain/Textures")
        {
            var file = $"{path}/{fileName}.asset";
            AssetDatabase.DeleteAsset(file);
            AssetDatabase.CreateAsset(array, file);
            AssetDatabase.ImportAsset(file, ImportAssetOptions.ForceUpdate);
        }

        /// <summary>
        /// 保存纹理对象到png图片
        /// </summary>
        /// <param name="texture2D">要保存的纹理对象</param>
        /// <param name="fileName">文件名（不含后缀）</param>
        /// <param name="path">文件路径</param>
        public static async Task SaveTextureToAssetAsync(
            Texture2D texture2D,
            string fileName,
            string path = "Assets/Milkspun/ChunkTerrain/Textures")
        {
            var file = $"{path}/{fileName}.png";
            var bytes = texture2D.EncodeToPNG();
            await using var fileStream = File.Open(file, FileMode.Create);
            await fileStream.WriteAsync(bytes);

            #if UNITY_EDITOR
            AssetDatabase.ImportAsset(file, ImportAssetOptions.ForceUpdate);
            if (AssetImporter.GetAtPath(file) is not TextureImporter textureIm) return;
            textureIm.isReadable = true;
            textureIm.anisoLevel = 9;
            textureIm.mipmapEnabled = false;
            textureIm.wrapMode = TextureWrapMode.Clamp;
            AssetDatabase.ImportAsset(file, ImportAssetOptions.ForceUpdate);
            #endif
        }

        /// <summary>
        /// 从Texture2D中获取指定块的纹理
        /// </summary>
        /// <param name="sourceTexture">源纹理图</param>
        /// <param name="size">纹理分成的块数,如4x4 则size为16</param>
        /// <param name="index">从左往右，从上往下，从0计数的索引值</param>
        /// <returns>局部纹理图</returns>
        public static Texture2D GetPartTextureFromTexture2D(
            Texture2D sourceTexture,
            int size,
            int index)
        {
            var sourceWidth = (int)Mathf.Sqrt(sourceTexture.GetPixels().Length);
            var sizeX = (int)Mathf.Sqrt(size);
            var width = sourceWidth / sizeX;
            var pixels = new Color[width * width];

            for (int i = 0, z = sourceWidth - ((index + sizeX) / sizeX * width);
                 z < sourceWidth - index / sizeX * width;
                 z++)
            {
                for (var x = (index % sizeX) * width;
                     x < ((index + 1) % sizeX) * width;
                     x++, i++)
                {
                    pixels[i] = sourceTexture.GetPixel(x, z);
                }
            }
            return BuildTexture(pixels);
        }

        /// <summary>
        /// 从Texture2DArray中获取指定页纹理的局部纹理
        /// </summary>
        /// <param name="array">纹理数组</param>
        /// <param name="page">纹理块所在页数</param>
        /// <param name="size">纹理分成的块数,如4x4 则size为16</param>
        /// <param name="index">从左往右，从上往下，从0计数的索引值</param>
        /// <returns>局部纹理图</returns>
        public static Texture2D GetPartTextureFromTexture2DArray(
            Texture2DArray array,
            int page,
            int size,
            int index)
        {
            var pixels = array.GetPixels(page);
            var currentTexture = BuildTexture(pixels);

            return GetPartTextureFromTexture2D(currentTexture, size, index);
        }
    }
}
