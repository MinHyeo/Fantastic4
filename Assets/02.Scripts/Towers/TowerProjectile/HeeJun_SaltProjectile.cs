using DG.Tweening;
using UnityEngine;

public class HeeJun_SaltProjectile : Projectile
{
    [SerializeField] private GameObject _burstEffect;

    private DebuffAbility _debuffAbility;

    public override void Initialize(float damage, Transform targetTransform, float projectileSpeed)
    {
        _damage = damage;
        _targetTransform = targetTransform;
        _projectileSpeed = projectileSpeed;

        transform.DORotate(new Vector3(0, 0, 360), 0.3f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear).SetLink(gameObject);
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