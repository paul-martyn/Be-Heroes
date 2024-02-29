using CodeBase.Data;
using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Services.PersistentProgress;
using TMPro;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(UniqueId))]
    public class LootPiece : MonoBehaviour, ISavedProgress
    {
        public GameObject Mesh;
        public TMP_Text LootText;
        public GameObject PickupPopup;
        
        private const float DelayBeforeDestroying = 1.5f;

        private Loot _loot;
        private bool _picked;
        private string _id;
        private bool _pickedUp;
        
        private WorldData _worldData;

        public void Construct(WorldData worldData) => 
            _worldData = worldData;

        public void Initialize(Loot loot) => 
            _loot = loot;

        private void Start() => 
            _id = GetComponent<UniqueId>().Id;
        
        private void OnTriggerEnter(Collider other)
        {
            if (_pickedUp) return;
            
            _pickedUp = true;
            Pickup();
        }

        private void Pickup()
        {
            UpdateWorldData();
            Hide();
            ShowText();

            Destroy(gameObject, DelayBeforeDestroying);
        }

        private void UpdateWorldData()
        {
            UpdateCollectedLootAmount();
            RemoveLootPieceFromSavedPieces();
        }

        private void Hide() => 
            Mesh.SetActive(false);
        
        private void ShowText()
        {
            LootText.text = $"{_loot.Value}";
            PickupPopup.SetActive(true);
        }
        
        private void UpdateCollectedLootAmount() =>
            _worldData.LootData.Collect(_loot);
        
        private void RemoveLootPieceFromSavedPieces()
        {
            LootPieceDataDictionary savedLootPieces = _worldData.LootData.LootPiecesOnScene;

            if (savedLootPieces.Dictionary.ContainsKey(_id)) 
                savedLootPieces.Dictionary.Remove(_id);
        }
        
        public void UpdateProgress(PlayerProgress progress)
        {
            if (_pickedUp)
                return;

            LootPieceDataDictionary lootPiecesOnScene = progress.WorldData.LootData.LootPiecesOnScene;

            if (!lootPiecesOnScene.Dictionary.ContainsKey(_id))
                lootPiecesOnScene.Dictionary
                    .Add(_id, new LootPieceData(transform.position.AsVectorData(), _loot));
        }

        public void LoadProgress(PlayerProgress progress) { }
        
    }
}