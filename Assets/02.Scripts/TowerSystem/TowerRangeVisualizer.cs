using UnityEngine;

public class TowerRangeVisualizer
{
    [SerializeField] private float _attackRange;
    [SerializeField] private GameObject _rangeCylinder;

    public TowerRangeVisualizer(float attackRange, GameObject rangeCylinder)
    {
        _attackRange = attackRange;
        _rangeCylinder = rangeCylinder;
        _rangeCylinder.transform.localScale = new Vector3((2.0f * _attackRange), 0.1f, (2.0f * _attackRange));

    }
    public void ShowRange()
    {
        _rangeCylinder.SetActive(true);
    }

    public void HideRange() 
    {
        _rangeCylinder.SetActive(false);
    }
    
}
