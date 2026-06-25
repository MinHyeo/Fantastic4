using UnityEngine;
using UnityEngine.UI;

public class StageUI : UIBase
{
    [SerializeField] private UIButton[] Button_Stage;
    [SerializeField] private UIButton Button_Back;

    [SerializeField] private Color LockColor = new Color(0.3f, 0.3f, 0.3f, 1f);
    [SerializeField] private Color UnLockColor = Color.white;

    private void OnEnable()
    {
        Button_Back.BindOnClickButtonEvent(OnClickReturnLobby);

        RefreshStageButton();
    }

    private void RefreshStageButton()
    {
        int clearStage = PlayerPrefs.GetInt("ClearStage", 0);

        int openStageIndex = clearStage;

        for (int i = 0; i < Button_Stage.Length; i++)
        {
            Button buttonComponent = Button_Stage[i].GetComponent<Button>();
            Image imageComponent = Button_Stage[i].GetComponent<Image>();

            if (i <= openStageIndex)
            {
                if (buttonComponent != null)
                {
                    buttonComponent.interactable = true;
                }
                if (imageComponent != null)
                {
                    imageComponent.color = UnLockColor;
                }

                StageButton info = Button_Stage[i].GetComponent<StageButton>();
                if (info == null)
                {
                    info = Button_Stage[i].gameObject.AddComponent<StageButton>();
                }

                info.InitStageButton(i + 1, this);
            }
            else
            {
                if (buttonComponent != null)
                {
                    buttonComponent.interactable = false;
                }
                if (imageComponent != null)
                {
                    imageComponent.color = LockColor;
                }
            }
        }
    }

    public void OnClickStageOpen(int stageNumber)
    {
        // TODO : 스테이지 시작 로직 넣기
    }

    private void OnClickReturnLobby()
    {
        UIManager.Instance.OpenUI(UIRootType.MainUI, UIType.LobbyUI);
        UIManager.Instance.CloseContentUI(UIType.StageUI);
    }
}
