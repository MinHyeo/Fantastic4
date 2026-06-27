using UnityEngine;

public class SaltProjectile : Projectile
{
    private DebuffAbility _debuffAbility;

    public bool IsInitialized => _debuffAbility != null;

    public new void Initialize(float damage, Transform targetTransform, float projectileSpeed)
    {
        // 부모의 Initialize를 실행하여 _targetTransform을 안전하게 채우기
        base.Initialize(damage, targetTransform, projectileSpeed);
    }

    public void SetupSaltAbility(DebuffAbility debuffAbility)
    {
        _debuffAbility = debuffAbility;
    }

    private void Update()
    {
        if (_targetTransform == null)
        {
            return;
        }

        // 데이터가 유효하고 적이 살아있을 때만 부모의 이동(Move) 및 기본 피격 연산 실행
        base.Update();

        // 적이 필드 상에서 안전하게 살아있을 때만 거리 계산 진행
        float distance = Vector3.Distance(_targetTransform.position, transform.position);

        if (distance < 0.1f)
        {
            if (_debuffAbility == null)
            {
                return;
            }

            _debuffAbility.ApplyDebuff(_targetTransform.gameObject);
        }
    }
}