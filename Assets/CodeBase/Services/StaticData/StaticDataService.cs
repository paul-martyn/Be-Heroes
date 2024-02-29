using System.Collections.Generic;
using System.Linq;
using CodeBase.StaticData;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string HeroDataPath = "StaticData/HeroData";
        private const string MonstersDataPath = "StaticData/Monsters";
        private const string LevelsDataPath = "StaticData/Levels";
        private const string WindowConfigsPath = "StaticData/UI/WindowStaticData";

        private HeroStaticData _hero;
        private Dictionary<MonsterTypeId, MonsterStaticData> _monsters;
        private Dictionary<string, LevelStaticData> _levels;
        private Dictionary<WindowId, WindowConfig> _windowConfigs;

        public void Load()
        {
            _hero = Resources
                .Load<HeroStaticData>(path: HeroDataPath);
            
            _monsters = Resources
                .LoadAll<MonsterStaticData>(path: MonstersDataPath)
                .ToDictionary(x => x.MonsterTypeId, x => x);

            _levels = Resources
                .LoadAll<LevelStaticData>(path: LevelsDataPath)
                .ToDictionary(x => x.LevelKey, x => x);
            
            _windowConfigs = Resources
                .Load<WindowStaticData>(path: WindowConfigsPath)
                .Configs
                .ToDictionary(x => x.WindowId, x => x);
        }

        public HeroStaticData ForHero() => 
            _hero;

        public MonsterStaticData ForMonster(MonsterTypeId typeId) =>
            _monsters.GetValueOrDefault(typeId);

        public LevelStaticData ForLevel(string sceneKey) =>
            _levels.GetValueOrDefault(sceneKey);

        public WindowConfig ForWindow(WindowId windowId) =>
            _windowConfigs.GetValueOrDefault(windowId);
        
    }
}