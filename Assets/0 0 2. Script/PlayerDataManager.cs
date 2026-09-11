using System.Collections.Generic;
using UnityEngine;


namespace CardGame
{
    
    public class PlayerDataManager : MonoBehaviour
    {

        public static PlayerDataManager Instance {get; private set;}

        public PlayerData currentPlayer{get; private set;}

        public Dictionary<Vector2Int , TileData> currentBoardData;

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

        /// <summary>
        /// 플레이어 위치값 저장을 위해 만든 메소드
        /// </summary>
        /// <param name="dirPos">플레이어 위치</param>
        public void PlayerPosSet(Vector2Int dirPos)
        {
            currentPlayer.PlayerPos = dirPos;
        }

        

    }

}
