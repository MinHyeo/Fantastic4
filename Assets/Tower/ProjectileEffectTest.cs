using UnityEngine;

public class ProjectileEffectTest : MonoBehaviour
{
    
    
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _target;   // 임시 타겟 (Cube 등)
    [SerializeField] private float _fireInterval = 1f;
    private float _lastFireTime;

    

    private void Update()
    {
        if (Time.time - _lastFireTime < _fireInterval)
        {
            return;
        }
        _lastFireTime = Time.time;

        GameObject proj = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
        Projectile p = proj.GetComponent<Projectile>();
        p.Initialize(10f, _target, 3f);   // 데미지, 타겟, 속도
    }

}
