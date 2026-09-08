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
}
