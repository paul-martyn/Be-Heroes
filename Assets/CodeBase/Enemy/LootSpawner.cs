using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.Services.Randomizer;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class LootSpawner : MonoBehaviour
    {
        public EnemyDeath EnemyDeath;
        
        private IGameFactory _gameFactory;
        private IRandomService _randomService;
        private int _lootMin;
        private int _lootMax;

        public void Construct(IGameFactory gameFactory, IRandomService randomService)
        {
            _gameFactory = gameFactory;
            _randomService = randomService;
        }

        private void Start()
        {
            EnemyDeath.Happened += SpawnLoot;
        }

        private async void SpawnLoot()
        {
            EnemyDeath.Happened -= SpawnLoot;

            LootPiece lootPiece = await _gameFactory.CreateLoot();
            lootPiece.transform.position = transform.position;
            lootPiece.GetComponent<UniqueId>().GenerateId();

            Loot loot = GenerateLoot();
      
            lootPiece.Initialize(loot);
        }

        private Loot GenerateLoot() => 
            new(value: _randomService.Next(_lootMin, _lootMax));

        public void SetLootValue(int min, int max)
        {
            _lootMin = min;
            _lootMax = max;
        }
    }
}