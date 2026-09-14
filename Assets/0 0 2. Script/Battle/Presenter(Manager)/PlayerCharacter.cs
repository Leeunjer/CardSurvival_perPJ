using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    

public class PlayerCharacter : MonoBehaviour , IICardCommand
{
    PlayerView _playerView;

    private Queue<IEnumerator> _commandQueue = new();

    private bool _isProcessing = false;
    public bool IsProcessing => _isProcessing;

    private GameObject _PlayerCharactor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetPlayerCharactor(PlayerDataManager.Instance.currentPlayer.playerPrefab);
        GameObject PlayerChractor = Instantiate(_PlayerCharactor, gameObject.transform);
        PlayerChractor.transform.localPosition = new Vector3(0,0,0);
        PlayerChractor.transform.rotation = Quaternion.Euler(0,-90,0);
        _playerView = gameObject.GetComponentInChildren<PlayerView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayerCharactor(GameObject playerCharactor)
        {
            _PlayerCharactor = playerCharactor;
        }

    private void AddCommand(IEnumerator command)
        {
            // 카드 사용 시에는 저장하고, 턴 종료 시 한 번에 실행한다.
            _commandQueue.Enqueue(command);
        }

    public IEnumerator ProcessCommand()
        {
            if (_isProcessing)
            {
                yield return new WaitUntil(() => !_isProcessing);
                yield break;
            }

            _isProcessing = true;
            try
            {
                while (_commandQueue.Count > 0)
                {
                    yield return _commandQueue.Dequeue();
                }
            }
            finally
            {
                _isProcessing = false;
            }
        }
    public void CardEffect(CardEffectType cardEffect)
    {
        
        switch (cardEffect)
            {
                case CardEffectType.Damage :
                // 첫 공격 카드만 명령을 등록하고, 실행 시 최종 누적치를 사용한다.
                if (BattlePlayManger.Instance.AttackStack == 1)
                {
                    AddCommand(Attack());
                }
                break;
                    
                case CardEffectType.Heal:
                AddCommand(Heal());
                break;


                case CardEffectType.Draw:
                AddCommand(Draw());
                break;


                case CardEffectType.Block:
                // 방어 카드를 여러 장 사용해도 턴 종료 명령은 한 번만 등록한다.
                if (BattlePlayManger.Instance.GuardStack == 1)
                {
                    AddCommand(Gaurd());
                }
                break;

            }
    }

    

    private IEnumerator Draw()
    {
        HandManager.Inst.AddCard();
        yield return null;
    }

    private IEnumerator Heal()
    {
        Debug.Log("Heal");
        _playerView.BUFF();
        yield return null;
    }

    private IEnumerator Attack()
    {
        int attackStack = BattlePlayManger.Instance.AttackStack;
        switch (attackStack)
        {
            case 1:
                _playerView.Attack1();
                break;
            case 2:
                _playerView.Attack2();
                break;
            case 3:
                _playerView.Attack3();
                break;
            default:
                yield break;
        }

        // 카드 한 장당 기존 피해량 20을 유지한다.
        BattlePlayManger.Instance.HitDamge(20 * attackStack);
        yield return null;
    }

    IEnumerator Gaurd()
        {
            Debug.Log($"Guard 실행: 누적 방어 스택 {BattlePlayManger.Instance.GuardStack}");
            _playerView.OnGuard();
            yield return new WaitForSeconds(2f);
            _playerView.UnGuard();
            yield return new WaitForSeconds(2f);

        }
}

}
