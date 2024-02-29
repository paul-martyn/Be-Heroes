using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Enemy;
using CodeBase.Hero;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Randomizer;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        public GameObject HeroGameObject;
        
        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticData;
        private readonly IRandomService _randomService;
        private readonly IPersistentProgressService _progressService;
        private readonly IWindowService _windowService;
        private readonly IInputService _inputService;

        public GameFactory(IAssetProvider assetProvider, IStaticDataService staticData, IRandomService randomService,
            IPersistentProgressService progressService, IWindowService windowService, IInputService inputService)
        {
            _assetProvider = assetProvider;
            _staticData = staticData;
            _randomService = randomService;
            _progressService = progressService;
            _windowService = windowService;
            _inputService = inputService;
        }
        
        public async Task WarmUp()
        {
            await _assetProvider.Load<GameObject>(AssetAddress.LootPiece);
            await _assetProvider.Load<GameObject>(AssetAddress.EnemySpawner);
        }

        public async Task<GameObject> CreateHero(Vector3 at)
        {
            HeroGameObject = await InstantiateRegisteredAsync(AssetAddress.Hero, at);
            HeroGameObject.GetComponent<HeroDeath>().Construct(_windowService);
            HeroGameObject.GetComponent<HeroAttack>().Construct(_inputService, _randomService);
            return HeroGameObject ;
        }

        public async Task<GameObject> CreateHud()
        {
            GameObject hud = await InstantiateRegisteredAsync(AssetAddress.Hud);

            hud.GetComponentInChildren<LootCounter>()
                .Construct(_progressService.Progress.WorldData);

            foreach (OpenWindowButton openWindowButton in hud.GetComponentsInChildren<OpenWindowButton>())
                openWindowButton.Construct(_windowService);

            return hud;
        }

        public async Task<LootPiece> CreateLoot()
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(AssetAddress.LootPiece);
            LootPiece lootPiece = InstantiateRegistered(prefab).GetComponent<LootPiece>();

            lootPiece.Construct(_progressService.Progress.WorldData);

            return lootPiece;
        }

        public async Task<GameObject> CreateMonster(MonsterTypeId typeId, Transform parent)
        {
            MonsterStaticData monsterData = _staticData.ForMonster(typeId);

            GameObject prefab = await _assetProvider.Load<GameObject>(monsterData.PrefabReference);
            GameObject monster = Object.Instantiate(prefab, parent.position, Quaternion.Euler(new Vector3(0f ,_randomService.Next(150, 210), 0f)), parent);

            IHealth health = monster.GetComponent<IHealth>();
            health.Current = monsterData.Hp;
            health.Max = monsterData.Hp;

            monster.GetComponent<ActorUI>().Construct(health);
            monster.GetComponent<EnemyAnimator>().Construct(HeroGameObject.GetComponent<HeroDeath>());
            monster.GetComponent<NavMeshAgent>().speed = monsterData.MoveSpeed;

            Attack attack = monster.GetComponent<Attack>();
            attack.Construct(HeroGameObject.transform, HeroGameObject.GetComponent<HeroDeath>());
            attack.Damage = monsterData.Damage;
            attack.Cleavage = monsterData.Cleavage;
            attack.EffectiveDistance = monsterData.EffectiveDistance;

            monster.GetComponent<AgentMoveToPlayer>()?.Construct(HeroGameObject.transform);
            monster.GetComponent<AgentRotateToHero>()?.Construct(HeroGameObject.transform);

            LootSpawner lootSpawner = monster.GetComponentInChildren<LootSpawner>();
            lootSpawner.Construct(this, _randomService);
            lootSpawner.SetLootValue(monsterData.MinLootValue, monsterData.MaxLootValue);

            return monster;
        }

        public async Task CreateSpawner(string spawnerId, Vector3 at, MonsterTypeId monsterTypeId)
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(AssetAddress.EnemySpawner);

            SpawnPoint spawner = InstantiateRegistered(prefab, at).GetComponent<SpawnPoint>();

            spawner.Construct(this);
            spawner.MonsterTypeId = monsterTypeId;
            spawner.Id = spawnerId;
        }

        public void Cleanup()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();

            _assetProvider.Cleanup();
        }

        private GameObject InstantiateRegistered(GameObject prefab, Vector3 at)
        {
            GameObject gameObject = Object.Instantiate(prefab, at, Quaternion.identity);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegistered(GameObject prefab)
        {
            GameObject gameObject = Object.Instantiate(prefab);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath, Vector3 at)
        {
            GameObject gameObject = await _assetProvider.Instantiate(path: prefabPath, at: at);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath)
        {
            GameObject gameObject = await _assetProvider.Instantiate(path: prefabPath);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private void RegisterProgressWatchers(GameObject instantiate)
        {
            foreach (ISavedProgressReader progressReader in instantiate.GetComponentsInChildren<ISavedProgressReader>())
            {
                Register(progressReader);
            }
        }

        private void Register(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter)
                ProgressWriters.Add(progressWriter);
            ProgressReaders.Add(progressReader);
        }
    }
}