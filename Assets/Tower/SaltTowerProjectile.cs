using UnityEngine;

public class SaltTowerProjectile : Projectile
{
    private AbilityData _saltAbility;
    private Transform _target;

    public bool IsInitialized => _saltAbility != null;

    public new void Initialize(float damage, Transform targetTransform, float projectileSpeed)
    {
        base.Initialize(damage, targetTransform, projectileSpeed);

        // 디버프를 멕이기 위해 타겟 적의 주소를 보관
        _target = targetTransform;
    }

    public void SetupSaltAbility(AbilityData abilityData)
    {
        _saltAbility = abilityData;
    }

    private void Update()
    {
        if (_target == null) return;

        float distance = Vector3.Distance(_target.position, transform.position);
        if (distance < 0.1f)
        {
            if (_saltAbility != null)
            {
                // 소금 디버프 메서드를 타겟 오브젝트에 직접 트리거 (받는 피해 증가 %, 지속시간)
                object[] parameters = new object[] { _saltAbility.PrecentValue, _saltAbility.ActiveTime };
                _target.gameObject.SendMessage("ApplyDamageAmplificationDebuff", parameters, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}