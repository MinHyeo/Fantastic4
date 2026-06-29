using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    protected int _courseIndex;
    public int CourseIndex
    {
        get { return _courseIndex; }
    }

    protected Animator _animator;
    protected EnemyData _enemyData;
    protected float _currentHp;
    protected float _damageBonus;
    protected bool _isDamageAmplified;

    [SerializeField] private float _arriveDistance = 0.4f; // 도착판정범위
    [SerializeField] private float _rotateSpeed = 360f; // 초당 회전 각도

    private Vector3 _targetPosition;

    public void Init(string enemyId)
    {
        _animator = GetComponent<Animator>();
        EnemyData enemyData = GameDataManager.Instance.GetData<EnemyData>(enemyId);
        if (enemyData == null)
        {
            return;
        }

        _enemyData = enemyData;
        _currentHp = _enemyData.MaxHp;
        _damageBonus = 0f;
        _targetPosition = StageManager.Instance.GetCoursePosition(_courseIndex);

        bool isCourseEnd = StageManager.Instance.CheckEndCourse(_courseIndex);
        if (isCourseEnd == true)
        {
            return;
        }

        _animator.SetBool("IsMove", true); 
    }

    protected void Update()
    {
        CheckArriveAndSetNextTarget();
    }

    protected void FixedUpdate()
    {
        if (_targetPosition == null)
            return;

        RotateTowardsTarget(_targetPosition);
        MoveToPosition(_targetPosition);
    }

    protected void MoveToPosition(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, (_enemyData.MoveSpeed * Time.deltaTime));
    }

    protected void RotateTowardsTarget(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0f; //y 무시

        if (direction.sqrMagnitude < 0.0001f)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, (_rotateSpeed * Time.deltaTime));
    }

    protected void CheckArriveAndSetNextTarget()
    {
        Vector3 pos1 = transform.position;
        Vector3 pos2 = _targetPosition;

        //3D > 2D로 변환, 그래서 Y값이 없음
        float diffX = pos1.x - pos2.x;
        float diffZ = pos1.z - pos2.z;

        float distance = (diffX * diffX) + (diffZ * diffZ); //2D환경으로 변환 후 피타고라스로 거리 계산
        float arriveDistance = _arriveDistance * _arriveDistance; //거리도 제곱으로 형식 맞춰줌 > 루트 씌우는 것보다 제곱이 연산이 가볍다
        if (distance > arriveDistance)
        {
            return;
        }

        _courseIndex++;

        bool isCourseEnd = StageManager.Instance.CheckEndCourse(_courseIndex);
        if (isCourseEnd == true)
        {
            return;
        }

        _targetPosition = StageManager.Instance.GetCoursePosition(_courseIndex);
    }

    public void OnDamaged(float damage)
    {
        _currentHp -= (damage + _damageBonus);
        if (_currentHp <= 0f)
        {
            Die();
        }
    }

    public void ApplyDamageAmplificationDebuff(object[] parameters)
    {
        if (_isDamageAmplified == true)
        {
            return;
        }

        float damageBonusAmount = (float)parameters[0];
        float duration = (float)parameters[1];

        _damageBonus = damageBonusAmount;
        _isDamageAmplified = true;

        Invoke(nameof(ResetDamageAmplificationDebuff), duration);
    }

    private void ResetDamageAmplificationDebuff()
    {
        _damageBonus = 0f;
        _isDamageAmplified = false;
    }

    protected virtual void Die()
    {
        _animator.SetBool("IsMove", false);
        _animator.SetTrigger("IsDead");
        //GameObjectManager.Instance.ReturnObjectPool(gameObject);
        //StageManager에서 해야하는 일
    }
}
