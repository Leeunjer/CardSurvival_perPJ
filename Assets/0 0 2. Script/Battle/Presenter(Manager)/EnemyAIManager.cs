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

    public EnemyItem CurrentEnemy { get; private set; }
    public AIState CurrentState => _aiState;

    private void Awake()
    {
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
        _aiState = state;
    }

    public void ExecuteAI()
    {
        switch (_aiState)
        {
            case AIState.Aggressive:
                AggressiveAI();
                break;
            case AIState.Defensive:
                DefensiveAI();
                break;
            case AIState.Neutral:
                NeutralAI();
                break;
            case AIState.Elite:
                EliteAI();
                break;
        }
    }

    private void AggressiveAI() { }
    private void DefensiveAI() { }
    private void NeutralAI() { }
    private void EliteAI() { }
}
