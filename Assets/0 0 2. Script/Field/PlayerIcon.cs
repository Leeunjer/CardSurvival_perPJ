using CardGame;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIcon : MonoBehaviour
{
    
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField]private float PlayerHeight = 2f;

    private bool _isMove = false;
    private GameObject _target;
    private Vector3 _targetPos;

    [SerializeField]
    private Image IconVeiw;
    

    

    void OnEnable()
    {
        GamePlayFieldManager.OnMouseClick += OrderPLayerMove;

        IconVeiw.sprite = PlayerDataManager.Instance.currentPlayer.playerIcon;

    }

    void OnDisable()
    {
        GamePlayFieldManager.OnMouseClick -= OrderPLayerMove;
    }

    void Update()
    {
        gameObject.transform.LookAt(Camera.main.transform);
        PlayerMove();
    }

    private void PlayerMove()
    {
        if(!_isMove) return;

        transform.position = Vector3.MoveTowards(transform.position, _targetPos,moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, _targetPos) <= 0.1f )
        {
            _isMove = false;
            transform.position = _targetPos;
            Debug.Log($"타일 도착 ");
        }
        
        
    }


    public void OrderPLayerMove(GameObject targetOBJ)
    {
        if(_isMove) return;

            _target = targetOBJ;
            _targetPos = new Vector3 (_target.transform.position.x , 1 * PlayerHeight, _target.transform.position.z);
            _isMove = true;
        


    }
}
