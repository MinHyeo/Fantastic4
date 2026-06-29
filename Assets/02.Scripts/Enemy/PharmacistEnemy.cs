using UnityEngine;

public class PharmacistEnemy : EnemyBase
{
    private AbilityBase _ability;

    public void Init(string enemyId)
    {
        base.Init(enemyId);
        _ability = new HealAbility();
    }

    protected override void Die()
    {
        _ability.Employ(_enemyData.AbilityId);
        base.Die();
    }
}
