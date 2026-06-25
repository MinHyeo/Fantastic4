using UnityEngine;

public class StageStartTest : MonoBehaviour
{
    [SerializeField] private string _stageId;

    public void StartStage()
    {
        StageManager.Instance.LoadStage(_stageId);
    }
}
