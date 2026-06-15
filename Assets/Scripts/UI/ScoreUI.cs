using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private Text Text_CurrentScore;

    public void AddGameScore(int currentScore)
    {
        Text_CurrentScore.text = $"잡은 피그미수 : {currentScore}";

        GameManager.Instance.IncreasePlayerExp(10);

        // 10개 잡을 때마다 자동 저장 시도
        if((currentScore % 10) == 0)
        {
            GameManager.Instance.SaveData();
            Debug.LogWarning("저장 시도!");
        }
    }
 
}
