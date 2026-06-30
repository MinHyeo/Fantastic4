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
    private float _slowPercent = 0; //이속감소 비율

    [SerializeField] private float _arriveDistance = 0.4f; // 도착판정범위
    [SerializeField] private float _rotateSpeed = 360f; // 초당 회전 각도
    [SerializeField] private float _slowDebuffBufferTime = 1.3f; // 대충 1초(틱)보다 살짝 길게

    private Vector3 _targetPosition;
    private bool _isDead = false;
    public bool IsDead => _isDead;

    public virtual void Init(string enemyId)
    {
        _animator = GetComponent<Animator>();
        EnemyData enemyData = GameDataManager.Instance.GetData<EnemyData>(enemyId);
        if (enemyData == null)
        {
            return;
        }

        _isDead = false;
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

    protected virtual void FixedUpdate()
    {
        if (_isDead == true)
            return;

        RotateTowardsTarget(_targetPosition);
        MoveToPosition(_targetPosition);
    }

    protected void MoveToPosition(Vector3 targetPosition)
    {
        float slowedMoveSpeed = _enemyData.MoveSpeed * (1f - (_slowPercent / 100f));
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, (slowedMoveSpeed * Time.deltaTime));
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
        if (_isDead == true)
            return;

        _currentHp -= (damage + _damageBonus);
        if (_currentHp <= 0f)
        {
            Die();
            StageManager.Instance.IncreseGold(_enemyData.RewardGold);
        }
    }

    public void ApplyDamageAmplificationDebuff(float damageBonusAmount, float duration)
    {
        if (_isDamageAmplified == true)
        {
            return;
        }

        _damageBonus = damageBonusAmount;
        _isDamageAmplified = true;

        _damageBonus = damageBonusAmount;
        _isDamageAmplified = true;
     
        Invoke(nameof(ResetDamageAmplificationDebuff), duration);
    }

    private void ResetDamageAmplificationDebuff()
    {
        _damageBonus = 0f;
        _isDamageAmplified = false;
    }

    public void ApplySlowDebuff(float slowPercent)
    {
        _slowPercent = slowPercent;

        CancelInvoke(nameof(ResetSlowDebuff));
        Invoke(nameof(ResetSlowDebuff), _slowDebuffBufferTime);
    }

    private void ResetSlowDebuff()
    {
        _slowPercent = 0f;
    }

    public void HealHealth(float healValue)
    {
        if(_isDead == true)
        {
            return;
        }

        _currentHp += healValue;
        if(_currentHp > _enemyData.MaxHp)
        {
            _currentHp = _enemyData.MaxHp;
        }
    }

    protected virtual void Die()
    {
        _animator.SetBool("IsMove", false);
        _animator.SetTrigger("IsDead");

        //GameObjectManager.Instance.ReturnObjectPool(gameObject);
        _isDead = true;
        StageManager.Instance.RemoveActivatedEnemy();

        Invoke("OnDeathAnimationComplete", 5f);   
    }

    protected void OnDeathAnimationComplete()
    {
        GameObjectManager.Instance.ReturnObjectPool(gameObject);
    }
}
