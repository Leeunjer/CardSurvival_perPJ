using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public class BoardMaanger : MonoBehaviour
    {
        // 역할은 타일 데이터 콜렉티드를 생성하고 타일 데이터에 맞는 이벤트 설정 계산 및 타일 이벤트 에 맞는 리턴
        public static BoardMaanger Instance {get; private set;}
        
        private TileDataCollectedData _tileDataCollecter = new TileDataCollectedData();

        [SerializeField]
        private TileEventData _tileEventData;

        private List<TileEventItem> _tileEventBuffer;

        [SerializeField]
        private GameObject _playerObjact;
        [SerializeField]
        private HexGridLayout _hexGridLayOut;

        [Header("Event Sprite")]
        public Sprite BattleSprite;
        public Sprite EventSprite;
        public Sprite ShopSprite;
        public Sprite CampFireSprite;

        

        private void Awake() 
        {

            if(Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            _tileEventBuffer = new List<TileEventItem>();
            HexGridLayout.OnBoardCreateComplete += SetUpBoard;
        }

        void Start()
        {
            
        }

        void OnDestroy()
        {
            HexGridLayout.OnBoardCreateComplete -= SetUpBoard;
        }

        private void SetUpBoard(Vector2Int boardSize)
        {
            

            _tileDataCollecter.BoardSetting(boardSize.x,boardSize.y);

            SetUpTileEventBuffer();
            SetUpBoardEvent(boardSize);

            ConnectAllNeighbors(boardSize);
            
            PlayerSpwan(boardSize);
            
        }


        #region 이벤트 셋팅
        private void SetUpBoardEvent(Vector2Int boardSize)
        {
            int Top = 0;
            int Bottom = boardSize.y - 1;
            int Left = 0;
            int Right = boardSize.x - 1;

            while (Top <= Bottom && Left <= Right)
            {
                for (int x = Left; x <= Right; x++)
                {
                    SetUpTileEvent(new Vector2Int(x,Top));
                }
                Top++;

                for(int y = Top; y <= Bottom ; y++)
                {
                    SetUpTileEvent(new Vector2Int(Right , y));
                }
                Right--;

                if(Top <= Bottom)
                {
                    for (int x = Right; x >= Left; x-- )
                    {
                        SetUpTileEvent(new Vector2Int(x,Bottom));
                    }
                    Bottom--;
                }

                if(Left <= Right)
                {
                    for (int y = Bottom; y >= Top; y--)
                    {
                        SetUpTileEvent(new Vector2Int(Left,y));
                    }
                    Left++;
                }

            }
        }

        private void SetUpTileEvent(Vector2Int tilePos)
        {
            
            if(_tileEventBuffer.Count <= 0)
            {
                _tileDataCollecter.SetUpTileEvent(tilePos , BoardType.None);
                return;
            }

            TileEventItem tileEventItem = _tileEventBuffer[0];
            _tileEventBuffer.RemoveAt(0);
            _tileDataCollecter.SetUpTileEvent(tilePos , tileEventItem.boardType);
            switch (tileEventItem.boardType)
            {
                case BoardType.None:
                break;

                case BoardType.Battle:
                SetupEventRender(tilePos , BattleSprite);
                break;

                case BoardType.Event:
                SetupEventRender(tilePos , EventSprite);
                break;

                case BoardType.Shop:
                SetupEventRender(tilePos , ShopSprite);
                break;

                case BoardType.CampFire:
                SetupEventRender(tilePos, CampFireSprite);
                break;
                
            }
        }

   

        private void SetUpTileEventBuffer()
        {
            _tileEventBuffer.Clear();
            for(int i = 0 ; i < _tileEventData.tileEventItems.Length; i++)
            {
                TileEventItem tileEventItem = _tileEventData.tileEventItems[i];
                for(int j = 0 ;j < tileEventItem.BoardCount; j++)
                {
                    _tileEventBuffer.Add(tileEventItem);
                }
            }
            ShuffleBoardBuffer();
        }

        private void ShuffleBoardBuffer()
        {
            for(int i = _tileEventBuffer.Count - 1; i > 0; i--)
            {
                int randomIndex = Random.Range(0, i + 1);
                (_tileEventBuffer[i] , _tileEventBuffer[randomIndex]) = (_tileEventBuffer[randomIndex] , _tileEventBuffer[i]);
            }
        }

        #endregion

        private void ConnectAllNeighbors(Vector2Int boardSize)
        {
            for(int i = 0 ; i < boardSize.x ; i++)
            {
                for (int j =0 ; j < boardSize.y ; j++)
                {
                    _tileDataCollecter.ConnectingNeighborTile(new Vector2Int(i,j));
                }
            }
        }


#region 이벤트 이동
        public void EvnetMove(Vector2Int tileOffset)
        {
            
        }

        private void FindEmtyNeighborTile(TileData eventTile)
        {
            
        }


        #endregion

        public Vector2Int GetPlsyerPos()
        {
            return _tileDataCollecter._playerPosData;
        }
        public bool GetPlayerHas(Vector2Int TargetTile)
        {
            return _tileDataCollecter.PlayerTileCheck(TargetTile);
        }

        public BoardType GetBoardType(Vector2Int TargetTile)
        {
            BoardType boardType = _tileDataCollecter.PlayerEventUpdate(TargetTile);
            Debug.Log($"보드 타입{boardType}");
            return boardType;

        }
        

        
        private void PlayerSpwan(Vector2Int boardSize)
        {
            Debug.Log("PLayerSpwan");

            Vector2Int SpwanTilePos = new Vector2Int()
            {
                x = Mathf.RoundToInt(boardSize.x / 2),
                y = Mathf.RoundToInt(boardSize.y / 2)
            };

            _tileDataCollecter.SetUpPlayerData(SpwanTilePos);
            _playerObjact.transform.position = _hexGridLayOut.GetTile(SpwanTilePos).transform.position + Vector3.up;
            _playerObjact.SetActive(true);
            
        }



        private void SetupEventRender(Vector2Int tileOffset , Sprite eventSprite)
        {
            GameObject EventRenderer = new GameObject($"GameEvent" , typeof(SpriteRenderer));
            
            EventRenderer.transform.SetParent(_hexGridLayOut.GetTile(tileOffset).transform);

            EventRenderer.GetComponent<SpriteRenderer>().sprite = eventSprite;

            EventRenderer.transform.localPosition = new Vector3(0,0.6f,0);
            EventRenderer.transform.localRotation = Quaternion.Euler(90,0,0);
        }





        

        private Vector2Int[] tileSerching(Vector2Int tileOffset , Dictionary<Vector2Int , TileData> boardData)
        {
            Vector2Int[] vector2Ints = new Vector2Int[6];

            for (int i = 0; i < boardData.Count; i++)
            {
                for(int j = 0 ; j<boardData.Count; j++)
                {
                    
                }
            }

            return vector2Ints;
        }

    }
}

