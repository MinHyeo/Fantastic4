using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class TowerInfoPopupUI : UIBase
{
    [SerializeField] private UIButton Button_EnemyInfo;

    [Header("프리팹")]
    [SerializeField] private GameObject Prefab_TowerSlotUI;

    [Header("타워 세부 정보 디테일")]
    [SerializeField] private Image Image_Portrait;
    [SerializeField] private TextMeshProUGUI Text_TowerName;
    [SerializeField] private TextMeshProUGUI Text_AttackDamage;
    [SerializeField] private TextMeshProUGUI Text_AttackRange;
    [SerializeField] private TextMeshProUGUI Text_AttackSpeed;

   


    [SerializeField] private UIButton Button_Close;

    [Header("슬롯 리스트 영역")]
    [SerializeField] private Transform Transform_TowerSlotUIRoot;

    private Dictionary<string, TowerSlotUI> _slotList = new Dictionary<string, TowerSlotUI>();
    private string _currentBaseId;

    private void OnEnable()
    {
        ReadTowerInfoListAndCreateSlot();
        Button_EnemyInfo.BindOnClickButtonEvent(OnClick_EnemyInfo);
        //StartReadTowerInfoListAndCreateSlot();

        Button_Close.BindOnClickButtonEvent(Onclick_Clsoe);
    }

    private void OnDisable()
    {
        if (_slotList.Count > 0)
        {
            foreach (var slotKv in _slotList)
            {
                var slot = slotKv.Value;
                DestroyImmediate(slot.gameObject);
            }

            _slotList.Clear();
        }
    }

    private void Onclick_Clsoe()
    {
        UIManager.Instance.ClosePopupUI(UIType.TowerInfoPopupUI);
    }


    private void OnClick_EnemyInfo()
    {
        UIManager.Instance.OpenPopupUI(UIType.EnemyInfoPopupUI);
    }
    


    //동적생성 테스트 전용 코루틴 로직
    //private void StartReadTowerInfoListAndCreateSlot()
    //{
    //    StartCoroutine(ReadNomalTowerInfoListAndCreateSlotCoroutine());
    //}

    //private IEnumerator ReadNomalTowerInfoListAndCreateSlotCoroutine()
    //{

    //    yield return new WaitForSeconds(1.0f);

    //    var TowerList = GameDataManager.Instance.GetAllTowerIds();

    //    if (TowerList != null)
    //    {
    //        foreach (var towerId in TowerList)
    //        {
    //            if (towerId.Contains("Level1"))
    //            {
    //                CreateNomalSlot(towerId);
    //            }

    //        }
    //        if (_slotList != null && _slotList.Count > 0)
    //        {
    //            var firstSlot = _slotList.Values.GetEnumerator();
    //            if (firstSlot.MoveNext())
    //            {
    //                firstSlot.Current.OnClick_Slot();
    //            }
    //        }
    //    }
    //}

    private void ReadTowerInfoListAndCreateSlot()
    {
        var TowerList = GameDataManager.Instance.GetAllTowerIds();
        foreach (var towerId in TowerList)
        {

            if (towerId.Contains("Level1"))
            {
                CreateNomalSlot(towerId);
            }

        }
        if (_slotList != null && _slotList.Count > 0)
        {
            var firstSlot = _slotList.Values.GetEnumerator();
            if (firstSlot.MoveNext())
            {
                firstSlot.Current.OnClick_Slot();
            }
        }
    }


    private void CreateNomalSlot(string dataId)
    {
        var gObj = Instantiate(Prefab_TowerSlotUI, Transform_TowerSlotUIRoot);
        if (gObj == null)
        {
            return;
        }

        var slotComponent = gObj.GetComponent<TowerSlotUI>();
        if (slotComponent == null)
        {
            return;
        }
        slotComponent.InitSlot(dataId, OnClickChildSlotSelected);
        _slotList.Add(dataId, slotComponent);
    }

    private void OnClickChildSlotSelected(string slotDataId)
    {
        var entityData = GameDataManager.Instance.GetData<EntityData>(slotDataId);
        if (entityData == null)
        {
            return;
        }

        var towerData = GameDataManager.Instance.GetData<TowerData>(slotDataId);
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
        Text_AttackDamage.text = towerData.AttackDamage.ToString();
        Text_AttackRange.text = towerData.AttackRange.ToString();
        Text_AttackSpeed.text = towerData.AttackSpeed.ToString();

        foreach (var slotkv in _slotList)
        {
            var slot = slotkv.Value;
            var dataId = slot.GetSlotDataId();
            slot.SetSelectedUI(slotDataId == dataId);

        }



    }





}
