using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// UI 타워 버튼의 드래그 입력과 타워 배치를 처리합니다.
/// </summary>
public class JiwonTestRunner : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("타워 배치 설정")]

    [SerializeField] private GameObject _towerPrefab;

    /// <summary>
    /// 버튼에서 드래그가 시작되면 미리보기 타워를 생성합니다.
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        TowerManager.Instance.ShowTowerPlacementIndicator(_towerPrefab);
    }

    /// <summary>
    /// 다음 단계에서 미리보기 타워를 이동시킵니다.
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        
    }

    /// <summary>
    /// 현재는 드래그가 끝나면 미리보기만 제거합니다.
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        TowerManager.Instance.HideTowerPlacementIndicator();
    }
}