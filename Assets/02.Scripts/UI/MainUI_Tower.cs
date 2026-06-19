using UnityEngine;
using TMPro;

public class MainUI_Tower : UIBase
{
    [SerializeField] private DaniTechUIButton Button_Pause;
    [SerializeField] private TextMeshProUGUI Text_Wave;
    [SerializeField] private TextMeshProUGUI Text_Gold;
    [SerializeField] private GameObject TowerDeck;

    private void OnEnable()
    {
        Button_Pause.BindOnClickButtonEvent(OnClickPauseGame);
    }
    
    private void OnClickPauseGame()
    {
        // UIManager.Instance.OpenPopupUI(UIType.PauseUI);
        // Todo : 게임 일시정지 기능도 넣어야 함.
    }

    private void InitMainUI()
    {

    }

}
