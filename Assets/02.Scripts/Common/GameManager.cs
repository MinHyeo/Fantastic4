using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    private const string SAVE_KEY = "CurrentUnlockedStage";

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
        if (stageNumber > _currentUnlockedStage)
        {
            _currentUnlockedStage = stageNumber;
            SaveGameData();
        }
    }

    private void SaveGameData()
    {
        PlayerPrefs.SetInt(SAVE_KEY, _currentUnlockedStage);
        PlayerPrefs.Save(); 
        Debug.Log($"[GameManager] 스테이지 저장 완료: {_currentUnlockedStage}");
    }
}