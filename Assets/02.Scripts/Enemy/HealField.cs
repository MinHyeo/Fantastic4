using System.Collections;
using UnityEngine;

public class HealField : FieldBase
{
    protected override void ApplyAbility()
    {
        StartCoroutine(CoHealingField());
    }

    private IEnumerator CoHealingField()
    {
        float activeTime = _abilityData.ActiveTime;

        while (activeTime > 0)
        {
            foreach(var entity in _onFieldEntityList)
            {
                var enemyScript = entity.GetComponent<EnemyBase>();
                if (enemyScript == null)
                    continue;

                float healValue = _abilityData.NumbericalValue;
                enemyScript.HealHealth(healValue);
            }
            yield return _tickRate;

            activeTime -= _tickTime;
        }

        GameObjectManager.Instance.ReturnObjectPool(this.gameObject);
    }
}