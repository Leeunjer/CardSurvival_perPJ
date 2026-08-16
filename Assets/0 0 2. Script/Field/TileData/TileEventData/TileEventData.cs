using System;
using UnityEngine;




namespace CardGame
{
    [Serializable]
    public class TileEventItem
    {
        public string EventName;
        public BoardType boardType;
        public int BoardCount;
    }



[CreateAssetMenu(fileName = "TileEventData", menuName = "Scriptable Objects/TileEventData")]
public class TileEventData : ScriptableObject
{
    public TileEventItem[] tileEventItems; 
}


}
