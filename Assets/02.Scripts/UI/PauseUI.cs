using UnityEngine;
using UnityEngine.UI;

public class PauseUI : UIBase
{
    [SerializeField] private UIButton Button_Resume;
    [SerializeField] private UIButton Button_Setting;
    [SerializeField] private UIButton Button_Menu;

    private void OnEnable()
    {
        Button_Resume.BindOnClickButtonEvent(OnClickResumeGame);
        Button_Setting.BindOnClickButtonEvent(OnClickOpenSetting);
        Button_Menu.BindOnClickButtonEvent(OnClickGoToMenu);
    }

    private void OnClickResumeGame()
    {
        UIManager.Instance.ClosePopupUI(UIType.PauseUI);
        Time.timeScale = 1.0f;
    }

    private void OnClickOpenSetting()
    {
        UIManager.Instance.OpenPopupUI(UIType.SettingUI);
    }

    private void OnClickGoToMenu()
    {
        // TODO : 모든 창과 맵을 닫는 로직을 추가해야 함. 게임매니저에서 할 듯
        UIManager.Instance.OpenUI(UIRootType.MainUI, UIType.MainUI);
    }
}
