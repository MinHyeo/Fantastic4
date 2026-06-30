using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Splines;
using static UnityEngine.GraphicsBuffer;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    private EnemyRouteManager _enemyRouteManager;
    private int _activeEnemyCount = 0;
    private int _currentStageGold = 0;
    //private List<EnemyBase> _activeEnemyList = new List<EnemyBase>();

    public int CurrentStageGold => _currentStageGold;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        _enemyRouteManager = new();
    }

    public void LoadStage(string stageId)
    {
        Vector3 spawnSpot = Vector3.zero;
        _activeEnemyCount = 0;
        GameObjectManager.Instance.CreateStageObject(stageId, spawnSpot).Forget();
    }

    public void StartStage(string stageId, GameObject stageObject)
    {
        StageData stageData = GameDataManager.Instance.GetData<StageData>(stageId);
        if (stageData == null)
            return;

        var splineContinear = stageObject.GetComponent<SplineContainer>();
        _enemyRouteManager.SetSplineCointer(splineContinear);

        // 스테이지 골드 불러오기
        InitStageGoldData();

        string[] waveIds = stageData.WaveId;
        foreach(string waveId in waveIds)
        {
            SpawnWave(waveId);
        }
    }

    private void SpawnWave(string waveId)
    {
        WaveData waveData = GameDataManager.Instance.GetData<WaveData>(waveId);
        if (waveData == null)
            return;

        CancellationToken cancellationToken = this.GetCancellationTokenOnDestroy();
        DelayAnSpawnWaveAsync(waveData, cancellationToken).Forget();
    }

    private async UniTaskVoid DelayAnSpawnWaveAsync(WaveData waveData, CancellationToken cancellationToken)
    {
        float preDelay = waveData.PreDelay;
        await UniTask.Delay(TimeSpan.FromSeconds(preDelay), cancellationToken: cancellationToken);

        int spawnCount = waveData.Count;
        float interval = waveData.Interval;
        string enemyId = waveData.EnemyId;
        _activeEnemyCount += spawnCount;
        for (int i = 0; i < spawnCount; i++)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(interval), cancellationToken: cancellationToken);

            Vector3 spawnPoint = GetCoursePosition(0);
            GameObjectManager.Instance.CreateEnemyObject(enemyId, spawnPoint).Forget();
        }
    }

    public bool CheckEndCourse(int courseIndex)
    {
        return _enemyRouteManager.CheckEndCource(courseIndex);
    }

    public Vector3 GetCoursePosition(int courseIndex)
    {
        return _enemyRouteManager.GetCoursePosition(courseIndex);
    }

    public GameObject CompareLeadEnemy(GameObject leadEnemy, GameObject comparisonEnemy)
    {
        var leadEnemyScript = leadEnemy.GetComponent<EnemyBase>();
        var comparisonEnemyScript = comparisonEnemy.GetComponent<EnemyBase>();
        if (leadEnemyScript == null || comparisonEnemyScript == null)
            return leadEnemy;

        if (comparisonEnemyScript.IsDead)
            return leadEnemy;
        if(leadEnemyScript.IsDead)
            return comparisonEnemy;

        int comparisonIndex = comparisonEnemyScript.CourseIndex;
        int leadIndex = leadEnemyScript.CourseIndex;
        if (comparisonIndex > leadIndex)
            return comparisonEnemy;
        if (comparisonIndex < leadIndex)
            return leadEnemy;

        var coursePosition = _enemyRouteManager.GetCoursePosition(comparisonIndex);
        float leadDist = Vector3.Distance(leadEnemy.transform.position, coursePosition);
        float comparisionDist = Vector3.Distance(comparisonEnemy.transform.position, coursePosition);

        if (leadDist < comparisionDist)
            return leadEnemy;
        return comparisonEnemy;
    }

    public void RemoveActivatedEnemy()
    {
        _activeEnemyCount -= 1;

        if(_activeEnemyCount <= 0)
        {
            ClearStage();
        }
    }

    private void InitStageGoldData()
    {
        var stageIdList = GameDataManager.Instance.GetStageIds();
        foreach (var stageId in stageIdList)
        {
            if (stageId == null)
            {
                continue;
            }

            GetStageGoldData(stageId);
        }
    }

    private void GetStageGoldData(string stageId)
    {
        var stagedata = GameDataManager.Instance.GetData<StageData>(stageId);
        _currentStageGold = stagedata.StartGold;
    }

    public void IncreseGold(int gold)
    {
        _currentStageGold += gold;
    }

    public void DecreaseGold(int gold)
    {
        _currentStageGold -= gold;
    }

    private void ClearStage()
    {
        // UI 호출
        UIManager.Instance.OpenUI(UIRootType.PopupUI, UIType.ResultSuccessUI);
    }

    public void FaildStage()
    {
        UIManager.Instance.OpenUI(UIRootType.PopupUI, UIType.ResultFailUI);
    }
}