using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager Instance;

    private Dictionary<int, GameObject> _spawnedTowerList = new Dictionary<int, GameObject>();

    [Header("3D 배치 설정")]
    [SerializeField] private float _gridSize = 1f;

    private int _towerSequenceId = 0;

    private void Awake()
    {
        Instance = this;
    }

    public bool CanPlaceTower(Vector3 cellPos)
    {
        // 올바른 바닥인지 검사
        Ray ray = new Ray(cellPos + Vector3.up * 1f, Vector3.down);

        // 레이어로 확인 후 태그 검사
        if (Physics.Raycast(ray, out RaycastHit hit, 2f, _groundLayer))
        {
            if (!hit.collider.CompareTag("TowerSpace"))
            {
                Debug.LogWarning("타워를 설치할 수 없는 종류의 지형입니다.");
                return false;
            }
        }
        else
        {
            Debug.LogWarning("타워를 설치할 수 있는 바닥이 없습니다.");
            return false;
        }

        // 중복 검사 OverlapSphere 범위 내에 컴포넌트가 있는지 검사
        float checkRadius = _gridSize * 0.4f;
        Collider[] hitColliders = Physics.OverlapSphere(cellPos, checkRadius);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<Tower>() != null)
            {
                Debug.LogWarning("이미 타워가 있음");
                return false;
            }
        }

        return true;
    }

    // 어떤 타워 ID가 들어오든 Grid에 맞춰 생성만 해주는 통합 기능
    public void SpawnTower(string towerId, Vector3 cellPos)
    {
        Vector3 snapPos = SnapToGrid(cellPos);

        GameObject towerObject = GameObjectManager.Instance.CreateTowerOjbect(towerId, snapPos);

        if (towerObject != null)
        {
            _spawnedTowerList.Add(_towerSequenceId, towerObject);
            _towerSequenceId++;
        }
    }

    public void DestroyAllTower()
    {
        GameObjectManager.Instance.RequestDestroyAllTowerObject();
        _spawnedTowerList.Clear();
        _towerSequenceId = 0;
    }

    private Vector3 SnapToGrid(Vector3 worldPos)
    {
        float x = Mathf.Floor(worldPos.x / _gridSize) * _gridSize + (_gridSize * 0.5f);
        float z = Mathf.Floor(worldPos.z / _gridSize) * _gridSize + (_gridSize * 0.5f);
        return new Vector3(x, worldPos.y, z);
    }
}