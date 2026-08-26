using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;



namespace CardGame
{
    
    public class GamePlayManager : MonoBehaviour
    {


        private TileRenderer _currentTileRendeer;

        [SerializeField]
        private HexGridLayout _hexGridLayout;

        public GameObject playerObject;

        public static event Action TurnEnd;
        public static event Action TurenStart;
        

        void OnEnable() 
        {
            
        }
        void Start()
        {
            
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
                
                if(Input.GetMouseButtonDown(0) && tilechecking(_currentTileRendeer))
                {
                    //playerObject.transform.position = _currentTileRendeer.gameObject.transform.position;
                    playerObject.transform.DOMove(_currentTileRendeer.gameObject.transform.position , 0.8f);
                    BoardMaanger.Instance.GetBoardType(_currentTileRendeer.tileOffset);
                    _currentTileRendeer.OnHoverExit();
                    TileEventMove();
                }


                if(!BoardMaanger.Instance.GetPlayerHas(_currentTileRendeer.tileOffset))
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



    }           
}

