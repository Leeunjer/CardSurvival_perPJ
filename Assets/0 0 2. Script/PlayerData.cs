using UnityEngine;

namespace CardGame
{
    public class PlayerData
    {
        public Vector2Int PlayerPos{get ; set;}

        public CardData cardDeck{get; set;}

        public int PlayerHp {get; set;}

        public GameObject playerPrefab;
        
        public Sprite playerIcon;
        
    }

}

