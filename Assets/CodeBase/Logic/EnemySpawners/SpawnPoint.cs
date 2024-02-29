using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Logic.EnemySpawners
{
    public class SpawnPoint : MonoBehaviour, ISavedProgress
    {
        public MonsterTypeId MonsterTypeId;
        public string Id { get; set; }
        
        private bool _slain;
        private IGameFactory _gameFactory;
        private EnemyDeath _enemyDeath;

        public void Construct(IGameFactory gameFactory)
        { 
            _gameFactory = gameFactory;
        }

        public void LoadProgress(PlayerProgress playerProgress)
        {
            if (playerProgress.KillData.ClearedSpawners.Contains(Id))
                _slain = true;
            else
                Spawn();
        }
        
        public void UpdateProgress(PlayerProgress playerProgress)
        {
            if (_slain)
                playerProgress.KillData.ClearedSpawners.Add(Id);
        }

        private async void Spawn()
        {
            GameObject monster = await _gameFactory.CreateMonster(MonsterTypeId, transform);
            _enemyDeath = monster.GetComponent<EnemyDeath>();
            _enemyDeath.Happened += Slay;
        }

        private void Slay()
        {
            if (_enemyDeath != null)
                _enemyDeath.Happened -= Slay;
      
            _slain = true;
        }
    }
}