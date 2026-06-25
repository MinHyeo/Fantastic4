using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.Splines;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    [SerializeField] private SplineContainer _splineContainer;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        StartCoroutine(test());
    }

    private IEnumerator test()
    {
        yield return new WaitForSeconds(1f);
        //StartStage("stage_01");
    }

    public void LoadStage(string stageId)
    {
        Vector3 spawnSpot = Vector3.zero;
        GameObjectManager.Instance.CreateStageObject(stageId, spawnSpot).Forget();
    }

    public bool CheckEndCourse(int courseIndex)
    {
        Spline spline = _splineContainer.Splines[0];
        if (spline.Count <= courseIndex)
            return true;
        return false;
    }

    public Vector3 GetCoursePosition(int courseIndex)
    {
        Spline spline = _splineContainer.Splines[0];
        Vector3 localPosition = (Vector3)spline[courseIndex].Position;
        Vector3 worldPosition = _splineContainer.transform.TransformPoint(localPosition);

        return worldPosition;
    }

    public void StartStage(string stageId, GameObject stageObject)
    {
        StageData stageData = GameDataManager.Instance.GetData<StageData>(stageId);
        if (stageData == null)
            return;

        _splineContainer = stageObject.GetComponent<SplineContainer>();
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
        for(int i = 0; i < spawnCount; i++)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(interval), cancellationToken: cancellationToken);

            Vector3 spawnPoint = GetCoursePosition(0);
            GameObjectManager.Instance.CreateEnemyObject(enemyId, spawnPoint).Forget();
        }
    }
}