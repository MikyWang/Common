using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MilkSpun.Common;
using MilkSpun.Common.Models;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MilkSpun.ChunkWorld.Main
{
    public class TextureManager : MonoBehaviour
    {
        [SerializeField]
        private TextureConfig textureConfig;

        [Button("生成2DArray")]
        private void GenerateArray()
        {
            var textures = textureConfig.Take(9).ToArray();
            var num = 0;
            while (textures.Length > 0)
            {
                GenerateArrayImpl(textures, $"Tex2dArray{num}");
                num++;
                textures = textureConfig.Skip(num * 9).Take(9).ToArray();
            }
        }

        private void GenerateArrayImpl(IReadOnlyList<Texture2D> textures, string fileName)
        {
            var array = new Texture2DArray(textures[0].width, textures[0].height, textures.Count, textures[0].format, true)
            {
                wrapMode = TextureWrapMode.Clamp
            };

            for (var i = 0; i < textures.Count; i++)
            {
                for (var m = 0; m < textures[i].mipmapCount; m++)
                {
                    Graphics.CopyTexture(textures[i], 0, m, array, i, m);
                }
            }
            TextureBuilder.SaveTexture2DArrayToAsset(array, fileName);
        }

    }
}
