
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace CardGame
{
    public class Card : MonoBehaviour, IDragHandler , IEndDragHandler
    {

        [SerializeField]GraphicRaycaster _raycaster;
        List<RaycastResult> _results =  new();
        private CardItem _cardData;

        void Awake() {
            _raycaster = FindFirstObjectByType<GraphicRaycaster>();
        }

        void OnEnable() {
            _cardData = CardManager.Inst.GetCardItem();
            gameObject.GetComponent<CardVeiw>().VeiwCardData(_cardData);
        }
        public void OnDrag(PointerEventData eventData)
        {
            gameObject.transform.position = eventData.position;
            gameObject.GetComponent<Image>().raycastTarget = false;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(eventData.position);


                _results.Clear();
                _raycaster.Raycast(eventData,_results);
                if(_results.Count > 0)
                {
                    foreach(var result in _results)
                    {
                        Debug.Log(result.gameObject.name);

                    }
                }

                BattlePlayManger.Instance.UseCard(_cardData.effactType);
                HandManager.Inst.RemoveCard(gameObject);
                gameObject.SetActive(false);

                /*
                if(Physics.Raycast(ray,out RaycastHit hit , 80f))
                {
                    Debug.Log(hit.collider.name);

                    if (hit.collider.CompareTag("Player"))
                    {
                        hit.collider.GetComponent<IICardCommand>().CardEffect(_cardData.effactType);
                        HandManager.Inst.RemoveCard(gameObject);
                        gameObject.SetActive(false);
                    }
                }
                */



            }
            
            HandManager.Inst.ArrangeCards();
            gameObject.GetComponent<Image>().raycastTarget = true;


            
        }

        public void moveCard(Vector2 posiotion)
        {
            gameObject.transform.DOMove(posiotion,0.5f);
        }
    }
}

