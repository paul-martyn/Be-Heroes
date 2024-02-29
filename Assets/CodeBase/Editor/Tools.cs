using CodeBase.Services;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor
{
    public static class Tools
    {
        [MenuItem("Tools/Clear PlayerPrefs")]
        public static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
        [MenuItem("Tools/Refresh Hero HP")]
        public static void RefreshHeroHp()
        {
            AllServices.Container.Single<IPersistentProgressService>().Progress.HeroState.CurrentHp =
                AllServices.Container.Single<IPersistentProgressService>().Progress.HeroState.MaxHp;
            
            AllServices.Container.Single<ISaveLoadService>().SaveProgress();
        }
    }
}

