using UnityEngine;

public class PharmacistEnemy : NormalEnemy
{
    private AbilityBase _ability;

    public new void Init(string enemyId)
    {
        base.Init(enemyId);
        _ability = new HealAbility();
    }

    protected override void Die()
    {
        _ability.Employ();
        base.Die();
    }
}
