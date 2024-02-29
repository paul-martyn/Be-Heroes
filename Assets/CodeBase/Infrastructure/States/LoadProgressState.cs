using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;

namespace CodeBase.Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IStaticDataService _staticData;
        
        private const string InitialLevelName = "Maze";

        public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService progressService, ISaveLoadService saveLoadService, IStaticDataService staticData)
        {
            _gameStateMachine = gameStateMachine;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            _staticData = staticData;
        }

        public void Enter()
        {
            LoadProgressOrInitNew();
            _gameStateMachine.Enter<LoadLevelState, string>(_progressService.Progress.WorldData.PositionOnLevel.Level);
        }

        public void Exit() { }

        private void LoadProgressOrInitNew() =>
            _progressService.Progress = _saveLoadService.LoadProgress() ?? NewProgress();

        private PlayerProgress NewProgress()
        {
            HeroStaticData heroStaticData = _staticData.ForHero();
            PlayerProgress playerProgress = new(InitialLevelName)
            {
                States =
                {
                    MinDamage = heroStaticData.MinDamage,
                    MaxDamage = heroStaticData.MaxDamage,
                    DamageRadius = heroStaticData.DamageRadius
                },
                HeroState =
                {
                    MaxHp = heroStaticData.MaxHp
                }
            };

            playerProgress.HeroState.ResetHp(); 
            return playerProgress;
        }
    }
}