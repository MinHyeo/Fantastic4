using System.Collections;
using UnityEngine;

public class HealField : MonoBehaviour
{
    private float duration = 5f;
    private float tickRate = 1f;
    private float healSize = 1f;

    private WaitForSeconds tickSeconds;

    private void OnEnable()
    {
        Destroy(this.gameObject, duration);
        tickSeconds = new WaitForSeconds(tickRate);
        StartCoroutine(CoTickHealing());
    }

    private IEnumerator CoTickHealing()
    {
        while (true)
        {
            Collider[] targets = Physics.OverlapSphere(transform.position, 3f, LayerMask.GetMask("Enemy"));
            foreach(var target in targets)
            {
                // 힐 적용
                Debug.Log("힐 중");
            }
            yield return tickSeconds;
        }
    }
}