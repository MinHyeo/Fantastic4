using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    private void Awake()
    {
        Instance = this; 
    }

    public void AttackToEnemy(GameObject enemy, float damage)
    {
        var enemyScript = enemy.GetComponent<EnemyBase>();
        if (enemyScript == null)
            return;

        enemyScript.OnDamaged(damage);
    }

    public GameObject GetBeInTheLead(Collider[] enemyArray)
    {
        if (enemyArray.Length <= 0)
            return null;

        GameObject leadEnemy = enemyArray[0].gameObject;
        foreach(var enemyCollider in enemyArray)
        {
            leadEnemy = StageManager.Instance.CompareLeadEnemy(leadEnemy, enemyCollider.gameObject);
        }

        return leadEnemy;
    }

    public void HealToEnemy(GameObject enemy, float healValue)
    {
        var enemyScript = enemy.GetComponent<EnemyBase>();
        if (enemyScript == null)
            return;

        enemyScript.HealHealth(healValue);
    }

    public void DamageAmplificationToEnemy(GameObject enemy, float damageBonus, float duration)
    {
        var enemyScript = enemy.GetComponent<EnemyBase>();
        if (enemyScript == null)
            return;

        enemyScript.ApplyDamageAmplificationDebuff(damageBonus, duration);
    }


    public void SlowToEnemy(GameObject enemy, float slowPercent, float duraction)
    {
        var enemyScript = enemy.GetComponent<EnemyBase>();
        if (enemyScript == null)
            return;

        enemyScript.ApplySlowDebuff(slowPercent);
    }
}