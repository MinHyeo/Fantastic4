using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    private int _currentUnlockedStage = 1;
    public int CurrentUnlockedStage => _currentUnlockedStage;

    private void Awake()
    {
        Instance = this;
    }

    public void UnlockStage(int stageNumber)
    {
        if (stageNumber > _currentUnlockedStage)
        {
            _currentUnlockedStage = stageNumber;
        }
    }
}