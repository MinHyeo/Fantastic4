using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NomalKnomeSlotUI : MonoBehaviour
{
    [Header("노움 정보")]
    [SerializeField] private Image Image_Portrait;
    [SerializeField] private TextMeshProUGUI Text_KnomeName;
    [SerializeField] private GameObject GObj_Selected;

    private string _slotDataId;
    public void InitSlot(string dataId)
    {
        _slotDataId = dataId;


    }

    
}
