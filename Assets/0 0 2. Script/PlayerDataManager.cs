using UnityEngine;


namespace CardGame
{
    
    public class PlayerDataManager : MonoBehaviour
    {

        public static PlayerDataManager Instance {get; private set;}

        public PlayerData currentPlayer{get; private set;}

        void Awake() 
        {
            if(Instance != null && Instance != this) 
            {
                Destroy(gameObject); 
                return;
            }
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            
        }

        public void CretaePlayerData(GameObject PlayerPrefab , Sprite playericon)
        {
            currentPlayer = new PlayerData
            {
                playerPrefab = PlayerPrefab,
                playerIcon = playericon
            };
        }

        

    }

}
