using UnityEngine;

public class ResultFailUI : UIBase
{
    [SerializeField] private UIButton Button_Retry;
    [SerializeField] private UIButton Button_Stage;
    [SerializeField] private UIButton Button_Main;

    private void OnEnable()
    {
        Button_Retry.BindOnClickButtonEvent(Onclick_Retry);
        Button_Stage.BindOnClickButtonEvent(Onclick_Stage);
        Button_Main.BindOnClickButtonEvent(Onclick_Main);
    }

    private void Onclick_Stage()
    {
        Debug.Log("재시도 버튼이 눌렸어요");
    }

    private void Onclick_Main()
    {
        Debug.Log("메뉴 버튼이 눌렸어요");

    }

    private void Onclick_Retry()
    {
        Debug.Log("리트라잇 버튼이 눌렸어요");

    }
}
