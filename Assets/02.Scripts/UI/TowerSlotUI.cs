using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System;

public class TowerSlotUI : MonoBehaviour
{
    [Header("타워 정보")]
    [SerializeField] private Image Image_Portrait;
    [SerializeField] private TextMeshProUGUI Text_TowerName;
    [SerializeField] private GameObject GObj_Selected;
    [SerializeField] private UIButton Button_SlotClick;

    private event Action<string> _onClickSlot;

    private string _slotDataId;

    public string GetSlotDataId()
    {
        return _slotDataId;
    }

    private void OnEnable()
    {
        Button_SlotClick.BindOnClickButtonEvent(OnClick_Slot);
    }

    public void OnClick_Slot()
    {
        _onClickSlot?.Invoke(_slotDataId);
    }

    private void OnDisable()
    {
        _onClickSlot = null;
    }
    public void InitSlot(string dataId, Action<string> onClickCallback)
    {
        var entityData = GameDataManager.Instance.GetData<EntityData>(dataId);
        if (entityData == null)
        {
            return;
        }

        string iconPath = entityData.IconPath;
        if (string.IsNullOrEmpty(iconPath) == true)
        {
            return;
        }
        GameUtil.LoadAndSetSpriteImage(Image_Portrait, iconPath).Forget();

        Text_TowerName.text = entityData.Name;

        _slotDataId = dataId;
        _onClickSlot += onClickCallback;

    }

    public void SetSelectedUI(bool isSelect)
    {
        GObj_Selected.SetActive(isSelect);
    }

}
