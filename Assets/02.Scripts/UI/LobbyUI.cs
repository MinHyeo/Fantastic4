using UnityEditor.SceneManagement;
using UnityEngine;

public class LobbyUI : UIBase
{
    [SerializeField] private DaniTechUIButton Button_Start;
    [SerializeField] private DaniTechUIButton Button_Information;
    [SerializeField] private DaniTechUIButton Button_Setting;
    [SerializeField] private DaniTechUIButton Button_End;

    private void OnEnable()
    {
        Button_Start.BindOnClickButtonEvent(OnClickStartButton);
        Button_Information.BindOnClickButtonEvent(OnClickInformationButton);
        Button_Setting.BindOnClickButtonEvent(OnClickSettingButton);
        Button_End.BindOnClickButtonEvent(OnClickEndButton);


    }

    private void OnClickStartButton()
    {
        // UIManager.Instance.OpenContentUI(UIType.StageUI);
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
