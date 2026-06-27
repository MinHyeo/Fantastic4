using TMPro;
using UnityEngine;

public class TowerUpgradeUI : UIBase
{
    [Header("버튼")]
    [SerializeField] private UIButton Button_CloseBG;
    [SerializeField] private UIButton Button_Close;
    [SerializeField] private UIButton Button_Upgrade;

    [Header("텍스트")]
    [SerializeField] private TextMeshProUGUI Text_LevelBefore;
    [SerializeField] private TextMeshProUGUI Text_DamageBefore;
    [SerializeField] private TextMeshProUGUI Text_AttackSpeedBefore;

    [SerializeField] private TextMeshProUGUI Text_LevelAfter;
    [SerializeField] private TextMeshProUGUI Text_DamageAfter;
    [SerializeField] private TextMeshProUGUI Text_AttackSpeedAfter;

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

    private void OnClickTowerUpgrade()
    {
        UpdateTowerUpgradeInfo();

    }

    private void UpdateTowerUpgradeInfo()
    {

    }
}
