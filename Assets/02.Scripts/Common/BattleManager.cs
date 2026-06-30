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

    // 소금 디버프
    public void ApplySaltBonusDamage(GameObject enemy, float bonusDamage)
    {
        var enemyScript = enemy.GetComponent<EnemyBase>();

        if (enemyScript == null)
        {
            return;
        }

        enemyScript.OnDamaged(bonusDamage);
    }
}