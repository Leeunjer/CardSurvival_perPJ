using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CardGame
{
    public class BattlePlayManger : MonoBehaviour
    {
        int _attackStack;
        public int AttackStack => _attackStack;
        int _guardStack;
        readonly Dictionary<CardItem, int> _cardUseCounts = new();
        public int GuardStack => _guardStack;
        Coroutine _turnCoroutine;
        bool _playerActionRequested;
        bool _playerTurnEndRequested;

        public bool CanUseCard => CurrentState == Turenstate.PlayerTurn
            && !_playerActionRequested && !_playerTurnEndRequested;

        PlayerCharacter _playerObj;

        EnemyHpbar _enemyHpbar;

        PlayerHpBar _playerHpBar;

        /// <summary>
        /// 턴은 TurnSet -> PlayerStart -> EnemyStart -> PlayerTurn -> EnemyTurn -> ... -> PlayerEnd -> EnemyEnd -> Battle -> TurnStart... 와 같은 방식으로 진행 된다
        /// </summary>
        public enum Turenstate
        {
            GameStart,
            TurnSet,
            PlayerTurnStart,
            EnemyTurnStart,
            PlayerTurn,
            EnemyTurn,
            PlayerturnEnd,
            EnemyturnEnd,
            Battle,
            Vectory,
            Defeat

        }

        public Turenstate CurrentState {get; private set;}
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
            ChangeTurnState(Turenstate.GameStart);
        }

        public void ChangeTurnState(Turenstate newState)
        {
            if (_turnCoroutine != null)
            {
                StopCoroutine(_turnCoroutine);
            }

            CurrentState = newState;
            _turnCoroutine = StartCoroutine(HandleTurnState(newState));
        }

        private IEnumerator HandleTurnState(Turenstate state)
        {
            // Start 초기화와 코루틴 핸들 할당이 끝난 뒤 상태 처리를 시작한다.
            yield return null;

            while (true)
            {
                CurrentState = state;
                switch (state)
                {
                    case Turenstate.GameStart:
                        yield return GameStart();
                        state = Turenstate.TurnSet;
                        break;
                    case Turenstate.TurnSet:
                        yield return TurnSet();
                        state = Turenstate.PlayerTurnStart;
                        break;
                    case Turenstate.PlayerTurnStart:
                        yield return PlayerTurnStart();
                        state = Turenstate.EnemyTurnStart;
                        break;
                    case Turenstate.EnemyTurnStart:
                        yield return EnemyTurnStart();
                        state = Turenstate.PlayerTurn;
                        break;
                    case Turenstate.PlayerTurn:
                        yield return PlayerTurn();
                        state = _playerTurnEndRequested
                            ? Turenstate.PlayerturnEnd : Turenstate.EnemyTurn;
                        break;
                    case Turenstate.EnemyTurn:
                        yield return EnemyTurn();
                        state = Turenstate.PlayerTurn;
                        break;
                    case Turenstate.PlayerturnEnd:
                        yield return PlayerTurnEnd();
                        state = Turenstate.EnemyturnEnd;
                        break;
                    case Turenstate.EnemyturnEnd:
                        yield return EnemyTurnEnd();
                        state = Turenstate.Battle;
                        break;
                    case Turenstate.Battle:
                        yield return Battle();
                        state = GetBattleResult();
                        break;
                    case Turenstate.Vectory:
                        yield return Victory();
                        _turnCoroutine = null;
                        yield break;
                    case Turenstate.Defeat:
                        yield return Defeat();
                        _turnCoroutine = null;
                        yield break;
                    default:
                        _turnCoroutine = null;
                        yield break;
                }

                if (state != Turenstate.Vectory && state != Turenstate.Defeat)
                {
                    Turenstate result = GetBattleResult();
                    if (result == Turenstate.Vectory || result == Turenstate.Defeat)
                    {
                        state = result;
                    }
                }
            }
        }

        private IEnumerator GameStart()
        {
            yield return null;
        }

        private IEnumerator TurnSet()
        {
            _attackStack = 0;
            _guardStack = 0;
            _cardUseCounts.Clear();
            _playerActionRequested = false;
            _playerTurnEndRequested = false;

            
            yield return null;
        }

        private IEnumerator PlayerTurnStart()
        {
            // 턴 시작 시 드로우 및 버프 처리를 추가한다.
            for (int i = 0; i <= 5 ; i++)
            {
                HandManager.Inst.AddCard();
            }

            yield return null;
        }

        private IEnumerator EnemyTurnStart()
        {
            // 적의 턴 시작 효과를 추가한다.

            yield return null;
        }

        private IEnumerator PlayerTurn()
        {
            yield return new WaitUntil(() => _playerActionRequested || _playerTurnEndRequested
                || GetBattleResult() != Turenstate.TurnSet);
            yield return new WaitUntil(() => _playerObj == null || !_playerObj.IsProcessing);
            _playerActionRequested = false;
        }

        private IEnumerator EnemyTurn()
        {
            // 적 AI의 행동 코루틴을 이 위치에서 기다린다.
            yield return null;
        }

        private IEnumerator PlayerTurnEnd()
        {
            if (_playerObj != null)
            {
                yield return _playerObj.ProcessCommand();
            }
        }

        private IEnumerator EnemyTurnEnd()
        {
            // 적의 턴 종료 효과를 추가한다.
            yield return null;
        }

        private IEnumerator Battle()
        {
            // 턴 종료 후 정산할 전투 효과를 추가한다.
            yield return null;
        }

        private IEnumerator Victory()
        {
            yield return new WaitForSeconds(1.0f);

            Utils.FadeOutIn(()=> {SceneManager.LoadScene(1);});
            
        }

        private IEnumerator Defeat()
        {
            Debug.Log("패배");
            yield return null;
        }

        private Turenstate GetBattleResult()
        {
            if (_playerHpBar != null && _playerHpBar._Hp <= 0)
                return Turenstate.Defeat;
            if (_enemyHpbar != null && _enemyHpbar._Hp <= 0)
                return Turenstate.Vectory;
            return Turenstate.TurnSet;
        }

        // 턴 종료 버튼의 OnClick에서 호출한다.
        public void EndPlayerTurn()
        {
            if (CurrentState == Turenstate.PlayerTurn)
            {
                _playerTurnEndRequested = true;
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
        public void UseCard(CardItem card)
        {
            TryUseCard(card);
        }

        public bool CanUseCardEffect(CardItem card)
        {
            if (card == null || !CanUseCard || _playerObj == null) return false;

            // 동일한 CardItem에서 생성된 카드들은 한 턴의 사용 횟수를 공유한다.
            _cardUseCounts.TryGetValue(card, out int usedCount);
            return usedCount < card.CardMaxCounct;
        }

        public bool TryUseCard(CardItem card)
        {
            if (card == null) return false;
            CardEffectType cardEffect = card.effactType;
            if (!CanUseCardEffect(card)) return false;

            _cardUseCounts.TryGetValue(card, out int usedCount);
            _cardUseCounts[card] = usedCount + 1;
            Debug.Log($"{card.cardName} 사용 횟수: {usedCount + 1}/{card.CardMaxCounct}");

            if (cardEffect == CardEffectType.Damage)
            {
                _attackStack++;
            }
            else if (cardEffect == CardEffectType.Block)
            {
                _guardStack++;
                Debug.Log($"Guard 카드 사용: 현재 방어 스택 {_guardStack}");
            }

            _playerActionRequested = true;
            _playerObj.CardEffect(card);
            return true;
        }

        public void HitDamge(int damage)
        {
            _enemyHpbar.HitEnemy(damage);

        }

    }
}
