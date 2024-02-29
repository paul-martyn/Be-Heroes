using CodeBase.Data;
using TMPro;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class LootCounter : MonoBehaviour
    {
        public TMP_Text Counter;
        private WorldData _worldData;

        public void Construct(WorldData worldData)
        {
            _worldData = worldData;
            worldData.LootData.Changed += UpdateCounter;

            UpdateCounter();
        }

        private void UpdateCounter() => 
            Counter.SetText($"{_worldData.LootData.Collected}");
    }
}