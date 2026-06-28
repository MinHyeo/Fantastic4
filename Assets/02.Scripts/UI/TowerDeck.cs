using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class TowerDeck : MonoBehaviour
{
    [SerializeField] private UIButton Button_Select;
    [SerializeField] private Image Image_Tower;
    [SerializeField] private TextMeshProUGUI Text_Cost;

    private string _towerDataId;

    private void OnEnable()
    {
        
    }

    public string GetTowerDataId()
    {
        return _towerDataId;
    }

    public void InitTowerDeck(string towerId)
    {
        var entityData = GameDataManager.Instance.GetData<EntityData>(towerId);
        if (entityData == null)
        {
            return;
        }

        string iconPath = entityData.IconPath;
        if (string.IsNullOrEmpty(iconPath) == true)
        {
            return;
        }
        GameUtil.LoadAndSetSpriteImage(Image_Tower, iconPath).Forget();

        string Id = entityData.Id;
        var towerData = GameDataManager.Instance.GetData<TowerData>(Id);

        int price = towerData.BuildPrice;
        Text_Cost.text = price.ToString();
        _towerDataId = towerId;
    }
}
