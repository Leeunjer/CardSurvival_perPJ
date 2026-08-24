using System;
using UnityEngine;




namespace CardGame
{
    [Serializable]
    public class TileEventItem
    {
        public string EventName; // 이벤트 이름
        public BoardType boardType; // 보드 타입
        public int BoardCount; // 퍼센테이지를 만들어서 넣자 싶었지
    }



[CreateAssetMenu(fileName = "TileEventData", menuName = "Scriptable Objects/TileEventData")]
public class TileEventData : ScriptableObject
{
    public TileEventItem[] tileEventItems; 
}


}
