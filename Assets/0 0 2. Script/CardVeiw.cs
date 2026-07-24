using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace CardGame
{
    public class CardVeiw : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _cost;
        [SerializeField] CardItem _cardData;

        
        private void OnEnable() 
        {
            
        }


        public void VeiwCardData(CardItem cardData)
        {
            

            _cardData = cardData;

            if (_cardData == null)
            {
                Debug.Log("No Card Data");
                _icon.sprite = null;
                _name.text = "noName";
                _cost.text = "noCost";
                gameObject.SetActive(false);
                return;
            }

            _icon.sprite = _cardData.icon;
            _name.text = _cardData.cardName;
            _cost.text = _cardData.cost.ToString();

            
        }
    }   
}

