using TMPro;
using UnityEngine;

public class Hud_TowerLevelSlotUI : MonoBehaviour
{
    [SerializeField] private int slotOffsetX;
    [SerializeField] private int slotOffsetY;
    [SerializeField] private TextMeshProUGUI Text_TowerLevel;

    private int _instanceId;
    private Transform _targetTransform;

    public void InitSlot(int instanceId, Transform targetTransform)
    {
        _instanceId = instanceId;
        _targetTransform = targetTransform;
        slotOffsetX = -45;
        slotOffsetY = 10;
        Text_TowerLevel.text = $"Lv : 1";

        TryBingStatChangedEvent(targetTransform.gameObject);
    }

    private void TryBingStatChangedEvent(GameObject gObj)
    {
        // TODO : 타워 클래스와 연동 필요
        //var tower = gObj.GetComponent<Tower>();
        //if (tower != null)
        //{
        //    tower.BindeOnStatChangedEvent(OnTargetLevleChanged);
        //    return;
        //}
    }

    private void OnTargetLevleChanged(int currentLevel)
    {
        Text_TowerLevel.text = $"Lv : {currentLevel}";
    }
    
    private void Update()
    {
        // TODO : 레벨 반영 때 교체되도록 실행
    }
}
