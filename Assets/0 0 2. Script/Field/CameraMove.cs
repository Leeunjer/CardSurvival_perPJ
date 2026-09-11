using UnityEngine;

public class CameraMove : MonoBehaviour
{


    private PlayerIcon _playerIcon;
    


    void Start()
    {
        _playerIcon = FindFirstObjectByType<PlayerIcon>();
    }


    void Update()
    {
        
    }

    void LateUpdate()
    {
        gameObject.transform.position = _playerIcon.gameObject.transform.position + new Vector3(0,5,0);
        gameObject.transform.rotation = Quaternion.Euler(90,90,90);
    }
}
