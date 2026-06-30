using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// UI 타워 버튼의 드래그 입력과 타워 배치를 처리합니다.
/// </summary>
public class TowerDragUI : UIBase, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("타워 배치 설정")]

    [SerializeField] private string _towerId;

    /// <summary>
    /// 이 UI가 생성할 타워
    /// </summary>
    private GameObject _tower;

    private int _towerBuildPrice = 0;

    public event Func<int, bool> OnCheckCanAfford;
    public event Action<int> OnTowerPlacedSpendGold;

    public void Start()
    {
        var towerData = GameDataManager.Instance.GetData<TowerData>(_towerId);
        _towerBuildPrice = towerData.BuildPrice;
        ResourceManager.Instance.LoadAsset<GameObject>(towerData.PrefabPath, tower => _tower = tower);
    }

    /// <summary>
    /// 버튼에서 드래그가 시작되면 미리보기 타워를 생성합니다.
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (OnCheckCanAfford != null && !OnCheckCanAfford.Invoke(_towerBuildPrice))
        {
            return;
        }

        TowerManager.Instance.ShowTowerPlacementIndicator(_tower);
    }

    /// <summary>
    /// 다음 단계에서 미리보기 타워를 이동시킵니다.
    /// </summary>
    public void OnDrag(PointerEventData eventData) { }

    /// <summary>
    /// 드래그가 끝나면 유효한 위치에 타워를 배치하고 미리보기를 제거합니다.
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        if (_tower == null)
        {
            TowerManager.Instance.HideTowerPlacementIndicator();
            return;
        }

        if (OnCheckCanAfford != null && !OnCheckCanAfford.Invoke(_towerBuildPrice))
        {
            TowerManager.Instance.HideTowerPlacementIndicator();
            return;
        }

        // UI_TODO : 여기서 돈 부족하다? 설치할 수 없다 notice 열어야함
        var towerBase = _tower.GetComponent<TowerBase>();
        if (TowerManager.Instance.CanPlaceTower(out Vector3 worldPos) && 
            StageManager.Instance.CurrentStageGold >= int.Parse(towerBase.Data.UpgradePrice))
        {
            TowerManager.Instance.SpawnTower(_towerId, worldPos);
            OnTowerPlacedSpendGold?.Invoke(_towerBuildPrice);
        }

        TowerManager.Instance.HideTowerPlacementIndicator();
    }

    /// <summary>
    /// 타워 ID 설정
    /// </summary>
    public void SetTowerID(string towerId)
    {
        _towerId = towerId;
    }
}