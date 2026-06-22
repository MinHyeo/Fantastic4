using UnityEngine;

public class SaltDebuff : DebuffAbility
{
    // 자식 단계에서 비로소 데이터 매니저에 접근해 세부 수치를 로드
    public override void Initialize(string abilityId)
    {
        AbilityId = abilityId;

        AbilityData data = GameDataManager.Instance.GetData<AbilityData>(abilityId);

        if (data != null)
        {
            _duration = data.ActiveTime;
            _percentValue = data.PrecentValue;
        }
    }

    // 부모의 세부화된 기능을 활용해 소금 타워의 고유 기믹만 수행
    public override void ApplyDebuff(GameObject enemyTarget)
    {
        // 부모의 기능을 활용해 소금 타워 전용 몬스터 메서드 이름을 넘겨줌
        SendDebuffToEnemy(enemyTarget, "ApplyDamageAmplificationDebuff");
    }
}