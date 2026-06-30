using System.Collections.Generic;
using UnityEngine;

public abstract class FieldBase : MonoBehaviour
{
    protected AbilityData _abilityData;
    protected List<GameObject> _onFieldEntityList;

    protected float _tickTime = 1f;
    protected WaitForSeconds _activeTime;
    protected WaitForSeconds _tickRate;

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Entity"))
        {
            if (other == null)
                return;

            _onFieldEntityList.Add(other.gameObject);
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Entity"))
        {
            if (other == null)
                return;

            _onFieldEntityList.Remove(other.gameObject);
        }
    }

    public void Init(AbilityData abilityData)
    {
        _abilityData = abilityData;

        _activeTime = new WaitForSeconds(_abilityData.ActiveTime);
        _tickRate = new WaitForSeconds(_tickTime);
    }

    public void ActiveField()
    {
        ApplyAbility();
    }

    protected abstract void ApplyAbility();
}