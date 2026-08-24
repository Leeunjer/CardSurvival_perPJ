using UnityEngine;



namespace CardGame
{
    
    public class GamePlayManager : MonoBehaviour
    {


        private TileRenderer _currentTileRendeer;

        public GameObject playerObject;

        

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
                    playerObject.transform.position = _currentTileRendeer.gameObject.transform.position;
                    BoardMaanger.Instance.GetBoardType(_currentTileRendeer.tileOffset);
                    _currentTileRendeer.OnHoverExit();
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

        



    }           
}

