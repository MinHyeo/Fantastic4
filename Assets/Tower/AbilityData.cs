using System;
using UnityEngine;

[Serializable]
public class AbilityData : GameDataBase
{
    // 특수 능력 Id

    public string Id;
    // 효과 (소금 타워의 받는 피해 증가 %, 케첩 타워의 이속 감소 % 등)
    public float PrecentValue;

    // 수치 효과 (기름 타워의 지속 데미지 수치 등)
    public float NumbericalValue;

    // 디버프 지속 시간 (초)
    public float ActiveTime;

    // 특수능력 범위
    public float EffectRound;
}