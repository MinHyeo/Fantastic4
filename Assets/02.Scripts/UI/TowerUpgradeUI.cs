using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerUpgradeUI : UIBase
{
    private enum TowerUpgradeLocate
    {
        Before = 0,
        After = 1,
    }

    [Header("버튼")]
    [SerializeField] private UIButton Button_CloseBG;
    [SerializeField] private UIButton Button_Close;
    [SerializeField] private UIButton Button_Upgrade;

    [Header("텍스트 영역")]
    [SerializeField] private List<TextMeshProUGUI> _damageTextList = new List<TextMeshProUGUI>();
    [SerializeField] private List<TextMeshProUGUI> _attackRangeTextList = new List<TextMeshProUGUI>();
    [SerializeField] private List<TextMeshProUGUI> _attackSpeedTextList = new List<TextMeshProUGUI>();
    [SerializeField] private List<TextMeshProUGUI> _projectileSpeedTextList = new List<TextMeshProUGUI>();
    [SerializeField] private TextMeshProUGUI Text_TowerName;
    [SerializeField] private TextMeshProUGUI Text_UpgradePrice;

    private TowerBase _towerBase;

    private void OnEnable()
    {
        Button_CloseBG.BindOnClickButtonEvent(OnClickCloseTowerUpgradeUI);
        Button_Close.BindOnClickButtonEvent(OnClickCloseTowerUpgradeUI);
        Button_Upgrade.BindOnClickButtonEvent(OnClickTowerUpgrade);
    }

    private void OnClickCloseTowerUpgradeUI()
    {
        if (TowerManager.Instance != null)
        {
            TowerManager.Instance.ClearSelectedTower();
        }

        UIManager.Instance.ClosePopupUI(UIType.TowerUpgradeUI);
    }

    private string GetEntityId(string towerId)
    {
        int levelIndex = towerId.IndexOf("_Level");
        string entityId = string.Empty;
        if (levelIndex != -1)
        {
            string baseId = towerId.Substring(0, levelIndex);

            entityId = $"{baseId}_Level1";
        }

        return entityId;
    }

    public void InitUpgradeInfo(TowerBase towerbase)
    {
        _towerBase = towerbase;

        Button_Upgrade.gameObject.SetActive(true);

        var towerData = towerbase.Data;
        if (towerData == null)
        {
            return;
        }

        ChangeText(towerData, TowerUpgradeLocate.Before);

        string entityId = GetEntityId(towerData.Id);
        var entityData = GameDataManager.Instance.GetData<EntityData>(entityId);
        if (entityData == null) 
        { 
            return;
        }

        Text_TowerName.text = entityData.Name;

        if (string.IsNullOrEmpty(towerData.UpgradeId))
        {
            SetMaxUpgradeUI();
            return;
        }

        var nextTowerData = GameDataManager.Instance.GetData<TowerData>(towerData.UpgradeId);
        Debug.Log(nextTowerData);
        if (nextTowerData == null) 
        { 
            return;
        }

        Text_UpgradePrice.text = $"{towerData.UpgradePrice}";
        ChangeText(nextTowerData, TowerUpgradeLocate.After);
    }

    private void ChangeText(TowerData towerData, TowerUpgradeLocate tower)
    {
        int index = (int)tower;

        _damageTextList[index].text = $"데미지 : {towerData.AttackDamage.ToString()}";
        _attackRangeTextList[index].text = $"사거리 : {towerData.AttackRange.ToString()}";
        _attackSpeedTextList[index].text = $"공격속도 : {towerData.AttackSpeed.ToString()}";
        _projectileSpeedTextList[index].text = $"투사체속도 : {towerData.ProjectileSpeed.ToString()}";
    }

    private void SetMaxUpgradeUI()
    {
        int Index = (int)TowerUpgradeLocate.After;

        _damageTextList[Index].text = $"최대 강화";
        _attackRangeTextList[Index].text = $"최대 강화";
        _attackSpeedTextList[Index].text = $"최대 강화";
        _projectileSpeedTextList[Index].text = $"최대 강화";
        Text_UpgradePrice.text = $"최대 강화";

        Button_Upgrade.gameObject.SetActive(false);
    }

    private void OnClickTowerUpgrade()
    {
        UpdateTowerUpgradeInfo();
    }

    private void UpdateTowerUpgradeInfo()
    {
        _towerBase.Upgrade();
        InitUpgradeInfo(_towerBase);
    }
}
