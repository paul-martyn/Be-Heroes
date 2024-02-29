using TMPro;
using UnityEngine;

namespace CodeBase.UI.Windows
{
    public class ShopWindow : WindowBase
    {
        [SerializeField] private TMP_Text LootText;

        protected override void Initialize() => 
            RefreshLootCounter();

        protected override void SubscribeUpdates() => 
            Progress.WorldData.LootData.Changed += RefreshLootCounter;

        protected override void Cleanup()
        {
            base.Cleanup();
            Progress.WorldData.LootData.Changed -= RefreshLootCounter;
        }

        private void RefreshLootCounter() => 
            LootText.SetText(Progress.WorldData.LootData.Collected.ToString());
    }
}