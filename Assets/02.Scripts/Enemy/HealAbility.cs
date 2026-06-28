using System.Collections;
using UnityEngine;

public class HealAbility : MountedAbility
{
    public override void Employ(string abilityId)
    {
        base.Employ(abilityId);

        GameObjectManager.Instance.CreateAbilityObject(abilityId, Vector3.zero).Forget();
    }
}