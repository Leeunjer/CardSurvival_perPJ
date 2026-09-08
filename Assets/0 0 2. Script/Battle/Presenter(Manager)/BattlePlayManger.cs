using UnityEngine;
using UnityEngine.SceneManagement;

namespace CardGame
{
    public class BattlePlayManger : MonoBehaviour
    {
        int _attackStack;
        int _guardStack;

        PlayerCharacter _playerObj;

        
        EnemyHpbar _enemyHpbar;

        PlayerHpBar _playerHpBar;

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
            _playerHpBar = FindFirstObjectByType<PlayerHpBar>();
            _enemyHpbar = FindFirstObjectByType<EnemyHpbar>();
        }
        public void UseCard(CardEffectType cardEffect)
        {
            _playerObj.GetComponent<IICardCommand>().CardEffect(cardEffect);

        }

        public void HitDamge(int damage)
        {
            _enemyHpbar.HitEnemy(damage);


            if(_enemyHpbar._Hp <= 0)
            {
                
                SceneManager.LoadScene(1);
            }
        }









    }
}

