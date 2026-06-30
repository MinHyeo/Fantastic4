using UnityEngine;

public class SaltDebuff : DebuffBase
{
    private GameObject _targetEnemy;
    private float _durationTimer;
    private bool _isDebuffActive;

    public bool IsDebuffActive
    {
        get
        {
            return _isDebuffActive;
        }
    }

    public override void ApplyDebuff(GameObject targetEnemy)
    {
        if (targetEnemy == null)
        {
            return;
        }

        _targetEnemy = targetEnemy;
        _durationTimer = _duration;
        _isDebuffActive = true;

        float baseBonusFactor = 10.0f;
        float finalSaltBonusDamage = baseBonusFactor * _percentValue;

        BattleManager.Instance.ApplySaltBonusDamage(targetEnemy, finalSaltBonusDamage);
    }

    public void UpdateDebuff(float deltaTime)
    {
        if (!_isDebuffActive)
        {
            return;
        }

        _durationTimer -= deltaTime;

        if (_durationTimer <= 0.0f)
        {
            _isDebuffActive = false;
            _targetEnemy = null;
        }
    }
}