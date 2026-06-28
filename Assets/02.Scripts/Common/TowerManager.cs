using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager Instance;

    [Header("3D 배치 설정")]

    [SerializeField] private SerializableDictionary<string, GameObject> towerPrefabs = new();

    [SerializeField] private GameObject _TowerPlacementIndicatorPrefab = null;

    [SerializeField] private float _gridSize = 1f;

    private Dictionary<Vector2Int, GameObject> _spawnedTowerList = new Dictionary<Vector2Int, GameObject>();

    private PlacementIndicator _towerPlacementIndicatorObject;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _towerPlacementIndicatorObject = Instantiate(_TowerPlacementIndicatorPrefab).GetComponent<PlacementIndicator>();
        _towerPlacementIndicatorObject.gameObject.SetActive(false);
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
                return false;
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
}
