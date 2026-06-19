using UnityEngine;

public class StageButton : MonoBehaviour
{
    public int StageNumber {  get; private set; }
    private StageUI stageUI;

    public void InitStageButton(int stagenumber, StageUI stageui)
    {
        StageNumber = stagenumber;
        stageUI = stageui;

        DaniTechUIButton button = GetComponent<DaniTechUIButton>();
        if (button != null)
        {
            button.BindOnClickButtonEvent(OnClickThisButton);
        }
    }

    private void OnClickThisButton()
    {
        stageUI.OnClickStageOpen(StageNumber);
    }
}
