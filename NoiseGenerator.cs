using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace MilkSpun.Common
{
    public static class NoiseGenerator
    {
        /// <summary>
        /// 生成2DPerlin噪声
        /// </summary>
        /// <param name="position">坐标</param>
        /// <param name="offset">偏移量</param>
        /// <param name="scale">放大倍数</param>
        /// <param name="resolution">分辨率</param>
        /// <returns>噪声值</returns>
        public static float Get2DPerlinNoise(
            Vector2 position,
            float offset,
            float scale,
            int resolution = 1)
        {
            //坐标不能为整数,Unity自身Bug
            var sampleX = position.x + 0.1f;
            var sampleY = position.y + 0.1f;

            sampleX = sampleX / resolution * scale + offset;
            sampleY = sampleY / resolution * scale + offset;

            return Mathf.PerlinNoise(sampleX, sampleY);
        }

        /// <summary>
        /// 生成3DPerlin噪声
        /// </summary>
        /// <param name="position">坐标</param>
        /// <param name="offset">偏移量</param>
        /// <param name="scale">放大倍数</param>
        /// <param name="threshold">阈值</param>
        /// <param name="resolution">分辨率</param>
        /// <param name="useUnityVersion">是否使用Unity.Mathematics的版本</param>
        /// <returns></returns>
        public static bool Get3DPerlinNoise(
            Vector3 position,
            float offset,
            float scale,
            float threshold,
            int resolution = 1,
            bool useUnityVersion = false)
        {
            var noiseVal = 0f;

            //坐标不能为整数,Unity自身Bug
            var sampleX = position.x + 0.1f;
            var sampleY = position.y + 0.1f;
            var sampleZ = position.z + 0.1f;

            sampleX = sampleX / resolution * scale + offset;
            sampleY = sampleY / resolution * scale + offset;
            sampleZ = sampleZ / resolution * scale + offset;

            if (useUnityVersion)
            {
                noiseVal = noise.snoise(new float3(sampleX, sampleY, sampleZ));
            }
            else
            {
                var ab = Mathf.PerlinNoise(sampleX, sampleY);
                var bc = Mathf.PerlinNoise(sampleY, sampleZ);
                var ac = Mathf.PerlinNoise(sampleX, sampleZ);
                var ba = Mathf.PerlinNoise(sampleY, sampleX);
                var cb = Mathf.PerlinNoise(sampleZ, sampleY);
                var ca = Mathf.PerlinNoise(sampleZ, sampleX);

                noiseVal = (ab + bc + ac + ba + cb + ca) / 6f;
            }

            return noiseVal > threshold;
        }
    }
}
