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
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        /// <param name="z">z坐标</param>
        /// <returns></returns>
        public bool CheckPositionInWorld(float x,float y,float z);
    }
    
}
