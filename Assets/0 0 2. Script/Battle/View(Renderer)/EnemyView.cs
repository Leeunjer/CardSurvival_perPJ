using UnityEngine;

public class EnemyView : MonoBehaviour
{
    [SerializeField] private string _attackTrigger = "Attack1";
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
        if (_animator != null) _animator.SetTrigger(_attackTrigger);
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
