using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    private void Awake()
    {
        Instance = this; 
    }

    public void AttackToEnemy(GameObject enemy)
    {
        var enemyScript = enemy.GetComponent<EnemyBase>();
        if (enemyScript == null)
            return;

        //enemyScript.OnDamaged();
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
}