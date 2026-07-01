using UnityEngine;

public class ResultFailUI : UIBase
{
    [SerializeField] private UIButton Button_Retry;
    [SerializeField] private UIButton Button_Stage;
    [SerializeField] private UIButton Button_Main;

    private void OnEnable()
    {
        GameManager.Instance.ResetGame();

        Button_Retry.BindOnClickButtonEvent(Onclick_Retry);
        Button_Stage.BindOnClickButtonEvent(Onclick_Stage);
        Button_Main.BindOnClickButtonEvent(Onclick_Main);
    }

    private void Onclick_Stage()
    {
        UIManager.Instance.OpenContentUI(UIType.StageUI);
        UIManager.Instance.ClosePopupUI(UIType.ResultFailUI);
    }

    private void Onclick_Main()
    {
        UIManager.Instance.OpenUI(UIRootType.MainUI, UIType.LobbyUI);
        UIManager.Instance.OpenUI(UIRootType.BackgroundUI, UIType.LobbyBackgroundUI);
        UIManager.Instance.ClosePopupUI(UIType.ResultFailUI);
    }

    private void Onclick_Retry()
    {
        string currentStageId = StageManager.Instance.CurrentStageId;
        StageManager.Instance.LoadStage(currentStageId);

        UIManager.Instance.ClosePopupUI(UIType.ResultFailUI);
    }
}
