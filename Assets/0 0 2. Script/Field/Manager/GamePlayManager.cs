using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;



namespace CardGame
{
    
    public class GamePlayManager : MonoBehaviour
    {


        private TileRenderer _currentTileRendeer;

        [SerializeField]
        private HexGridLayout _hexGridLayout;

        public GameObject playerObject;

        private BoardMaanger _boardManager;

        public static event Action TurnEnd;
        public static event Action TurenStart;

        private bool isMoving = false;
        

        void OnEnable() 
        {
            
        }
        void Start()
        {
            _boardManager = FindFirstObjectByType<BoardMaanger>();
        }

        void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray , out RaycastHit hit))
            {
                if(_currentTileRendeer == null)
                {
                    _currentTileRendeer = hit.transform.GetComponent<TileRenderer>();

                }else if(_currentTileRendeer != hit.transform.GetComponent<TileRenderer>())
                {
                    _currentTileRendeer.OnHoverExit();
                    _currentTileRendeer = hit.transform.GetComponent<TileRenderer>();
                }
                
                if(Input.GetMouseButtonDown(0) && tilechecking(_currentTileRendeer) && !isMoving)
                {
                    Vector3 currentTileRendererPos = _currentTileRendeer.gameObject.transform.position;
                    Vector3 dirPos = new Vector3(currentTileRendererPos.x, 1f , currentTileRendererPos.z);
                    BoardType ClickBoard;
                    ClickBoard = BoardMaanger.Instance.GetBoardType(_currentTileRendeer.tileOffset);
                    isMoving = true;
                    playerObject.transform.DOMove(dirPos , 0.8f).OnComplete(() =>
                    {
                        isMoving = false;
                        OnClickTile(ClickBoard);
                        PlayerDataManager.Instance.PlayerPosSet(_currentTileRendeer.tileOffset);
                        
                    });
                    
                    _currentTileRendeer.OnHoverExit();
                    TileEventMove();
                    
                }


                if(!BoardMaanger.Instance.GetPlayerHas(_currentTileRendeer.tileOffset) && !isMoving)
                _currentTileRendeer.OnHoverEnter();
            }
        }


        private bool tilechecking(TileRenderer tilerenderer)
        {
            if(tilerenderer == null) return false;
            int distance = Utils.GetHexDistance(BoardMaanger.Instance.GetPlsyerPos() ,tilerenderer.tileOffset);
            Debug.Log($" 유저와 클릭 타일 거리 {Utils.GetHexDistance(BoardMaanger.Instance.GetPlsyerPos() ,tilerenderer.tileOffset)} , 플레이어 존제 여부 {BoardMaanger.Instance.GetPlayerHas(tilerenderer.tileOffset)}");

            if(distance > 1 || BoardMaanger.Instance.GetPlayerHas(tilerenderer.tileOffset))
            {
                return false;
            }
            else
            {
                return true;
            }

        }


        /// <summary>
        /// 이벤트가 있는 타일들을 순회하면서 랜덤한 방향의 이웃 이벤트를 변경하고 해당 타일로 이동한다.
        /// </summary>
        private void TileEventMove()
        {
            List<TileEventRenderer> tileEventRenderers = BoardMaanger.Instance.GetEventTileEventRendererList();
            foreach (TileEventRenderer tileEventRenderer in tileEventRenderers)
            {
                Vector2Int dirOffset  = BoardMaanger.Instance.TryMoveEvent(tileEventRenderer.TileOffset);
                GameObject dirTile = _hexGridLayout.GetTile(dirOffset);
                tileEventRenderer.MoveEventRenderer(dirTile , dirOffset);
            }
        }


        /// <summary>
        /// 클릭된 보드 타입에 따라 씬이 바뀐다
        /// </summary>
        /// <param name="boardType"></param>
        private void OnClickTile(BoardType boardType)
        {
            switch (boardType)
            {
                
                case BoardType.None :
                
                break;

                case BoardType.Battle :

                    SceneManager.LoadScene(2);
                    _boardManager.TileEventSave();

                    break;

                case BoardType.Event :
                
                break;

                case BoardType.Shop :
                
                break;

                case BoardType.CampFire :
                
                break;
            }

            boardType =BoardType.None;
        }



    }           
}

