using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MilkSpun.Common.Models
{
    [CreateAssetMenu(fileName = "_Textures",menuName = "MilkSpun/创建纹理组")]
    public class TextureConfig:ScriptableObject,IEnumerable<Texture2D>
    {
        public string arrayName;
        [InlineEditor]
        public List<Texture2D> textures;
        public IEnumerator<Texture2D> GetEnumerator()
        {
            return textures.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this).GetEnumerator();
        }
        public Texture2D this[int index]
        {
            set => textures[index] = value;
            get => textures[index];
        }
        public int Count => textures.Count;
    }
}
