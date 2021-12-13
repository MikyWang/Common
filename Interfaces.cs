using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MilkSpun.Common
{
    public interface IWorld
    {
        /// <summary>
        /// 检查坐标点是否在地上
        /// </summary>
        /// <param name="position">被检测的坐标点</param>
        /// <returns></returns>
        public bool CheckPositionInWorld(Vector3 position);
    }
}
