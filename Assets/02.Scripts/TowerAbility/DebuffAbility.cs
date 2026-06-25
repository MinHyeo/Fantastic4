using UnityEngine;

public abstract class DebuffAbility : TowerAbilityBase
{
    protected float _duration;
    protected float _percentValue;

    // 몬스터에게 특정 메시지와 수치, 지속시간을 안전하게 전달하는 공용 함수
    protected void SendDebuffToEnemy(GameObject enemyTarget, string methodName)
    {
        if (enemyTarget == null) return;

        object[] parameters = new object[] { _percentValue, _duration };
        enemyTarget.SendMessage(methodName, parameters, SendMessageOptions.DontRequireReceiver);

        Debug.Log($"[{gameObject.name}] {enemyTarget.name}에게 {methodName} 디버프 전송 완료 (값: {_percentValue}, 시간: {_duration})");
    }

    // 필요 시 디버프 이펙트(이펙트 프리팹 등)를 생성해주는 공용 기능 확장 가능
    protected void PlayDebuffVisual(GameObject enemyTarget, GameObject effectPrefab)
    {
        if (effectPrefab == null || enemyTarget == null) return;
        Instantiate(effectPrefab, enemyTarget.transform.position, enemyTarget.transform.rotation, enemyTarget.transform);
    }

    // 외부(투사체 등)에서 디버프를 발동시키기 위해 호출할 최종 실행 인터페이스
    public abstract void ApplyDebuff(GameObject enemyTarget);
}
