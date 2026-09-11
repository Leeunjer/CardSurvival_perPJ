using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace CardGame
{
    

public class PlayerCharacter : MonoBehaviour , IICardCommand
{
    PlayerView _playerView;

    private Queue<IEnumerator> _commandQueue = new();

    private bool _isProcessing = false;

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
            _commandQueue.Enqueue(command);

            if (!_isProcessing)
            {
                StartCoroutine(ProcessCommand());
            }
        }

    private IEnumerator ProcessCommand()
        {
            _isProcessing = true;

            while (_commandQueue.Count > 0)
            {
                yield return StartCoroutine(_commandQueue.Dequeue());
            }

            _isProcessing = false;
        }
    public void CardEffect(CardEffectType cardEffect)
    {
        
        switch (cardEffect)
            {
                case CardEffectType.Damage :
                AddCommand(Attack());
                break;
                    
                case CardEffectType.Heal:
                AddCommand(Heal());
                break;


                case CardEffectType.Draw:
                Draw();
                break;


                case CardEffectType.Block:
                AddCommand(Gaurd());
                break;

            }
    }

    

    private void Draw()
    {
        HandManager.Inst.AddCard();
    }

    private IEnumerator Heal()
    {
        Debug.Log("Heal");
        _playerView.BUFF();
        yield return null;
    }

    private IEnumerator Attack()
    {
        Debug.Log("Attack1");
        _playerView.Attack1();
        BattlePlayManger.Instance.HitDamge(20);
        yield return null;
    }

    IEnumerator Gaurd()
        {
            _playerView.OnGuard();
            yield return new WaitForSeconds(2f);
            _playerView.UnGuard();
            yield return new WaitForSeconds(2f);

        }
}

}
