using UnityEngine;

public class StageUI : MonoBehaviour
{
    [SerializeField] private DaniTechUIButton Button_Stage1;
    [SerializeField] private DaniTechUIButton Button_Stage2;
    [SerializeField] private DaniTechUIButton Button_Stage3;
    [SerializeField] private DaniTechUIButton Button_Stage4;
    [SerializeField] private DaniTechUIButton Button_Stage5;

    [SerializeField] private DaniTechUIButton Button_Back;

    private void OnEnable()
    {
        Button_Back.BindOnClickButtonEvent(OnClickReturnLobby);
    }

    private void OnClickReturnLobby()
    {
        // UIManager.Instance.OpenUI(UIRootType.MainUI, UIType.LobbyUI);
        // UIManager.Instance.CloseContentUI(UIType.StageUI);
    }
}
