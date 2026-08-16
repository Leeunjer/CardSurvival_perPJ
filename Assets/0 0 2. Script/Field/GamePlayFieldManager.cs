using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{

    


    public class GamePlayFieldManager : MonoBehaviour
    {

    public static GamePlayFieldManager Instance {get; private set;}

    public GameObject Player;

    
    public Vector2Int _gridSize;

    [Header("Tiledata")]
    [SerializeField]
    private TileData _tileData;

    
    public static event Action OnPlayerSpwan;
    public static event Action<GameObject> OnMouseClick;
    public static event Action OnHoverEnter;
    public static event Action OnHoverExit;

    //public Dictionary<Vector2Int, TileEvent> tileBuffer = new Dictionary<Vector2Int , TileEvent>();
    
    private GameObject _currentGameObject;

    private HexScript _currentHex;

    private Vector2Int PlayerTile;

    private HexGridLayout hexGridLayout;


    /*
    private void Awake() {
        
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        
        hexGridLayout  = FindAnyObjectByType<HexGridLayout>();
        
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray , out RaycastHit hit))
            {
                if(_currentGameObject == null)
                {
                    _currentGameObject = hit.transform.gameObject;
                }else if (hit.transform.gameObject != _currentGameObject)
                {
                    _currentGameObject.GetComponent<IHoverable>()?.OnHoverExit();
                    _currentGameObject = hit.transform.gameObject;
                    return;
                }

                if (Input.GetMouseButtonDown(0) && TileChecking(hit.transform.gameObject))
                {
                    OnMouseClick?.Invoke(hit.transform.gameObject);
                    if(_currentHex != null)
                    {
                        _currentHex.OnClicked();
                    }
                    _currentHex = hit.transform.gameObject.GetComponent<HexScript>();
                    

                    Vector2Int _tileoffset;
                    BoardType _boardType;

                    _tileoffset = _currentHex.TileOffset;
                    //_boardType = tileBuffer[_tileoffset].boardType;

                    //Debug.Log($"{_tileoffset} 의  보드타입 {_boardType}");

                    _currentHex.OnClicked();
                    OnHoverExit?.Invoke();
                }
                hit.transform.gameObject.GetComponent<IHoverable>()?.OnHoverEnter();

            }


        }




        public void SpwanPlayer()
        {


        Debug.Log("SqwanPLayer");
        _gridSize = hexGridLayout.GetGridSize();

        Vector2Int spwanTile = new Vector2Int(0, 0)
            {
                x = Mathf.RoundToInt(_gridSize.x / 2),
                y = Mathf.RoundToInt(_gridSize.y / 2)
            };

        PlayerTile = spwanTile;
        GameObject spwanTileOBJ = hexGridLayout.GetTile(spwanTile);
        spwanTileOBJ.GetComponent<HexScript>().OnClicked();
        Vector3 spwanPos = spwanTileOBJ.transform.position + (Vector3.up * 2);

        Player.SetActive(true);
        Player.transform.position = spwanPos;
        HexGridLayout.OnBoardCreated -= SpwanPlayer;
        OnPlayerSpwan?.Invoke();
        }


        private bool TileChecking(GameObject targetTile)
        {
            if(!targetTile.GetComponent<HexScript>()) return false;

            HexScript hextile = targetTile.GetComponent<HexScript>();
            Debug.Log(Utils.GetHexDistance(PlayerTile , hextile.TileOffset));
            if(Utils.GetHexDistance(PlayerTile , hextile.TileOffset) > 1)
            {
                return false;
            }else
            {
                return true;
            }

        }

        
        public void TileEventCreate(Vector2Int tileAD)
        {
           

            if (!tileBuffer.TryGetValue(tileAD , out TileEvent tile) ||
            tile == null ||
            tile.boardType == BoardType.None)
            {
                int currentTileNum = UnityEngine.Random.Range(0,_tileData.EventDatas.Length);
                tile = _tileData.EventDatas[currentTileNum];
                tileBuffer[tileAD] = tile;
            }else
            {
                Debug.Log($"{tileAD}Tile has Event");
            }
        }

        public TileEvent TileEventGet(Vector2Int tileAD)
        {
            if (!tileBuffer.TryGetValue(tileAD,out TileEvent currenttile))
            {
                Debug.Log($"{tileAD} Tile has no Event");
            }
            
            
            tileBuffer.Remove(tileAD);

            return  currenttile; 
        }

        public void TileEventUpdate()
        {
            
        }


        */


    }
}

