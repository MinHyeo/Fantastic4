using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerUpgradeUI : UIBase
{
    private enum TowerUpgradeState
    {
        Before = 0,
        After = 1,
    }

    [Header("버튼")]
    [SerializeField] private UIButton Button_CloseBG;
    [SerializeField] private UIButton Button_Close;
    [SerializeField] private UIButton Button_Upgrade;

    [Header("텍스트 영역")]
    [SerializeField] private List<TextMeshProUGUI> _damgeTextList = new List<TextMeshProUGUI>();
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
        UIManager.Instance.ClosePopupUI(UIType.TowerUpgradeUI);
    }

    public void InitUpgradeInfo(TowerBase towerbase)
    {
        _towerBase = towerbase;

        var towerData = towerbase.Data;
        if (towerData == null)
        {
            return;
        }

        Text_UpgradePrice.text = $"{towerData.UpgradePrice}";
        ChangeText(towerData, TowerUpgradeState.Before);

        var entityData = GameDataManager.Instance.GetData<EntityData>(towerData.Id);
        if (entityData == null) 
        { 
            return; 
        }
        Text_TowerName.text = entityData.Name;

        string nextTowerId = towerData.UpgradeId;
        var nextTowerData = GameDataManager.Instance.GetData<TowerData>(nextTowerId);
        if (nextTowerData == null)
        {
            MaxUpgradeLeve();
            return;
        }

        ChangeText(nextTowerData, TowerUpgradeState.After);
    }

    private void ChangeText(TowerData towerData, TowerUpgradeState tower)
    {
        int index = (int)tower;
        _damgeTextList[index].text = $"데미지 : {towerData.AttackDamage.ToString()}";
        _attackRangeTextList[index].text = $"사거리 : {towerData.AttackRange.ToString()}";
        _attackSpeedTextList[index].text = $"공격속도 : {towerData.AttackSpeed.ToString()}";
        _projectileSpeedTextList[index].text = $"투사체속도 : {towerData.ProjectileSpeed.ToString()}";
    }

    private void MaxUpgradeLeve()
    {
        int afterIndex = (int)TowerUpgradeState.After;

        if (_damgeTextList.Count > afterIndex) _damgeTextList[afterIndex].text = "MAX";
        if (_attackRangeTextList.Count > afterIndex) _attackRangeTextList[afterIndex].text = "MAX";
        if (_attackSpeedTextList.Count > afterIndex) _attackSpeedTextList[afterIndex].text = "MAX";
        if (_projectileSpeedTextList.Count > afterIndex) _projectileSpeedTextList[afterIndex].text = "MAX";

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
