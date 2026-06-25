using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerDeck : MonoBehaviour
{
    [SerializeField] private UIButton Button_Select;
    [SerializeField] private Image Image_Tower;
    [SerializeField] private TextMeshProUGUI Text_Cost;

    private event Action<string> _onclickTowerDeck;
    private string _towerDataId;

    private void OnEnable()
    {
        Button_Select.BindOnClickButtonEvent(OnClickTowerDeck);
    }

    private void OnDisable()
    {
        _onclickTowerDeck = null;
    }

    public string GetTowerDataId()
    {
        return _towerDataId;
    }

    public void OnClickTowerDeck()
    {
        _onclickTowerDeck?.Invoke(_towerDataId);
    }

    public void InitTowerDeck(string dataId, Action<string> OnClickCallback)
    {
        //var towerData = GameDataManager.Instance.GetTowerData(dataId);
        //if (towerData == null)
        //{
        //    return;
        //}

        //Text_Cost.text = towerData.cost;
        //string iconPath = towerData.IconPath;
        //if (string.IsNullOrEmpty(iconPath) == true)
        //{
        //    return;
        //}

        //GameUtil.LoadAndSetSpriteImage(Image_Tower, iconPath).Forget();

        //_towerDataId = dataId;

        _onclickTowerDeck += OnClickCallback;
    }

    public void SetSelected(bool isSelect)
    {

    }
}
