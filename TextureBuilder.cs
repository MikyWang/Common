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
        public static Texture2D BuildTexture(
            Color[] pixels,
            int length,
            TextureWrapMode textureWrapMode = TextureWrapMode.Clamp,
            FilterMode filterMode = FilterMode.Bilinear)
        {
            var texture2D = new Texture2D(length, length)
            {
                wrapMode = textureWrapMode,
                filterMode = filterMode
            };

            texture2D.SetPixels(pixels);
            texture2D.Apply();

            return texture2D;
        }

        public static void SaveTexture2DArrayToAsset(Texture2DArray array, string fileName)
        {
            var path = $"Assets/Milkspun/ChunkTerrain/Textures/{fileName}.asset";
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.CreateAsset(array, path);
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }

        public static async Task SaveTextureToAssetAsync(Texture2D texture2D, string fileName)
        {
            var path = $"Assets/Milkspun/ChunkTerrain/Textures/{fileName}.png";
            var bytes = texture2D.EncodeToPNG();
            await using var fileStream = File.Open(path, FileMode.Create);
            await fileStream.WriteAsync(bytes);

            #if UNITY_EDITOR
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            if (AssetImporter.GetAtPath(path) is not TextureImporter textureIm) return;
            textureIm.isReadable = true;
            textureIm.anisoLevel = 9;
            textureIm.mipmapEnabled = false;
            textureIm.wrapMode = TextureWrapMode.Clamp;
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
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
            return BuildTexture(pixels, width);
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
            var length = (int)Mathf.Sqrt(pixels.Length);
            var currentTexture = BuildTexture(pixels, length);

            return GetPartTextureFromTexture2D(currentTexture, size, index);
        }
    }
}
