using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    public int CurrentUnlockedStage { get; private set; } = 1;

    private void Awake()
    {
        Instance = this;
    }

    public void UnlockStage(int stageNumber)
    {
        if (stageNumber > CurrentUnlockedStage)
        {
            CurrentUnlockedStage = stageNumber;
        }
    }
}
