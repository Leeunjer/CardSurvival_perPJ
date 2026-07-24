using System;
using UnityEngine;


namespace CardGame
{

    [Serializable]
    public class TileItem
    {
        public string TileName;
        public BoardType boardType;

    }



    [CreateAssetMenu(fileName = "TileData", menuName = "Scriptable Objects/TileData")]
        public class TileData : ScriptableObject
        {
            public TileItem[] tileDatas; 
        }
}

