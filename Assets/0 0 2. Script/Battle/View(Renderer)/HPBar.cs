
using UnityEngine;
using UnityEngine.UI;


namespace CardGame
{
    
    public class HPBar : MonoBehaviour
    {
        [SerializeField]
        private Image _image;

        [SerializeField]
        private int _Hp;


        void Start()
        {
            
        }


        private void UpdateHpBar(int currentHp , int MaxHp)
        {
            _image.fillAmount = (float)currentHp / MaxHp;
        }

        
    }

}
