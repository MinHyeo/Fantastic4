using UnityEngine;
using DG.Tweening;

public class HeeJun_PepperProjectile : Projectile
{
    [SerializeField] private GameObject _burstEffect;

    public override void Initialize(float damage, Transform targetTransform, float projectileSpeed)
    {
        _damage = damage;
        _targetTransform = targetTransform;
        _projectileSpeed = projectileSpeed;

        transform.DORotate(new Vector3(0, 0, 360), 0.3f, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.Linear)
            .SetLink(gameObject);
    }

    protected override void Update()
    {
        Move();
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
            if (distance < 0.5f)
            {
                if (_burstEffect != null)
                {
                    Instantiate(_burstEffect, transform.position, Quaternion.identity);  
                }
                Destroy(gameObject);
            }
            // TODO: 피격 파편 효과
            Destroy(gameObject);
        }
    }
}