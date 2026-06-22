using UnityEngine;

public class SlowEffect : MonoBehaviour, IDebuffEffect
{
    public void ApplyEffect(Transform target)
    {
        Debug.LogWarning($"{target.name}에 슬로우 적용");
    }
}
