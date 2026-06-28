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
                var enemyScript = entity.GetComponent<EnemyBase>();
                if (enemyScript == null)
                    continue;

                float damageValue = _abilityData.NumbericalValue;
                enemyScript.OnDamaged(damageValue);
            }
            yield return _tickRate;

            activeTime -= _tickTime;
        }

        GameObjectManager.Instance.ReturnObjectPool(this.gameObject);
    }
}