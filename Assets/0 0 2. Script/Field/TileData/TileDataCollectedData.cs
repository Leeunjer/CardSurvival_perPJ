using System;
using System.Collections.Generic;
using UnityEngine;


namespace CardGame
{

    

    public enum BoardType
    {
        Battle,
        Event,
        None,

    }
    

    public class TileData
    {
        private BoardType _boardType;

        private bool isPlayerOnHere;

        public void BoardTypeSetting(BoardType boardType)
        {
            _boardType = boardType;
        }

        public BoardType GetBoardType()
        {
            return _boardType;
        }


    }

    public  class TileDataCollectedData  
    {

    

        private  Vector2Int _playerPosData;
    
        //보드 구성 데이터 구성 데이터는 딕셔너리로 벡터 2 인트 와 타일 데이터를 인자로 받는다 타일 데이터는 보드타입과 보드타입 세팅을 갖는다
        private Dictionary<Vector2Int , TileData> _boardData;

        public static event Action<Vector2Int> OnPlayerMove;
        public static event Action<Dictionary<Vector2Int , TileData>> OnBoardUpdate;
        


        public void BoardUpdate()
        {
            OnBoardUpdate?.Invoke(_boardData);
        }

        public BoardType PlayerMove(Vector2Int PlayerDis)
        {
            // TODO 디렉토리 가서 이벤트 찾기
            return BoardType.None;
        }



    }
}

