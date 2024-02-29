using CodeBase.Services.SaveLoad;

namespace CodeBase.Infrastructure.States
{
    public class GameLoopState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly ISaveLoadService _saveLoadService;

        public GameLoopState(GameStateMachine gameStateMachine, ISaveLoadService saveLoadService)
        {
            _gameStateMachine = gameStateMachine;
            _saveLoadService = saveLoadService;
        }

        public void Enter()
        {
            _saveLoadService.SaveProgress();
        }

        public void Exit()
        {
            
        }
    }
}