
using UnityEngine;
using UnityEngine.UI;


namespace CardGame
{
    
    public class HPBar : MonoBehaviour
    {
        [SerializeField]
        private Image _image;

        
        public int _Hp {get; private set;}

        [SerializeField]
        private int _maxHp;


        void Start()
        {
            _Hp = _maxHp;
            UpdateHpBar(_Hp,_maxHp);
            Debug.Log($"초기 HP : {_Hp}");
        }

        /// <summary>
        /// hp바를 업데이트 하는 메소드
        /// </summary>
        /// <param name="currentHp">현재 체력</param>
        /// <param name="MaxHp"> 최대 체력</param>
        private void UpdateHpBar(int currentHp , int MaxHp)
        {

            _image.fillAmount = (float)currentHp / MaxHp;
        }

        /// <summary>
        /// 데미지를 받을 경우 이를 hp에 감소 후 반영하는 코드
        /// </summary>
        /// <param name="damage">데미지 량</param>
        public void HitEnemy(int damage)
        {
            _Hp -= damage;
            Debug.Log($"hp량 {_Hp}");
            UpdateHpBar(_Hp , _maxHp);   
        }

        
    }

}
