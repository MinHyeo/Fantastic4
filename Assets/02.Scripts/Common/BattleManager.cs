using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    public GameObject GetBeInTheLead(Collider[] enemyArray)
    {
        if (enemyArray.Length <= 0)
            return null;

        GameObject leadEnemy = enemyArray[0].gameObject;
        foreach (var enemyCollider in enemyArray)
        {
            leadEnemy = StageManager.Instance.CompareLeadEnemy(leadEnemy, enemyCollider.gameObject);
        }

        return leadEnemy;
    }
}