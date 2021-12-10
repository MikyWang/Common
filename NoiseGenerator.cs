using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MilkSpun.Common
{
    public static class NoiseGenerator
    {
        /// <summary>
        /// 生成2D培林噪声
        /// </summary>
        /// <param name="pos">坐标</param>
        /// <param name="offset">偏移量</param>
        /// <param name="scale">放大倍数</param>
        /// <param name="resolution">分辨率</param>
        /// <returns>噪声值</returns>
        public static float Get2DPerlinNoise(
            Vector2 pos,
            float offset,
            float scale,
            int resolution = 1)
        {
            //坐标不能为整数,Unity自身Bug
            var sampleX = pos.x + 0.1f;
            var sampleY = pos.y + 0.1f;

            sampleX = sampleX / resolution * scale + offset;
            sampleY = sampleY / resolution * scale + offset;

            return Mathf.PerlinNoise(sampleX, sampleY);
        }
    }
}
