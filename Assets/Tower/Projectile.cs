using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private GameObject _areaEffectPrefab;

    public void Initialize(float damage, Transform targetTransform, float projectileSpeed)
    {
        _damage = damage;
        _targetTransform = targetTransform;
        _projectileSpeed = projectileSpeed;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetTransform.position, (_projectileSpeed * Time.deltaTime));
        float distance = Vector3.Distance(_targetTransform.position, transform.position);
        if (distance < 0.1f)
        {
            if (_areaEffectPrefab != null)
            {
                Instantiate(_areaEffectPrefab, transform.position, Quaternion.identity);
            }
            Debug.LogWarning("투사체 피격"); // TODO 희준 TakeDamage 필요
            Destroy(gameObject);
        }
    }
}
