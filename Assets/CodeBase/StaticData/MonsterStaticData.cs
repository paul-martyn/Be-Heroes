using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "MonsterData", menuName = "StaticData/Monster")]
    public class MonsterStaticData : ScriptableObject
    {
        public MonsterTypeId MonsterTypeId;
        
        [Range(1, 100)]
        public int Hp;

        [Range(1f, 5f)]
        public float MoveSpeed = 1f;

        [Range(1f, 30f)]
        public float Damage;

        [Range(0.5f, 1f)]
        public float EffectiveDistance = 0.5f;

        [Range(0.5f, 1f)]
        public float Cleavage = 0.5f;

        public int MinLootValue;
        public int MaxLootValue;

        public AssetReferenceGameObject PrefabReference;
    }
}