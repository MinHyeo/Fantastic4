using DG.Tweening;
using UnityEngine;

public class SaltProjectile : Projectile
{
    [SerializeField] private GameObject _burstEffect;

    private DebuffAbility _debuffAbility;

    public bool IsInitialized => _debuffAbility != null;

    public override void Init(float damage, Transform targetTransform, float projectileSpeed)
    {
        _damage = damage;
        _targetTransform = targetTransform;
        _projectileSpeed = projectileSpeed;

        // 소금 투사체는 전방 축 기준으로 회전하는 이펙트를 사용
        transform.DORotate(new Vector3(0, 0, 360), 0.3f, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.Linear)
            .SetLink(gameObject);
    }

    public void SetupSaltAbility(DebuffAbility debuffAbility)
    {
        _debuffAbility = debuffAbility;
    }

    protected override void Move()
    {
        if (_targetTransform == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, _targetTransform.position, (_projectileSpeed * Time.deltaTime));
        float distance = Vector3.Distance(_targetTransform.position, transform.position);

        if (distance < 0.1f)
        {
            BattleManager.Instance.AttackToEnemy(_targetTransform.gameObject, _damage);

            if (_debuffAbility != null)
            {
                _debuffAbility.ApplyDebuff(_targetTransform.gameObject);
            }

            if (_burstEffect != null)
            {
                Instantiate(_burstEffect, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}
