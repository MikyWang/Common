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
        public static Texture2D GenerateGradientTexture2D(in Texture2D outSideTexture2D, in Texture2D insideTexture2D, float threshold = 0.1f)
        {
            var outSidePixels = outSideTexture2D.GetPixels();
            var insidePixels = insideTexture2D.GetPixels();
            var length = Mathf.Min(outSidePixels.Length, insidePixels.Length);
            var sqrtLength = (int)Mathf.Sqrt(length);
            var outPixels = new Color[length];
            var minValue = threshold * sqrtLength;
            var maxValue = (1 - threshold) * sqrtLength;

            for (int x = 0, i = 0; x < sqrtLength; x++)
            {
                for (var z = 0; z < sqrtLength; z++, i++)
                {
                    var xPer = x < minValue
                        ? x / minValue
                        // : x > maxValue
                        //     ? (sqrtLength - x) / (sqrtLength - maxValue)
                        : 1;
                    var zPer = z < minValue
                        ? z / minValue
                        // : z > maxValue
                        //     ? (sqrtLength - z) / (sqrtLength - maxValue)
                        : 1;

                    outPixels[i] = Color.Lerp(outSidePixels[i], insidePixels[i], Mathf.Min(xPer, 1));
                }
            }

            var texture2D = new Texture2D(sqrtLength, sqrtLength)
            {
                wrapMode = TextureWrapMode.Repeat,
                filterMode = FilterMode.Bilinear
            };

            texture2D.SetPixels(outPixels);
            texture2D.Apply();

            return texture2D;
        }

        public static Texture2D BuildTexture(Color[] pixels, int length, TextureWrapMode textureWrapMode = TextureWrapMode.Repeat, FilterMode filterMode = FilterMode.Bilinear)
        {
            var texture2D = new Texture2D(length, length)
            {
                wrapMode = TextureWrapMode.Clamp,
                filterMode = FilterMode.Bilinear
            };

            texture2D.SetPixels(pixels);
            texture2D.Apply();

            return texture2D;
        }

        public static void SaveTexture2DArrayToAssetAsync(Texture2DArray array, string fileName)
        {
            var path = $"Assets/Milkspun/Terrain/Textures/{fileName}.asset";
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.CreateAsset(array, path);
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }

        public static async Task SaveTextureToAsset(Texture2D texture2D, string fileName)
        {
            var path = $"Assets/Milkspun/Terrain/Textures/{fileName}.png";
            var bytes = texture2D.EncodeToPNG();
            await using var fileStream = File.Open(path, FileMode.Create);
            await fileStream.WriteAsync(bytes);

            #if UNITY_EDITOR
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            var textureIm = AssetImporter.GetAtPath(path) as TextureImporter;
            if (textureIm is null) return;
            textureIm.isReadable = true;
            textureIm.anisoLevel = 9;
            textureIm.mipmapEnabled = false;
            textureIm.wrapMode = TextureWrapMode.Clamp;
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            #endif
        }

        // public static Texture2D BuildNoiseTexture(float[,] noiseMap, TerrainType[] terrainTypes)
        // {
        //     var pixels = new Color[noiseMap.Length];
        //     int pixelLength = noiseMap.GetLength(0);
        //
        //     for (int x = 0, pi = 0; x < pixelLength; x++)
        //     {
        //         for (int z = 0; z < pixelLength; z++, pi++)
        //         {
        //             // pixels[pi] = Color.Lerp(Color.black, Color.white, noiseMap[x, z]);
        //             for (int i = 0; i < terrainTypes.Length; i++)
        //             {
        //                 if (noiseMap[x, z] < terrainTypes[i].threshold)
        //                 {
        //                     if (i == terrainTypes.Length - 1)
        //                     {
        //                         pixels[pi] = terrainTypes[i].baseTexture.GetPixel(x, z);
        //                         break;
        //                     }
        //                     var t = terrainTypes[i].threshold - noiseMap[x, z];
        //                     pixels[pi] = Color.Lerp(terrainTypes[i].baseTexture.GetPixel(x, z)
        //                         , terrainTypes[i + 1].baseTexture.GetPixel(x, z)
        //                         , noiseMap[x, z]);
        //                     break;
        //                 }
        //             }
        //             foreach (var type in terrainTypes)
        //             {
        //                 if (noiseMap[x, z] < type.threshold)
        //                 {
        //                     pixels[pi] = type.baseTexture.GetPixel(x, z);
        //                     break;
        //                 }
        //             }
        //
        //         }
        //     }
        //
        //     Texture2D texture2D = new Texture2D(pixelLength, pixelLength);
        //     texture2D.wrapMode = TextureWrapMode.Clamp;
        //     texture2D.filterMode = FilterMode.Bilinear;
        //     texture2D.SetPixels(pixels);
        //     texture2D.Apply();
        //
        //     return texture2D;
        // }
    }
}
