using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    protected int _courseIndex;
    public int CourseIndex
    {
        get { return _courseIndex; }
    }

    protected float _maxHp;
    protected float _currentHp;

    public void OnDamaged(float damage)
    {
        _currentHp -= damage;
        if (_currentHp <= 0f)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        GameObjectManager.Instance.ReturnObjectPool(gameObject);
    }
}
