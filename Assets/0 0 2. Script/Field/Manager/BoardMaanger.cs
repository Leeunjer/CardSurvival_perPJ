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

        /// <summary>
        /// 타일 이벤트 스프라이트를 관리하는 게임 오브젝트의 보드 오프셋
        /// </summary>
        private List<TileEventRenderer> _TileEventRenderers = new List<TileEventRenderer>();

        private bool _isDataLoad = false;
        

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

            _isDataLoad = LoadBoardData();


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

        /// <summary>
        /// 타일이 생성 되었을 때 그 생성된 타일들에게 이벤트를 부여 (부를 데이터가 있으면 데이터를 기준으로 데이터가 없으면 랜덤으로 부여) \n
        ///  이벤트가 부여된 타일들은 그 위에 타일 이벤트 렌더러 생성
        /// </summary>
        /// <param name="tilePos"></param>
        private void SetUpTileEvent(Vector2Int tilePos) 
        {
            BoardType tileEventType;
            if (_isDataLoad)
            {
                tileEventType  = _tileDataCollecter.GetTileData(tilePos).GetBoardType();
                Debug.Log($"{tileEventType}");
                
            }
            else
            {
                if(_tileEventBuffer.Count <= 0)
                {
                _tileDataCollecter.SetUpTileEvent(tilePos , BoardType.None);
                return;
                }

                tileEventType = _tileEventBuffer[0].boardType;
                _tileEventBuffer.RemoveAt(0);
                _tileDataCollecter.SetUpTileEvent(tilePos , tileEventType);
            }

            
            



            switch (tileEventType)
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
            if(_isDataLoad)return;

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

        private void SetupEventRender(Vector2Int tileOffset , Sprite eventSprite)
        {
            GameObject EventRenderer = new GameObject($"GameEvent" , typeof(TileEventRenderer));
            TileEventRenderer tileEventRenderer = EventRenderer.GetComponent<TileEventRenderer>();
            _TileEventRenderers.Add(tileEventRenderer);
            
            EventRenderer.transform.SetParent(_hexGridLayOut.GetTile(tileOffset).transform);

            tileEventRenderer.SetSprite(eventSprite);
            tileEventRenderer.tile = _hexGridLayOut.GetTile(tileOffset);
            tileEventRenderer.TileOffset = tileOffset;
            

            EventRenderer.transform.localPosition = new Vector3(0,0.6f,0);
            EventRenderer.transform.localRotation = Quaternion.Euler(90,0,0);
        }


        /// <summary>
        /// 씬이 이동할 때 마다 타일 이벤트를 저장하고 다시 돌아올 때 그 내용을 저장하는 메소드
        /// </summary>
        public void TileEventSave()
        {
            if(PlayerDataManager.Instance.currentBoardData == null)
            {
                PlayerDataManager.Instance.currentBoardData = new Dictionary<Vector2Int, TileData>();
            }
            PlayerDataManager.Instance.currentBoardData.Clear();
            PlayerDataManager.Instance.currentBoardData = _tileDataCollecter.GetBoardData();
            

            
        }

        /// <summary>
        /// 씬 이동시 타일 데이터가 존제 할 때 그 데이터를 불러오는 메소드
        /// </summary>
        /// <returns></returns>
        private bool LoadBoardData()
        {
            if(PlayerDataManager.Instance.currentBoardData == null) return false;

            
            _tileDataCollecter.SetBoardData(PlayerDataManager.Instance.currentBoardData);
            _tileDataCollecter.SetUpPlayerData(PlayerDataManager.Instance.currentPlayer.PlayerPos);


            _tileEventBuffer.Clear();
            return true;
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




        /// <summary>
        /// 이벤트 타일의 offset을 인자로 받으면 해당 offset의 타일의 이웃이 되는 타일 중 이벤트가 없는 타일과 이벤트를 바꾼다 그리고 이웃이 되는 타일의 offset을 return한다
        /// </summary>
        /// <param name="tileOffset"></param>
        public Vector2Int TryMoveEvent(Vector2Int tileOffset) //타일 값을 받으면 변경하는 것
        {
            ChangeTileEvent(_tileDataCollecter.GetTileData(tileOffset) , FindEmtyNeighborTile(_tileDataCollecter.GetTileData(tileOffset)) , out Vector2Int NeighborTileOffset , out bool isComplete);
            
            return isComplete ? NeighborTileOffset : tileOffset;
        }

        private TileData FindEmtyNeighborTile(TileData eventTile) // 해당 타일의 이웃 타일 중 이벤트가 없는 타일 찾는 기능
        {
            List<TileData> EmtyTile = new List<TileData>();
            foreach (TileData tile in eventTile.neighborTiles)
            {
                if(tile != null  && !tile.isPlayerOnHere && tile.GetBoardType() == BoardType.None)
                {
                    EmtyTile.Add(tile);
                }
            }

            if (EmtyTile.Count == 0)
            {
                return null;
            }


            return EmtyTile[Random.Range(0 , EmtyTile.Count)];

        }

        private void ChangeTileEvent(TileData tileDataA , TileData tileDataB , out Vector2Int tileDataBOffset , out bool isComplete) // 타일 a와 타일 b의 이벤트를 교환함
        {

            if(tileDataA == null ||tileDataB == null) 
            {
                tileDataBOffset = Vector2Int.zero;
                isComplete = false;
                return;
            }
            BoardType emp;
            
            emp = tileDataA.GetBoardType();
            tileDataA.BoardTypeSetting(tileDataB.GetBoardType());
            tileDataB.BoardTypeSetting(emp);
            tileDataBOffset = tileDataB.tileOffset;
            isComplete = true;

        }




        #endregion

        #region getSet 시리즈

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
            for(int i = _TileEventRenderers.Count -1; i >= 0; i--)
            {
                TileEventRenderer tileEventRenderer = _TileEventRenderers[i];
                if(tileEventRenderer != null && tileEventRenderer.TileOffset == TargetTile)
                {
                    Destroy(tileEventRenderer.gameObject);
                    
                    _TileEventRenderers.RemoveAt(i);
                    break;
                }
            }
            Debug.Log($"보드 타입{boardType}");

            
            return boardType;

        }

        public List<TileEventRenderer> GetEventTileEventRendererList()
        {
            return _TileEventRenderers;
        }

        #endregion

        

        
        private void PlayerSpwan(Vector2Int boardSize)
        {
            Vector2Int SpwanTilePos;
            if(!_isDataLoad)
            {
            Debug.Log("PLayerSpwan");
            SpwanTilePos = new Vector2Int()
            {
                x = Mathf.RoundToInt(boardSize.x / 2),
                y = Mathf.RoundToInt(boardSize.y / 2)
            };
            _tileDataCollecter.SetUpPlayerData(SpwanTilePos);
            }
            else
            {   
                Debug.Log("playerLoad");
                
                SpwanTilePos = _tileDataCollecter._playerPosData;
            }


            _playerObjact.transform.position = _hexGridLayOut.GetTile(SpwanTilePos).transform.position + Vector3.up;
            _playerObjact.SetActive(true);
            
        }



        





        

        

    }
}

