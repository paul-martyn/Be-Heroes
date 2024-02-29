using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class GameRunner : MonoBehaviour
    {
        [SerializeField] private GameBootstrapper GameBootstrapper;

        private void Awake()
        {
            GameBootstrapper gameBootstrapper = FindObjectOfType<GameBootstrapper>();
            
            if (gameBootstrapper == null) 
                Instantiate(GameBootstrapper);
        }
    }
}