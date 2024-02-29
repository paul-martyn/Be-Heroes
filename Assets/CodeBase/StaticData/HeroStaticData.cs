using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "HeroData", menuName = "StaticData/Hero")]
    public class HeroStaticData : ScriptableObject
    {
        [Range(1, 100)]
        public int MaxHp;

        [Range(1f, 10f)]
        public float Speed;

        [Range(1, 5)]
        public int MinDamage;
        [Range(5, 10)]
        public int MaxDamage;
        
        [Range(0.5f, 2f)]
        public float DamageRadius;
    }
}
