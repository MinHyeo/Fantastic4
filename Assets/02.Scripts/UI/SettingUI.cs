using UnityEngine;
using UnityEngine.UI;

public class SettingUI : UIBase
{
    [SerializeField] private UIButton Button_Close;
    [SerializeField] private Slider Slider_BGMSound;
    [SerializeField] private Slider Slider_SFXSound;

    private void OnEnable()
    {
        Button_Close.BindOnClickButtonEvent(OnClickCloseSettingUI);

        CurrentSoundVolume();

        Slider_BGMSound.onValueChanged.AddListener(OnChangedBGMVolume);
        Slider_SFXSound.onValueChanged.AddListener(OnChangedSFXVolume);
    }

    private void OnDisable()
    {
        Slider_BGMSound.onValueChanged.RemoveListener(OnChangedBGMVolume);
        Slider_SFXSound.onValueChanged.RemoveListener(OnChangedSFXVolume);
    }

    private void CurrentSoundVolume()
    {
        if (SoundManager.Instance != null)
        {
            Slider_BGMSound.value = SoundManager.Instance.BGMVolume;
            Slider_SFXSound.value = SoundManager.Instance.SFXVolume;
        }
    }

    private void OnChangedBGMVolume(float value)
    {
        SoundManager.Instance.SetBGMVolume(value);
    }
    private void OnChangedSFXVolume(float value)
    {
        SoundManager.Instance.SetSFXVolume(value);
    }

    private void OnClickCloseSettingUI()
    {
        UIManager.Instance.ClosePopupUI(UIType.SettingUI);
    }
}
