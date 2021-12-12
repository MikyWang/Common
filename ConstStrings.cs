using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MilkSpun.Common
{
    public static class ConstStrings
    {
        public static readonly string MenuNamePrefix = "MilkSpun";
        public static readonly int TerrainTexture = Shader.PropertyToID("_VerticesHeightTexture2D");
        public static readonly int LeftOffset = Shader.PropertyToID("_LeftOffset");
        public static readonly int RightOffset = Shader.PropertyToID("_RightOffset");
        public static readonly int TopOffset = Shader.PropertyToID("_TopOffset");
        public static readonly int BottomOffset = Shader.PropertyToID("_BottomOffset");
        public static readonly int TextureScale = Shader.PropertyToID("_TextureScale");
        public static readonly int FirstTexture = Shader.PropertyToID("_FirstTexture");
        public static readonly int SecondTexture = Shader.PropertyToID("_SecondTexture");
        public static readonly int ThirdTexture = Shader.PropertyToID("_ThirdTexture");
        public static readonly int FourthTexture = Shader.PropertyToID("_FourthTexture");
        public static readonly int FirstThreshold = Shader.PropertyToID("_FirstThreshold");
        public static readonly int SecondThreshold = Shader.PropertyToID("_SecondThreshold");
        public static readonly int ThirdThreshold = Shader.PropertyToID("_ThirdThreshold");
        public static readonly int FourthThreshold = Shader.PropertyToID("_FourthThreshold");
    }
}
