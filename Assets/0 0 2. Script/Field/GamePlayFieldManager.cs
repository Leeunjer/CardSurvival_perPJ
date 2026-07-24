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


    public class GamePlayFieldManager : MonoBehaviour
    {

    public static GamePlayFieldManager Instance {get; private set;}

    public GameObject Player;

    [Header("get GridSize")]
    public Vector2Int _gridSize;

    [Header("Tiledata")]
    [SerializeField]
    private TileData _tileData;

    
    public static event Action OnPlayerSpwan;
    public static event Action<GameObject> OnMouseClick;

    public Dictionary<Vector2Int, TileItem> tileBuffer = new Dictionary<Vector2Int , TileItem>();
    



    private void Awake() {
        
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
    }

        void OnEnable()
        {
            HexGridLayout.OnBoardCreated += SpwanPlayer;
        }

        void OnDisable()
        {
            HexGridLayout.OnBoardCreated -= SpwanPlayer;
        }


        void Start()
        {
            
        }


        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if(Physics.Raycast(ray , out RaycastHit hit))
                {
                    OnMouseClick?.Invoke(hit.transform.gameObject);
                }
            }
        }


        public void SpwanPlayer()
        {
        Debug.Log("SqwanPLayer");
        HexGridLayout hexGridLayout = FindAnyObjectByType<HexGridLayout>();
        _gridSize = hexGridLayout.GetGridSize();

        Vector2Int spwanTile = new Vector2Int(0, 0)
            {
                x = Mathf.RoundToInt(_gridSize.x / 2),
                y = Mathf.RoundToInt(_gridSize.y / 2)
            };

        Vector3 spwanPos = hexGridLayout.GetTilePos(spwanTile) + (Vector3.up * 1);

        Player.SetActive(true);
        Player.transform.position = spwanPos;
        HexGridLayout.OnBoardCreated -= SpwanPlayer;
        OnPlayerSpwan?.Invoke();
        }

        public void TileEventCreate(Vector2Int tileAD)
        {
            //todo tile 딕션너리를 vector2Int값을 통해 tiledata 값을 지정 이후 tiledata값을 찾는 코드 추가 

            if (!tileBuffer.TryGetValue(tileAD , out TileItem tile) ||
            tile == null ||
            tile.boardType == BoardType.None)
            {
                int currentTileNum = UnityEngine.Random.Range(0,_tileData.tileDatas.Length);
                tile = _tileData.tileDatas[currentTileNum];
                tileBuffer[tileAD] = tile;
            }else
            {
                Debug.Log($"{tileAD}Tile has Event");
            }
        }

        public TileItem TileEventGet(Vector2Int tileAD)
        {
            if (!tileBuffer.TryGetValue(tileAD,out TileItem currenttile))
            {
                Debug.Log($"{tileAD} Tile has no Event");
            }
            
            
            tileBuffer.Remove(tileAD);

            return  currenttile; 
        }

        public void TileEventUpdate()
        {
            
        }


    }
}

