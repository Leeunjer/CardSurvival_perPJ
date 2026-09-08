using UnityEngine;
using UnityEngine.UI;


namespace CardGame
{
    

    public class IntroUI_View : MonoBehaviour
    {
        [SerializeField]
        private GameObject _introOptions;

        private Button _startButton;
        
        private Button _settingButton;
        
        private Button _ExitButton; 


        
        [SerializeField]
        private GameObject _CharactorChoice;


        void Start()
        {
            _startButton = _introOptions.transform.Find("[Button] Start").GetComponent<Button>();


            _CharactorChoice.SetActive(false);
            _introOptions.SetActive(true);
            _startButton.onClick.AddListener(ClickStartButton);


        }

        private void ClickStartButton()
        {
            _introOptions.SetActive(false);
            _CharactorChoice.SetActive(true);
        }

        private void ClickSettingButton()
        {
           
            
        }

        private void ClickExitButton()
        {
            

        }



        
    }

}