using System.Collections;
using UnityEngine;

public class KetchupField : FieldBase
{
    protected override void ApplyAbility()
    {
        StartCoroutine(CoKetchupSlowed());
    }

    private IEnumerator CoKetchupSlowed()
    {
        float activeTime = _abilityData.ActiveTime;

        while (activeTime > 0)
        {
            foreach (var entity in _onFieldEntityList)
            {
                var enemyScript = entity.GetComponent<EnemyBase>();
                if (enemyScript == null)
                    continue;

                float slowPercent = _abilityData.PercentValue;
                enemyScript.ApplySlowDebuff(slowPercent);
            }
            yield return _tickRate;

            activeTime -= _tickTime;
        }

        GameObjectManager.Instance.ReturnObjectPool(this.gameObject);
    }

}