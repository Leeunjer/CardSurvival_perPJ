using TMPro;
using UnityEngine;

public class GameAlramsManger : MonoBehaviour
{
    [SerializeField]
    GameObject _gameoverAlram;
    [SerializeField]
    TextMeshProUGUI GameOverReson;
    

    public static GameAlramsManger Instance{get; private set;}

    string dontMove = "이동할 수 있는 타일이 없습니다";
    string HpZero ="현제 남은 체력이 없습니다";



    private void Awake() {
        
         if(Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _gameoverAlram.SetActive(false);
    }

    public void DontMovePlayer()
    {
        _gameoverAlram.SetActive(true);
        GameOverReson.text = dontMove;
    }

    public void PlayerDie()
    {
        _gameoverAlram.SetActive(true);
        GameOverReson.text = HpZero;
    }

    


}
