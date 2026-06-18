using System;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.LowLevelPhysics2D.PhysicsShape;

public class TowerDragger : MonoBehaviour, IDragHandler, IPointerUpHandler
{
    [Header("Attack Range Object")]
    [SerializeField] private GameObject _attackRangeCircle;

    private string _towerId;
    private Action<bool> _onPlacementResult;
    private Camera _mainCam; // 메인 카메라 캐싱용 변수 추가

    private void Awake()
    {
        // 매 프레임 Camera.main을 호출하는 오버헤드를 줄이기 위해 캐싱
        _mainCam = Camera.main;
    }

    public void InitBatchObject(string towerId, Action<bool> callback)
    {
        _towerId = towerId;
        _onPlacementResult = callback;

        TowerData towerData = GameDataManager.Instance.GetData<TowerData>(towerId);

        float rangeDiameter = towerData.AttackRange * 2f;
        _attackRangeCircle.transform.localScale = new Vector3(rangeDiameter, 1f, rangeDiameter);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_mainCam == null) return;

        // 마우스 위치로부터 3D 공간으로 Ray(광선)를 생성
        Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);

        // 바닥(Ground) 레이어에 부딪힌 지점을 찾기 (성능을 위해 Ground 레이어마스크 설정 추천)
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("배치 시도");
        bool isPlaceSuccess = false;

        if (TowerManager.Instance.CanPlaceTower(transform.position))
        {
            TowerManager.Instance.SpawnTower(_towerId, transform.position);
            isPlaceSuccess = true;
        }

        gameObject.SetActive(false);
        _onPlacementResult?.Invoke(isPlaceSuccess);
    }
}
