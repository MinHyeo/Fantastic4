using UnityEngine;

public abstract class DebuffBase : AbilityBase
{
    protected float _duration;
    protected float _percentValue;

    public float Duration
    {
        get
        {
            return _duration;
        }
    }

    public float PercentValue
    {
        get
        {
            return _percentValue;
        }
    }

    public override void Employ(string abilityId)
    {
        AbilityData data = GameDataManager.Instance.GetData<AbilityData>(abilityId);

        if (data != null)
        {
            _duration = data.ActiveTime;
            _percentValue = data.PercentValue;
        }
    }

    public abstract void ApplyDebuff(GameObject targetEnemy);
}
