using UnityEngine;

public abstract class AbilityBase
{
    protected string _targetType;

    public abstract void Employ(string abilityId, Transform spawnSpot = null);
}