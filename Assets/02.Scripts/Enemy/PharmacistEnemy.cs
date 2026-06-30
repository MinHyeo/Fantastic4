using UnityEngine;

public class PharmcistEnemy : EnemyBase
{
    private AbilityBase _ability;

    public override void Init(int instanceId, string enemyId)
    {
        base.Init(instanceId, enemyId);
        _ability = new HealAbility();
    }

    protected override void Die()
    {
        _ability.Employ(_enemyData.AbilityId, transform);
        base.Die();
    }
}
