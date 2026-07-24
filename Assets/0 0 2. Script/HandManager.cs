using System.Collections.Generic;
using UnityEngine;
namespace CardGame
{
    public class HandManager : MonoBehaviour
    {
        
        public static HandManager Inst {get; private set;}
        void Awake() {
            Inst = this;
        }

        [SerializeField]private GameObject _CardPrefab;
        [SerializeField] private GameObject _CreateCardPos;
        private int _maxCards;
        [SerializeField]private List<GameObject> _handCards;
        



        public void AddCard()
        {
            if(!CardManager.Inst.CheckBuffer())
            {
                Debug.Log("NoCard");
                return;
            }
            var currentCard = Instantiate(_CardPrefab,_CreateCardPos.transform);
            currentCard.transform.SetParent(gameObject.transform);
            _handCards.Add(currentCard);
            ArrangeCards();
            
        }

        public void RemoveCard(GameObject Card)
        {
            _handCards.Remove(Card);
            ArrangeCards();
        }

        public void ArrangeCards ()
        {
            int count = _handCards.Count;
            float center = (count - 1) * 0.5f;

            for (int i = 0; i < count; i++)
            {
                
                float offset = i - center;

                float x = offset * 120f;
                float y = -Mathf.Abs(offset) * 20f;
                float angle = offset * 8f;
                Vector2 CurrentPos = new Vector2(transform.position.x + x, transform.position.y + y);


                _handCards[i]?.GetComponent<Card>().moveCard(CurrentPos);
            }
        }



    }
}

