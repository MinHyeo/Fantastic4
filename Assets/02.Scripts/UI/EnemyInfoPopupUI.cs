using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInfoPopupUI : UIBase
{
    [Header("프리팹")]
    [SerializeField] private GameObject Prefab_NomalKnomeSlotUI;
    [SerializeField] private GameObject Prefab_SpecialKnomeSlotUI;

    [Header("노움 세부 정보 디테일")]
    [SerializeField] private Image Image_Portrait;
    [SerializeField] private TextMeshProUGUI Text_KnomeName;
    [SerializeField] private TextMeshProUGUI Text_StatATD;
    [SerializeField] private TextMeshProUGUI Text_StatRange;
    [SerializeField] private TextMeshProUGUI Text_StatAttackSpeed;
    [SerializeField] private TextMeshProUGUI Text_StatSpeed;

    [Header("슬롯 리스트 영역")]
    [SerializeField] private Transform Transform_NomalSlotRoot;
    [SerializeField] private Transform Transform_SpecialSlotRoot;



    private void CreateNomalSlot(string dataId)
    {
        var gObj = Instantiate(Prefab_NomalKnomeSlotUI, Transform_NomalSlotRoot);
        if (gObj == null)
        {
            return;
        }

        var slotComponent = gObj.GetComponent<NomalKnomeSlotUI>();
        if (slotComponent == null)
        {
            return;
        }

    }

    private void CreateSpecialSlot(string dataId)
    {
        var gObj = Instantiate(Prefab_SpecialKnomeSlotUI, Transform_SpecialSlotRoot);
        if (gObj == null)
        {
            return;
        }

        var slotComponent = gObj.GetComponent<SpecialKnomeSlotUI>();
        if (slotComponent == null)
        {
            return;
        }

    }




}
