using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameObjectManager : MonoBehaviour
{
    [SerializeField] private Transform _root;

    public static GameObjectManager Instance { get; set; }

    // 생성된 오브젝트의 키가 됨
    private int _objectInstanceKeyGenerator = 0;

    // 생성된 오브젝트의 생명을 보관
    private Dictionary<string, Queue<GameObject>> _objectPool = new Dictionary<string, Queue<GameObject>>();
    private HashSet<GameObject> _activeObjectList = new HashSet<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    private GameObject GetGameObjectInObjectPool(string objectId)
    {
        if (_objectPool.ContainsKey(objectId) == false)
        {
            _objectPool[objectId] = new Queue<GameObject>();
        }

        GameObject spawnObject = null;
        if (_objectPool[objectId].Count > 0)
        {
            spawnObject = _objectPool[objectId].Dequeue();
        }

        return spawnObject;
    }

    // 적 소환
    public async UniTaskVoid CreateEnemyObject(string enemyId, Vector3 spawnSpot)
    {
        // 데이터 유무 확인
        var enemyData = GameDataManager.Instance.GetData<EnemyData>(enemyId);
        if (enemyData == null)
            return;

        // ObjectPool에 존재하는지 확인
        // 없으면 Queue 초기화
        GameObject enemyObject = GetGameObjectInObjectPool(enemyId);
        if(enemyObject == null)
        {
            enemyObject = await ResourceManager.Instance.InstantiateAsync(enemyData.PrefabPath, _root, true);
        }
        enemyObject.SetActive(true);
        enemyObject.transform.position = spawnSpot;
        AddEnemyObjectOnCreate(enemyObject, enemyId);
    }

    private void AddEnemyObjectOnCreate(GameObject createdObject, string enemyId)
    {
        _objectInstanceKeyGenerator++;
        var generatedInstanceId = _objectInstanceKeyGenerator;
        var enemyObject = createdObject.GetComponent<NormalEnemy>();

        if (enemyObject != null)
        {
            enemyObject.Init(enemyId);
            _activeObjectList.Add(createdObject);
        }
    }

    // 타워 소환
    public async UniTaskVoid CreateTowerObject(string towerId, Vector3 spawnSpot)
    {
        // 데이터 유무 확인
        var towerData = GameDataManager.Instance.GetData<TowerData>(towerId);
        if (towerData == null)
            return;

        // ObjectPool에 존재하는지 확인
        // 없으면 Queue 초기화
        GameObject towerObject = GetGameObjectInObjectPool(towerId);
        if (towerObject == null)
        {
            towerObject = await ResourceManager.Instance.InstantiateAsync(towerData.PrefabPath, _root, true);
        }
        towerObject.SetActive(true);
        towerObject.transform.position = spawnSpot;
        AddTowerObjectOnCreate(towerObject, towerId);
    }

    private void AddTowerObjectOnCreate(GameObject createdObject, string towerId)
    {
        _objectInstanceKeyGenerator++;
        var generatedInstanceId = _objectInstanceKeyGenerator;
        var towerObject = createdObject.GetComponent<NormalEnemy>();

        if (towerObject != null)
        {
            towerObject.Init(towerId);
            _activeObjectList.Add(createdObject);
        }
    }

    // 스테이지 소환
    public async UniTaskVoid CreateStageObject(string stageId, Vector3 spawnSpot)
    {
        // 데이터 유무 확인
        var stageData = GameDataManager.Instance.GetData<StageData>(stageId);
        if (stageData == null)
            return;

        // ObjectPool에 존재하는지 확인
        // 없으면 Queue 초기화
        GameObject stageObject = GetGameObjectInObjectPool(stageId);
        if (stageObject == null)
        {
            stageObject = await ResourceManager.Instance.InstantiateAsync(stageData.PrefabPath, _root, true);
        }
        stageObject.SetActive(true);
        stageObject.transform.position = spawnSpot;
        AddStageObjectOnCreate(stageObject, stageId);
    }

    private void AddStageObjectOnCreate(GameObject createdObject, string stageId)
    {
        _objectInstanceKeyGenerator++;
        var generatedInstanceId = _objectInstanceKeyGenerator;

        StageManager.Instance.StartStage(stageId, createdObject);
        _activeObjectList.Add(createdObject);
    }
}
