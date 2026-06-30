using DG.Tweening;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float _damage;
    [SerializeField] protected Transform _targetTransform;
    [SerializeField] protected float _projectileSpeed;
    [SerializeField] protected GameObject _areaEffectPrefab;

    public virtual void Init(float damage, Transform targetTransform, float projectileSpeed, string abilityId = "")
    {
        transform.DORotate(new Vector3(0, 360, 0), 0.3f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear).SetLink(gameObject);

        _damage = damage;
        _targetTransform = targetTransform;
        _projectileSpeed = projectileSpeed;
    }

    protected virtual void Update()
    {
        Move();
    }

    protected virtual void Move()
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
            if (_areaEffectPrefab != null)
            {
                Instantiate(_areaEffectPrefab, transform.position, Quaternion.identity);
            }

            BattleManager.Instance.AttackToEnemy(_targetTransform.gameObject, _damage);
            Destroy(gameObject);
        }
    }
}
