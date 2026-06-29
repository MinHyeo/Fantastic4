using UnityEngine;

public class PharmcistEnemy : EnemyBase
{
    private AbilityBase _ability;

    public override void Init(string enemyId)
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
