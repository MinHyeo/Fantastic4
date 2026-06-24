using UnityEngine;
using TMPro;

public class MainUI_Tower : UIBase
{
    [SerializeField] private UIButton Button_Pause;
    [SerializeField] private TextMeshProUGUI Text_Wave;
    [SerializeField] private TextMeshProUGUI Text_Gold;
    [SerializeField] private TextMeshProUGUI Text_Timer;
    [SerializeField] private GameObject TowerDeck;

    // 타이머 변수


    private void OnEnable()
    {
        Button_Pause.BindOnClickButtonEvent(OnClickPauseGame);
    }
    
    private void OnClickPauseGame()
    {
        UIManager.Instance.OpenPopupUI(UIType.PauseUI);
        Time.timeScale = 0f;
    }

    private void InitMainUI()
    {

    }

}
