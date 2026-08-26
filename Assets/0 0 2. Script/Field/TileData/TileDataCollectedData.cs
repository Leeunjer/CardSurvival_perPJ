using System;
using System.Collections.Generic;
using UnityEngine;


namespace CardGame
{

    

    public enum BoardType
    {
        Battle,
        Event,
        Shop,
        CampFire,
        None,

    }
    

    public class TileData
    {
        private BoardType _boardType = BoardType.None;

        public bool isPlayerOnHere {get ; private set;} = false;
        

        private string _tileName;

        public TileData()
        {
            
        }

        public void BoardTypeSetting(BoardType boardType)
        {
            _boardType = boardType;
        }

        public BoardType GetBoardType()
        {
            return _boardType;
        }

        public void PlayerPosSetting( bool playerOnHere)
        {
            isPlayerOnHere = playerOnHere;
        }

        public TileData[] neighborTiles = new TileData[6];




    }


    public class PlayerData
    {
        public Vector2Int PlayerPos{get ; set;}

        public String DeckData{get; set;}

        
    }

    public  class TileDataCollectedData  
    {
        
        
    

        public  Vector2Int _playerPosData{get ; private set;}
    
        //보드 구성 데이터 구성 데이터는 딕셔너리로 벡터 2 인트 와 타일 데이터를 인자로 받는다 타일 데이터는 보드타입과 보드타입 세팅을 갖는다
        private Dictionary<Vector2Int , TileData> _boardData = new Dictionary<Vector2Int, TileData>();

        public static event Action<Vector2Int> OnPlayerMove;
        public static event Action<Dictionary<Vector2Int , TileData>> OnBoardUpdate;

        private PlayerData _playerdata;
        
        private static readonly Vector2Int[] EvenDirections =
        {
        new(-1, 0),  // 왼
        new( 0,-1),  // 좌상
        new( 1,-1),  // 우상
        new( 1, 0),  // 오른
        new( 0, 1),  // 우하
        new(-1, 1)   // 좌하
        };

        private static readonly Vector2Int[] OddDirections =
        {
        new(-1,-1),  // 좌상
        new( 0,-1),  // 우상
        new( 1, 0),  // 오른
        new( 1, 1),  // 우하
        new( 0, 1),  // 좌하
        new(-1, 0)   // 왼
        };


        public void BoardSetting(int horizon , int vertical) // 보드 데이터 생성 및 초기화 담당
        {
            for(int x = 0; x < horizon; x++)
            {
                for(int y = 0; y < vertical; y++)
                {
                    Vector2Int currentTilePos = new Vector2Int(x,y);
                    TileSetting(currentTilePos);
                    
                }
            }
        }

        private void TileSetting(Vector2Int tilePos) // 타일 데이터 생성 및 초기화 담당
        {
            TileData currentTiledata = new TileData();

            _boardData[tilePos] = currentTiledata;
        }

        public void SetUpTileEvent(Vector2Int tilePos , BoardType boardEvent) //타일 이벤트 생성 밑 할당
        {
            _boardData[tilePos].BoardTypeSetting(boardEvent);
        }

        
        public void ConnectingNeighborTile(Vector2Int offset) // 타일의 이웃 타일을 저장
        {
            Vector2Int[] directions = (offset.y % 2 == 0) ? EvenDirections : OddDirections;

            if(!_boardData.TryGetValue(offset , out TileData currentTile))
            return;

            for(int i =0; i < 6; i++)
            {
                Vector2Int neighborCoordinate = offset + directions[i];
                if(_boardData.TryGetValue(neighborCoordinate , out TileData neighborTile))
                {
                    currentTile.neighborTiles[i] = neighborTile;
                }
            }
        }

        
        
        public BoardType PlayerEventUpdate(Vector2Int PlayerDis) // 플레이어 위치 변경에 대한 코드 , 플레이어의 목적지의 보드타입을 반환함
        {
            BoardType boardType = BoardType.None;

            _boardData[_playerPosData].PlayerPosSetting(false);

            _playerPosData = PlayerDis;
            _boardData[_playerPosData].PlayerPosSetting(true);

            boardType = _boardData[_playerPosData].GetBoardType();


            return boardType;
        }

        public void SetUpPlayerData(Vector2Int PlayerPos)
        {
            _boardData[PlayerPos].PlayerPosSetting(true);
            _playerPosData = PlayerPos;
        }
        public bool PlayerTileCheck(Vector2Int TilePos)
        {
            return _boardData[TilePos].isPlayerOnHere;
        }

        



    }
}

