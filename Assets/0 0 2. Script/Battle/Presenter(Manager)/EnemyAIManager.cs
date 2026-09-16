using System.Collections;
using System.Collections.Generic;
using CardGame;
using UnityEngine;

public class EnemyAIManager : MonoBehaviour
{
    public enum AIState
    {
        Aggressive,
        Defensive,
        Neutral,
        Elite
    }

    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private EnemyView _enemyView;
    [SerializeField] private AIState _aiState = AIState.Neutral;
    [Tooltip("사용할 AI를 추가하세요. Elite는 판단 구현 전까지 제외됩니다.")]
    [SerializeField] private List<AIState> _enemyJudgmentTypes = new List<AIState>
    {
        AIState.Aggressive, AIState.Defensive, AIState.Neutral
    };

    private readonly List<EnemyJudgmentData> _enemyJudgments = new List<EnemyJudgmentData>();
    private readonly List<AIState> _availableStates = new List<AIState>();
    private readonly Queue<EnemyJudgmentData.Think> _judgmentQueue = new Queue<EnemyJudgmentData.Think>();
    [SerializeField, Min(0.1f)] private float _guardDuration = 2f;
    public int PendingJudgmentCount => _judgmentQueue.Count;
    public bool IsProcessing { get; private set; }

    public IReadOnlyList<EnemyJudgmentData> EnemyJudgments => _enemyJudgments;
    public EnemyJudgmentData EnemyAI { get; private set; }
    public EnemyJudgmentData.Think? LastJudgment { get; private set; }

    public EnemyItem CurrentEnemy { get; private set; }
    public AIState CurrentState => _aiState;

    private void Awake()
    {
        InitializeAI();

        if (_enemyView == null)
        {
            _enemyView = GetComponentInChildren<EnemyView>(true);
        }

        if (_enemyView == null || _enemyData == null
            || _enemyData.Enemys == null || _enemyData.Enemys.Length == 0)
        {
            Debug.LogWarning("EnemyData와 EnemyView 및 적 목록을 설정해주세요.", this);
            return;
        }

        CurrentEnemy = _enemyData.Enemys[Random.Range(0, _enemyData.Enemys.Length)];
        if (CurrentEnemy == null || CurrentEnemy.EnemyObject == null)
        {
            Debug.LogWarning("선택한 EnemyItem에 EnemyObject가 없습니다.", this);
            return;
        }

        _enemyView.SetEnemyObject(CurrentEnemy.EnemyObject);
    }

    public void ChangeAIState(AIState state)
    {
        EnemyJudgmentData judgment = CreateJudgment(state);
        if (judgment == null) return;

        _aiState = state;
        EnemyAI = judgment;
    }

    private EnemyJudgmentData CreateJudgment(AIState state)
    {
        switch (state)
        {
            case AIState.Aggressive:
                return new AggressiveAI();
            case AIState.Defensive:
                return new DefensiveAI();
            case AIState.Neutral:
                return new NeutralAI();
            default:
                Debug.LogWarning($"{state} AI는 판단이 구현되지 않아 사용할 수 없습니다.", this);
                return null;
        }
    }

    private void InitializeAI()
    {
        _enemyJudgments.Clear();
        _availableStates.Clear();
        if (_enemyJudgmentTypes != null)
        {
            foreach (AIState state in _enemyJudgmentTypes)
            {
                EnemyJudgmentData judgment = CreateJudgment(state);
                if (judgment == null) continue;
                _enemyJudgments.Add(judgment);
                _availableStates.Add(state);
            }
        }

        if (_enemyJudgments.Count == 0)
        {
            Debug.LogWarning("Enemy Judgment Types에 사용할 AI를 추가해주세요.", this);
            return;
        }

        int index = Random.Range(0, _enemyJudgments.Count);
        EnemyAI = _enemyJudgments[index];
        _aiState = _availableStates[index];
        Debug.Log($"적 AI 선택: {_aiState}", this);
    }

    public void ResetTurn()
    {
        _judgmentQueue.Clear();
        LastJudgment = null;
        if (_enemyView != null) _enemyView.UnGuard();
    }

    public void ExecuteAI()
    {
        BattlePlayManger battle = BattlePlayManger.Instance;
        if (battle == null || EnemyAI == null) return;

        LastJudgment = EnemyAI.Judgment(battle.AttackStack, battle.GuardStack, battle.HealStack);
        _judgmentQueue.Enqueue(LastJudgment.Value);
        Debug.Log($"적 AI [{_aiState}]: 공격 {battle.AttackStack}, 방어 {battle.GuardStack}, "
            + $"회복 {battle.HealStack} → {LastJudgment.Value}", this);

    }

    public IEnumerator ProcessJudgments()
    {
        if (IsProcessing)
        {
            yield return new WaitUntil(() => !IsProcessing);
            yield break;
        }

        IsProcessing = true;
        try
        {
            while (_judgmentQueue.Count > 0)
            {
                EnemyJudgmentData.Think judgment = _judgmentQueue.Dequeue();
                if (_enemyView == null) continue;

                _enemyView.PlayJudgment(judgment);
                if (judgment == EnemyJudgmentData.Think.Guard)
                {
                    yield return new WaitForSeconds(_guardDuration);
                    _enemyView.UnGuard();
                }

                Animator animator = _enemyView.EnemyObject != null
                    ? _enemyView.EnemyObject.GetComponentInChildren<Animator>() : null;
                yield return WaitForActionAnimation(animator);
            }
        }
        finally
        {
            IsProcessing = false;
            if (_enemyView != null) _enemyView.UnGuard();
        }
    }

    // 트리거 반영과 전환을 기다린 뒤 비반복 동작이 끝날 때까지 대기한다.
    // 반복 대기 상태나 잘못된 Animator 설정 때문에 전투가 멈추지 않도록 제한한다.
    public static IEnumerator WaitForActionAnimation(Animator animator)
    {
        yield return null;
        yield return null;
        float elapsed = 0f;
        while (animator != null && animator.isActiveAndEnabled
            && animator.runtimeAnimatorController != null && elapsed < 10f)
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
            if (!animator.IsInTransition(0) && (state.loop || state.normalizedTime >= 1f))
                yield break;

            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
