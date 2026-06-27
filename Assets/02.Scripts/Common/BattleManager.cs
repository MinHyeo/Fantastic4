using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    public GameObject GetBeInTheLead(GameObject[] enemyArray)
    {
        if (enemyArray.Length <= 0)
            return null;

        GameObject leadEnemy = enemyArray[0];
        foreach(GameObject enemy in enemyArray)
        {
            leadEnemy = StageManager.Instance.CompareLeadEnemy(leadEnemy, enemy);
        }

        return leadEnemy;
    }
}