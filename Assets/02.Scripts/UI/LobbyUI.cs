using UnityEditor.SceneManagement;
using UnityEngine;

public class LobbyUI : UIBase
{
    [SerializeField] private UIButton Button_Start;
    [SerializeField] private UIButton Button_Information;
    [SerializeField] private UIButton Button_Setting;
    [SerializeField] private UIButton Button_End;

    private void OnEnable()
    {
        Button_Start.BindOnClickButtonEvent(OnClickStartButton);
        Button_Information.BindOnClickButtonEvent(OnClickInformationButton);
        Button_Setting.BindOnClickButtonEvent(OnClickSettingButton);
        Button_End.BindOnClickButtonEvent(OnClickEndButton);
    }

    private void OnClickStartButton()
    {
        UIManager.Instance.OpenContentUI(UIType.StageUI);
        UIManager.Instance.CloseUI(UIRootType.MainUI, UIType.LobbyUI);
        UIManager.Instance.CloseUI(UIRootType.BackgroundUI, UIType.LobbyBackgroundUI);
    }
    private void OnClickInformationButton()
    {
        // UIManager.Instance.OpenContentUI(UIType.InformationUI);
    }

    private void OnClickSettingButton()
    {
        // UIManager.Instance.OpenContentUI(UIType.SettingUI);
    }
    private void OnClickEndButton()
    {
        // UIManager.Instance.OpenContentUI(UIType.EndUI);
    }

}
