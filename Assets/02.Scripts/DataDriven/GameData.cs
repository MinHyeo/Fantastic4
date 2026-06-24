using System;
using System.Collections.Generic;

[System.Serializable]
public class GameDataBase
{
    public string Id;
}

[System.Serializable]
public class EntityData : GameDataBase
{
    public string Name;
    public string Description;
    public string EntityType;
    public string IconPath;
}

[System.Serializable]
public class TowerData : GameDataBase
{
    public float AttackDamage;
    public float AttackRange;
    public float AttackSpeed;
    public float ProjectileSpeed;
    public string AbilityId;
    public int BuildPrice;
    public string UpgradeId;
    public string UpgradePrice;
    public string PrefabPath;
    public string ProjectilePath;
}

[System.Serializable]
public class EnemyData : GameDataBase
{
    public float MaxHp;
    public float MoveSpeed;
    public string AbilityId;
    public int RewardGold;
    public string PrefabPath;
}

//[System.Serializable] 
//public class AbilityData : GameDataBase
//{
//    public float PrecentValue;
//    public float NumbericalValue;
//    public float ActiveTime;
//    public float EffectRound;
//}

[System.Serializable]
public class StageData : GameDataBase
{
    public int MaxLife;
    public int StartGold;
    public string[] WaveId;
    public string PrefabPath;
}

[System.Serializable]
public class WaveData : GameDataBase
{
    public int WaveGroup;
    public string EnemyId;
    public int Count;
    public float Interval;
    public float PreDelay;
}

// ===============임시로 필요한 코드======================
[System.Serializable]
public class ItemData : GameDataBase
{
    public string Name;
    public string Description;
    public string ItemType;
    public string Grade;
    public string MaxStackCount;
    public string SellingPrice;
    public string IconPath;
}

[System.Serializable]
public class DialogueGroupData : GameDataBase
{
    public List<string> DialogueIdList;
}

[System.Serializable]
public class DialogueData : GameDataBase
{
    public string CharacterDataId;
    public string Description;
    public string NextDialogueId;
    public List<string> SelectionNameList;
    public List<string> SelectionDialogueIdList;
    public string TexturePath;
    public string VoicePath;
}

[System.Serializable]
public class FieldObjectData : GameDataBase
{
    public string Name;
    public string Description;
    public string FieldObjectType;
    public List<int> DropCountRange;
    public string DropItemDataId;
    public string IconPath;
    public string PrefabPath;
}

[System.Serializable]
public class MonsterData : GameDataBase
{
    public string Name;
    public string Description;
    public string IconPath;
    public string PrefabPath;
}

[System.Serializable]
public class CharacterData : GameDataBase
{
    public string Name;
    public string SkillList;
    public string UseWeaponId;
    public string BasicCostumeId;
}

[System.Serializable]
public class SkillData : GameDataBase
{
    public string Name;
    public string Description;
}

[System.Serializable]
public class WeaponData : GameDataBase
{
    public string Name;
    public string Description;
}

[System.Serializable]
public class CostumeData : GameDataBase
{
    public string Name;
    public string Description;
}