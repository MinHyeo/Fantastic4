using System;
using UnityEngine;

/// <summary>
/// 타워의 기본 스탯과 리소스 주소 데이터를 담는 클래스
/// </summary>
[Serializable]
public class TowerData : GameDataBase
{
    /// <summary>
    /// 타워 ID
    /// </summary>
    public string Id;

    /// <summary>
    /// 타워 데미지
    /// </summary>
    public float AttackDamage;

    /// <summary>
    /// 타워 공격 범위
    /// </summary>
    public float AttackRange;

    /// <summary>
    /// 타워 공격 속도
    /// </summary>
    public float AttackSpeed;

    /// <summary>
    /// 타워 투사체 속도
    /// </summary>
    public float ProjectileSpeed;

    /// <summary>
    /// 특수능력 ID
    /// </summary>
    public string AbilityId;

    /// <summary>
    /// 건설 비용
    /// </summary>
    public int BuildPrice;

    /// <summary>
    /// 다음 단계 업그레이드 시 참조할 타워 ID
    /// </summary>
    public string UpgradeId;

    /// <summary>
    /// 다음 단계 업그레이드 비용
    /// </summary>
    public int UpgradePrice;

    /// <summary>
    /// 타워 프리팹 주소
    /// </summary>
    public string PrefabPath;

    /// <summary>
    /// 투사체 프리팹 주소
    /// </summary>
    public string ProjectilePath;
}