using UnityEngine;
using UnityEngine.UI;

public class SettingUI : UIBase
{
    [SerializeField] private UIButton Button_Close;
    [SerializeField] private Slider Slider_EnvironmentSound;
    [SerializeField] private Slider Slider_EffectSound;


    private void OnEnable()
    {
        Button_Close.BindOnClickButtonEvent(OnClickCloseSettingUI);
    }

    private void SoundControl()
    {
        // TODO : 사운드가 생기면 연동되도록 SoundManager에서 사용될 듯
    }

    private void OnClickCloseSettingUI()
    {
        UIManager.Instance.ClosePopupUI(UIType.SettingUI);
    }
}
