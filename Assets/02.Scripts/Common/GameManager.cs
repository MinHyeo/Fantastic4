using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    private const string SAVE_KEY = "CurrentUnlockedStage";

    private int _maxStageIndex = 3;
    private int _currentUnlockedStage = 1;
    public int CurrentUnlockedStage => _currentUnlockedStage;

    private void Awake()
    {
        Instance = this;
        LoadGameData(); 
    }

    private void LoadGameData()
    {
        _currentUnlockedStage = PlayerPrefs.GetInt(SAVE_KEY, 1);
    }

    public void UnlockStage(int stageNumber)
    {
        if (_currentUnlockedStage >= _maxStageIndex)
            return;

        _currentUnlockedStage = stageNumber + 1;
        SaveGameData();
    }

    public void ResetGame()
    {
        UIManager.Instance.CloseUI(UIRootType.MainUI, UIType.MainUI);

        UIManager.Instance.RemoverAllHudSlot();
        StageManager.Instance.EndStage();
        GameObjectManager.Instance.DestroyAllObjectPool();
        TowerManager.Instance.DestroyAllTower();
    }

    private void SaveGameData()
    {
        PlayerPrefs.SetInt(SAVE_KEY, _currentUnlockedStage);
        PlayerPrefs.Save(); 
        Debug.Log($"[GameManager] 스테이지 저장 완료: {_currentUnlockedStage}");
    }
}