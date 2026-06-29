using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerManager : MonoBehaviour
{
    public static TowerManager Instance;

    [Header("3D 배치 설정")]

    [SerializeField] private GameObject _TowerPlacementIndicatorPrefab = null;

    [SerializeField] private Camera _selectionCamera;

    [SerializeField] private LayerMask _towerSelectionLayerMask = ~0;

    [SerializeField] private float _gridSize = 1f;

    private Dictionary<Vector2Int, GameObject> _spawnedTowerList = new Dictionary<Vector2Int, GameObject>();

    private readonly List<RaycastResult> _uiRaycastResults = new List<RaycastResult>();

    private PlacementIndicator _towerPlacementIndicatorObject;

    private TowerBase _selectedTower;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _towerPlacementIndicatorObject = Instantiate(_TowerPlacementIndicatorPrefab).GetComponent<PlacementIndicator>();
        _towerPlacementIndicatorObject.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        if (IsPointerOverUI())
        {
            return;
        }

        if (TryGetClickedTower(out TowerBase clickedTower))
        {
            ToggleTowerRangeVisualizer(clickedTower);
            return;
        }

        ClearSelectedTower();
    }

    /// <summary>
    /// 클릭된 타워의 사거리만 표시하고 이전 타워의 사거리 표시는 끕니다.
    /// </summary>
    public void ShowTowerRangeVisualizer(TowerBase tower)
    {
        if (tower == null)
        {
            ClearSelectedTower();
            return;
        }

        if (_selectedTower != null && _selectedTower != tower)
        {
            _selectedTower.SetRangeVisualizerVisible(false);
        }

        _selectedTower = tower;
        _selectedTower.SetRangeVisualizerVisible(true);
    }

    /// <summary>
    /// 기존 토글 호출 경로가 TowerManager의 단일 선택 규칙을 거치도록 처리합니다.
    /// </summary>
    public void ToggleTowerRangeVisualizer(TowerBase tower)
    {
        if (tower == null)
        {
            ClearSelectedTower();
            return;
        }

        if (_selectedTower == tower)
        {
            ClearSelectedTower();
            return;
        }

        ShowTowerRangeVisualizer(tower);
    }

    /// <summary>
    /// 현재 선택된 타워의 사거리 표시를 해제합니다.
    /// </summary>
    public void ClearSelectedTower()
    {
        if (_selectedTower == null)
        {
            return;
        }

        _selectedTower.SetRangeVisualizerVisible(false);
        _selectedTower = null;
    }

    /// <summary>
    /// 전달된 타워가 현재 선택된 타워일 때만 선택을 해제합니다.
    /// </summary>
    public void ClearSelectedTower(TowerBase tower)
    {
        if (_selectedTower != tower)
        {
            return;
        }

        ClearSelectedTower();
    }

    /// <summary>
    /// 카메라 레이가 맞은 오브젝트가 배치 가능한 레이어인지 검사합니다.
    /// </summary>
    public bool CanPlaceTower(RaycastHit placementHit, LayerMask placementLayerMask)
    {
        return CanPlaceTower(placementHit.collider, placementLayerMask);
    }

    /// <summary>
    /// 배치 가능한 콜라이더의 위치와 높이를 기준으로 배치 가능 여부를 검사합니다.
    /// </summary>
    public bool CanPlaceTower(Collider placementCollider, LayerMask placementLayerMask)
    {
        if (placementCollider == null)
        {
            return false;
        }

        Vector3 snapPos = GetGridSnappedPosition(placementCollider);
        int hitLayerMask = 1 << placementCollider.gameObject.layer;
        if ((placementLayerMask.value & hitLayerMask) == 0)
        {
            return false;
        }

        if (_spawnedTowerList.ContainsKey(GetGridCell(snapPos)))
        {
            return false;
        }

        // 배치 지점에 이미 다른 타워가 있는지 검사합니다.
        float checkRadius = _gridSize * 0.4f;
        Collider[] hitColliders = Physics.OverlapSphere(snapPos, checkRadius);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.GetComponentInParent<TowerBase>() != null)
            {
                continue;
            }
        }

        return true;
    }

    /// <summary>
    /// 타워 인디케이터를 통해 현재 배치 가능한 부분인지?
    /// </summary>
    public bool CanPlaceTower(out Vector3 worldPos)
    {
        bool result = _towerPlacementIndicatorObject.CanPlaceTower;
        if (result)
        {
            worldPos = _towerPlacementIndicatorObject.transform.position;
        }
        else
        {
            worldPos = Vector3.zero;
        }

        return result;
    }

    // 어떤 타워 ID가 들어오든 Grid에 맞춰 생성만 해주는 통합 기능
    public void SpawnTower(string towerId, Vector3 cellWorldPos)
    {
        Vector2Int gridCell = GetGridCell(cellWorldPos);
        if (_spawnedTowerList.ContainsKey(gridCell))
        {
            return;
        }

        GameObjectManager.Instance.CreateTowerObject(towerId, cellWorldPos, towerObject =>
        {
            _spawnedTowerList.Add(gridCell, towerObject);
        }).Forget();
    }

    /// <summary>
    /// 타워 배치 인디케이터 켜기
    /// </summary>
    public PlacementIndicator ShowTowerPlacementIndicator(GameObject towerPrefabToIndicate)
    {
        // 타워 프리팹에 저장된 배치 가능 레이어를 인디케이터에 전달합니다.
        TowerBase tower = towerPrefabToIndicate.GetComponent<TowerBase>();
        _towerPlacementIndicatorObject.gameObject.SetActive(true);
        _towerPlacementIndicatorObject.SetRenderTarget(towerPrefabToIndicate, tower.PlacementLayerMask);
        return _towerPlacementIndicatorObject;
    }

    /// <summary>
    /// 타워 배치 인디케이터 끄기
    /// </summary>
    public void HideTowerPlacementIndicator()
    {
        _towerPlacementIndicatorObject.gameObject.SetActive(false);
    }

    public void DestroyAllTower()
    {
        // GameObjectManager.Instance.RequestDestroyAllTowerObject();
        _spawnedTowerList.Clear();
    }

    private Vector2Int GetGridCell(Vector3 worldPos)
    {
        int x = Mathf.FloorToInt(worldPos.x / _gridSize);
        int z = Mathf.FloorToInt(worldPos.z / _gridSize);
        return new Vector2Int(x, z);
    }

    public Vector3 GetGridSnappedPosition(Collider placementCollider)
    {
        Vector3 snappedWorldPos = placementCollider.gameObject.transform.position;
        float heightOffset = placementCollider.bounds.size.y;

        return new Vector3(snappedWorldPos.x, snappedWorldPos.y + heightOffset, snappedWorldPos.z);
    }

    private bool TryGetClickedTower(out TowerBase tower)
    {
        tower = null;

        Camera rayCamera = _selectionCamera != null ? _selectionCamera : Camera.main;
        if (rayCamera == null)
        {
            return false;
        }

        RaycastHit[] hits = Physics.RaycastAll(rayCamera.ScreenPointToRay(Input.mousePosition), Mathf.Infinity, _towerSelectionLayerMask);
        System.Array.Sort(hits, (left, right) => left.distance.CompareTo(right.distance));

        foreach (RaycastHit hit in hits)
        {
            if (IsSelectionIgnoredCollider(hit.collider))
            {
                continue;
            }

            tower = hit.collider.GetComponentInParent<TowerBase>();
            return tower != null;
        }

        return false;
    }

    /// <summary>
    /// 다른 콜라이더는 TowerManager의 클릭 로직에서 제외하도록 하는 함수
    /// </summary>
    private bool IsSelectionIgnoredCollider(Collider hitCollider)
    {
        if (hitCollider == null)
        {
            return true;
        }

        // 사거리 감지용 콜라이더는 타워 본체 클릭으로 취급하지 않습니다.
        if (hitCollider.GetComponentInParent<Detector>() != null)
        {
            return true;
        }

        return hitCollider.GetComponentInParent<RangeVisualizer>() != null;
    }

    /// <summary>
    /// UI를 클릭한거면 tower를 클릭하지 못하게 블락
    /// </summary>
    private bool IsPointerOverUI()
    {
        if (EventSystem.current == null)
        {
            return false;
        }

        _uiRaycastResults.Clear();

        PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        EventSystem.current.RaycastAll(pointerEventData, _uiRaycastResults);
        foreach (RaycastResult raycastResult in _uiRaycastResults)
        {
            if (raycastResult.module is UnityEngine.UI.GraphicRaycaster)
            {
                return true;
            }
        }

        return false;
    }
}
