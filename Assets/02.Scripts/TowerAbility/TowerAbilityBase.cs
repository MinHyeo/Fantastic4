using UnityEngine;

public abstract class TowerAbilityBase : MonoBehaviour
{
    public string AbilityId { get; protected set; }

    // 자식에서 오버라이드하여 데이터 매니저로부터 데이터를 로드하는 진입점
    public abstract void Initialize(string abilityId);
}
