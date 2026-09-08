using UnityEngine;
using UnityEngine.SceneManagement;


namespace CardGame
{
    public class IntroMaanger : MonoBehaviour
    {   

        

        GameObject currentPlayerCharactor;
        Sprite currentPlayerIcon;
        
        public void SettingPLayer()
        {
            PlayerDataManager.Instance.CretaePlayerData(currentPlayerCharactor , currentPlayerIcon);
            Debug.Log($"{PlayerDataManager.Instance.currentPlayer.playerPrefab.name} , {PlayerDataManager.Instance.currentPlayer.playerIcon.name}  select");
            SceneManager.LoadScene(1);
        }

        public void SelectCharactor(GameObject curentOBJ )
        {
            currentPlayerCharactor = curentOBJ;
            
        }
        public void SelectIcon(Sprite sprite)
        {
            currentPlayerIcon = sprite;
        }

        


    }

}
