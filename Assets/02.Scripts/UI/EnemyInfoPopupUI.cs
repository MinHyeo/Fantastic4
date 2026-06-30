using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class EnemyInfoPopupUI : UIBase
{
    [SerializeField] private UIButton Button_TowerInfo;

    [Header("프리팹")]
    [SerializeField] private GameObject Prefab_NomalKnomeSlotUI;

    [Header("노움 세부 정보 디테일")]
    [SerializeField] private Image Image_Portrait;
    [SerializeField] private TextMeshProUGUI Text_KnomeName;
    [SerializeField] private TextMeshProUGUI Text_StatSpeed;
    [SerializeField] private TextMeshProUGUI Text_RewardGold;
    [SerializeField] private TextMeshProUGUI Text_HP;

    [SerializeField] private UIButton Button_Close;

    [Header("슬롯 리스트 영역")]
    [SerializeField] private Transform Transform_NomalSlotRoot;

    private Dictionary<string, NomalKnomeSlotUI> _slotList = new Dictionary<string, NomalKnomeSlotUI>();

    private void OnEnable()
    {
        ReadNomalKnomeInfoListAndCreateSlot();
        Button_TowerInfo.BindOnClickButtonEvent(OnClick_TowerInfo);
        //StartCoroutineForNomalKnome();

        Button_Close.BindOnClickButtonEvent(Onclick_Clsoe);
    }

    private void OnDisable()
    {
        if(_slotList.Count > 0)
        {
            foreach(var slotKv in _slotList)
            {
                var slot = slotKv.Value;
                DestroyImmediate(slot.gameObject);
            }

            _slotList.Clear();
        }
    }

    private void Onclick_Clsoe()
    {
        UIManager.Instance.ClosePopupUI(UIType.EnemyInfoPopupUI);
    }


    private void OnClick_TowerInfo()
    {
        UIManager.Instance.ClosePopupUI(UIType.EnemyInfoPopupUI);

        UIManager.Instance.OpenPopupUI(UIType.TowerInfoPopupUI);
    }

    //동적 생성 테스트 코루틴 코드 
    //private void StartCoroutineForNomalKnome()
    //{
    //    StartCoroutine(ReadNomalKnomeInfoListAndCreateSlotCoroutine());
    //}

    //private IEnumerator ReadNomalKnomeInfoListAndCreateSlotCoroutine()
    //{

    //    yield return new WaitForSeconds(1.0f);

    //    var NomalKnomeList = GameDataManager.Instance.GetEnemyIds();

    //    if (NomalKnomeList != null)
    //    {
    //        foreach (var enemyId in NomalKnomeList)
    //        {
    //            if (enemyId == null)
    //            {
    //                continue;
    //            }

    //            CreateNomalSlot(enemyId);
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
    private void ReadNomalKnomeInfoListAndCreateSlot()
    {
        var NomalKnomeList = GameDataManager.Instance.GetEnemyIds();
        foreach (var enemyId in NomalKnomeList)
        {

            if (enemyId == null)
            {
                continue;
            }

            CreateNomalSlot(enemyId);
        }
        if (_slotList.Count > 0)
        {
            foreach (var slotKv in _slotList)
            {
                var slot = slotKv.Value;
                slot.OnClick_Slot();
            }
        }
    }


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
        slotComponent.InitSlot(dataId, OnClickChildSlotSelected);
        _slotList.Add(dataId, slotComponent);
    }

    private void OnClickChildSlotSelected(string slotDataId)
    {
        var entityData = GameDataManager.Instance.GetData<EntityData>(slotDataId);
        if(entityData == null)
        {
            return;
        }

        var enemyData = GameDataManager.Instance.GetData<EnemyData>(slotDataId);
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
        
        Text_KnomeName.text = entityData.Name;
        Text_HP.text = enemyData.MaxHp.ToString();
        Text_StatSpeed.text = enemyData.MoveSpeed.ToString();
        Text_RewardGold.text = enemyData.RewardGold.ToString();

        foreach(var slotkv in _slotList)
        {
            var slot = slotkv.Value;
            var dataId = slot.GetSlotDataId();
            slot.SetSelectedUI(slotDataId == dataId);
            
        }

        

    }
   




}
