using System.Collections;
using UnityEngine;

public class KetchupAbility : FieldAbility
{
    public override void Employ(string abilityId, Transform spawnSpot = null)
    {
        base.Employ(abilityId);

        Vector3 spawnPostion = spawnSpot.position + (Vector3.up * 0.1f);
        GameObjectManager.Instance.CreateAbilityObject(abilityId, spawnPostion).Forget();
    }
}