using System.Collections;
using UnityEngine;

public class OilField : FieldBase
{
    protected override void ApplyAbility()
    {
        StartCoroutine(CoOilDamaged());
    }

    private IEnumerator CoOilDamaged()
    {
        float activeTime = _abilityData.ActiveTime;

        while (activeTime > 0)
        {
            foreach (var entity in _onFieldEntityList)
            {
                float damageValue = _abilityData.NumbericalValue;
                BattleManager.Instance.AttackToEnemy(entity, damageValue);
            }
            yield return _tickRate;

            activeTime -= _tickTime;
            Debug.Log(activeTime);
        }

        GameObjectManager.Instance.ReturnObjectPool(this.gameObject);
    }
}