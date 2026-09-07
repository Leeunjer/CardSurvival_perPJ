using UnityEngine;

namespace CardGame
{
    public class BattlePlayManger : MonoBehaviour
    {
        int _attackStack;
        int _guardStack;

        PlayerCharacter _playerObj;

        public static BattlePlayManger Instance {get; private set;}
        void Awake()
        {
            if(Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        void Start()
        {
            _playerObj = FindFirstObjectByType<PlayerCharacter>();
        }
        public void UseCard(CardEffectType cardEffect)
        {
            _playerObj.GetComponent<IICardCommand>().CardEffect(cardEffect);
        }



    }
}

