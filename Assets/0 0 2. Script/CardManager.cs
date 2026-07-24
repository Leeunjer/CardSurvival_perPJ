using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{   
    public class CardManager : MonoBehaviour
    {
        public static CardManager Inst {get; private set;}
        private void Awake() {
            Inst = this;
        }
        [SerializeField] CardData _cardData;

        List<CardItem> cardBuffer;

        void Start()
        {
            SetupCardBuffer();
        }


        private void SetupCardBuffer()
        {
            cardBuffer = new List<CardItem>();
            
            for (int i = 0; i < _cardData.cards.Length; i++)
            {
                CardItem card = _cardData.cards[i];
                for (int j = 0; j < card.CardCount; j++)
                {
                    cardBuffer.Add(card);
                }
            }
            ShuffleCardBuffer();

        }

        public bool CheckBuffer()
        {
            if(cardBuffer.Count != 0) return true;
            return false;
        }

        public CardItem GetCardItem()
        {

            if(cardBuffer.Count <= 0) return null;

            int LastCard = cardBuffer.Count - 1;

            CardItem currentCard = cardBuffer[LastCard];
            cardBuffer.RemoveAt(LastCard);

            return currentCard;
        }

        private void ShuffleCardBuffer()
{
        for (int i = cardBuffer.Count - 1; i > 0; i--)
        {
        int randomIndex = Random.Range(0, i + 1);

        (cardBuffer[i], cardBuffer[randomIndex]) = (cardBuffer[randomIndex], cardBuffer[i]); // 요거 c# 7.0부터 추가된 튜플을 이용한 값교환의 축약형임
        }
}

    }
}

