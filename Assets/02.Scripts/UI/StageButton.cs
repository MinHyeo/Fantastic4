using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class StageButton : MonoBehaviour
{
    public int StageNumber { get; private set; }
    private StageUI stageUI;
    private bool isLocked = false;

    public void InitStageButton(int stagenumber, StageUI stageui)
    {
        StageNumber = stagenumber;
        stageUI = stageui;

        UIButton button = GetComponent<UIButton>();
        if (button != null)
        {
            button.BindOnClickButtonEvent(OnClickThisButton);
        }
    }

    public void SetLockStage(bool lockStage, Color color)
    {
        isLocked = lockStage;

        if (TryGetComponent<Image>(out var image))
        {
            image.color = color;
        }
    }

    private void OnClickThisButton()
    {
        if (isLocked == true)
        {
            return;
        }

        stageUI.OnClickStageOpen(StageNumber);
    }
}
