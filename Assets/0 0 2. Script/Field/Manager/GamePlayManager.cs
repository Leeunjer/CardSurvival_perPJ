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
            TileRenderer hoveredTile = null;
            Camera mainCamera = Camera.main;
            if (mainCamera != null && BoardMaanger.Instance != null && !isMoving)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    hoveredTile = hit.collider.GetComponentInParent<TileRenderer>();
                    if (hoveredTile != null && (hoveredTile.IsFallen ||
                        BoardMaanger.Instance.IsTileFallen(hoveredTile.tileOffset)))
                    {
                        hoveredTile = null;
                    }
                }
            }

            if (_currentTileRendeer != hoveredTile)
            {
                if (_currentTileRendeer != null) _currentTileRendeer.OnHoverExit();
                _currentTileRendeer = hoveredTile;
            }

            if (_currentTileRendeer == null) return;

            if (Input.GetMouseButtonDown(0) && tilechecking(_currentTileRendeer))
            {
                TileRenderer destinationTile = _currentTileRendeer;
                Vector2Int previousOffset = BoardMaanger.Instance.GetPlsyerPos();
                Vector3 tilePosition = destinationTile.transform.position;
                Vector3 destination = new Vector3(tilePosition.x, 1f, tilePosition.z);

                isMoving = true;
                destinationTile.OnClicked();
                _currentTileRendeer = null;
                BoardType clickedBoard = BoardMaanger.Instance.GetBoardType(destinationTile.tileOffset);


                // Reserve the departed tile before events choose their destinations.
                BoardMaanger.Instance.MarkTileFallen(previousOffset);


                TileEventMove();
                BoardMaanger.Instance.TrySpawnElite();
                BoardMaanger.Instance.TileEventSave();
                playerObject.transform.DOMove(destination, 0.8f).OnComplete(() =>
                {
                    BoardMaanger.Instance.FallTile(previousOffset);
                    isMoving = false;
                    LogAvailableNeighborTiles();
                    OnClickTile(clickedBoard);
                });
                return;
            }

            if (!BoardMaanger.Instance.GetPlayerHas(_currentTileRendeer.tileOffset))
                _currentTileRendeer.OnHoverEnter();
        }



        private void LogAvailableNeighborTiles()
        {
            Vector2Int playerOffset = BoardMaanger.Instance.GetPlsyerPos();
            Vector2Int gridSize = _hexGridLayout.GetGridSize();
            int availableCount = 0;

            for (int x = playerOffset.x - 1; x <= playerOffset.x + 1; x++)
            {
                for (int y = playerOffset.y - 1; y <= playerOffset.y + 1; y++)
                {
                    if (x < 0 || y < 0 || x >= gridSize.x || y >= gridSize.y) continue;

                    GameObject tile = _hexGridLayout.GetTile(new Vector2Int(x, y));
                    if (tile != null && tile.activeInHierarchy &&
                        tile.TryGetComponent(out TileRenderer renderer) && tilechecking(renderer))
                    {
                        availableCount++;
                    }
                }
            }

            Debug.Log($"주변에 이동 가능한 타일: {availableCount}개");

            if(availableCount <= 0)
            {
                GameAlramsManger.Instance.DontMovePlayer();
            }
        }

        private bool tilechecking(TileRenderer tileRenderer)
        {
            if (tileRenderer == null || tileRenderer.IsFallen || isMoving) return false;
            BoardMaanger board = BoardMaanger.Instance;
            return !board.IsTileFallen(tileRenderer.tileOffset)
                && !board.GetPlayerHas(tileRenderer.tileOffset)
                && Utils.GetHexDistance(board.GetPlsyerPos(), tileRenderer.tileOffset) == 1;
        }

        /// <summary>
        /// 이벤트가 있는 타일들을 순회하면서 랜덤한 방향의 이웃 이벤트를 변경하고 해당 타일로 이동한다.
        /// </summary>
        private void TileEventMove()
        {
            List<TileEventRenderer> tileEventRenderers = BoardMaanger.Instance.GetEventTileEventRendererList();
            foreach (TileEventRenderer tileEventRenderer in tileEventRenderers)
            {
                if (tileEventRenderer == null) continue;
                Vector2Int dirOffset  = BoardMaanger.Instance.TryMoveEvent(tileEventRenderer.TileOffset);
                if (dirOffset == tileEventRenderer.TileOffset) continue;
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

                case BoardType.Elite :
                case BoardType.Battle :

                    _boardManager.TileEventSave();
                    SceneManager.LoadScene(2);

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
