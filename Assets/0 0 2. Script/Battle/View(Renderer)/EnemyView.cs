using UnityEngine;

public class EnemyView : MonoBehaviour
{
    [SerializeField] private string _attackTrigger = "Attack";
    [SerializeField] private string _healTrigger = "BUFF";
    [SerializeField] private string _hitTrigger = "Hit";
    [SerializeField] private string _guardParameter = "Guard";

    private Animator _animator;
    public GameObject EnemyObject { get; private set; }

    public void SetEnemyObject(GameObject enemyPrefab)
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("생성할 적 프리팹이 없습니다.", this);
            return;
        }

        if (EnemyObject != null)
        {
            EnemyObject.SetActive(false);
            Destroy(EnemyObject);
        }

        EnemyObject = Instantiate(enemyPrefab, transform);
        EnemyObject.transform.localPosition = Vector3.zero;
        EnemyObject.transform.localRotation = Quaternion.Euler(0,90,0);
        _animator = EnemyObject.GetComponentInChildren<Animator>(true);

        if (_animator == null)
        {
            Debug.LogWarning("생성한 적 오브젝트에 Animator가 없습니다.", this);
        }
    }

    public void Attack()
    {
        UnGuard();
        // 기존 씬에 저장된 Attack1 설정도 현재 적 컨트롤러의 Attack에 연결한다.
        string trigger = _attackTrigger;
        if (!HasParameter(trigger, AnimatorControllerParameterType.Trigger)
            && trigger == "Attack1")
            trigger = "Attack";
        PlayTrigger(trigger);
    }

    public void PlayJudgment(EnemyJudgmentData.Think judgment)
    {
        switch (judgment)
        {
            case EnemyJudgmentData.Think.Attack:
                Attack();
                break;
            case EnemyJudgmentData.Think.Guard:
                OnGuard();
                break;
            case EnemyJudgmentData.Think.Heal:
                UnGuard();
                PlayTrigger(_healTrigger);
                break;
        }
    }

    private bool HasParameter(string parameterName, AnimatorControllerParameterType type)
    {
        if (_animator == null || string.IsNullOrEmpty(parameterName)) return false;
        foreach (AnimatorControllerParameter parameter in _animator.parameters)
        {
            if (parameter.name == parameterName && parameter.type == type) return true;
        }
        return false;
    }

    private void PlayTrigger(string trigger)
    {
        if (_animator == null) return;
        if (HasParameter(trigger, AnimatorControllerParameterType.Trigger))
            _animator.SetTrigger(trigger);
        else
            Debug.LogWarning($"적 Animator에 {trigger} Trigger가 없습니다.", this);
    }

    public void Hit()
    {
        if (_animator != null) _animator.SetTrigger(_hitTrigger);
    }

    public void OnGuard()
    {
        if (_animator != null) _animator.SetBool(_guardParameter, true);
    }

    public void UnGuard()
    {
        if (_animator != null) _animator.SetBool(_guardParameter, false);
    }
}
