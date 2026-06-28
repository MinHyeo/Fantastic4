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
        if (GameManager.Instance == null)
        {
            return;
        }

        int unlockedStage = GameManager.Instance.CurrentUnlockedStage;

        for (int i = 0; i < Button_Stage.Length; i++)
        {
            if (Button_Stage[i] == null)
            {
                continue;
            }

            int stageNumber = i + 1;

            StageButton stageButton = Button_Stage[i].GetComponent<StageButton>();
            if(stageButton != null)
            {
                stageButton.InitStageButton(stageNumber, this);

                if (stageNumber > unlockedStage)
                {
                    stageButton.SetLockStage(true, LockColor);
                }
                else
                {
                    stageButton.SetLockStage(false, UnLockColor);
                }
            }
        }
    }

    public void OnClickStageOpen(int stageNumber)
    {
        string stageId = $"stage_{stageNumber:D2}";

        StageManager.Instance.LoadStage(stageId);

        UIManager.Instance.CloseContentUI(UIType.StageUI);
    }

    private void OnClickReturnLobby()
    {
        UIManager.Instance.OpenUI(UIRootType.MainUI, UIType.LobbyUI);
        UIManager.Instance.CloseContentUI(UIType.StageUI);
    }
}
