using UnityEngine;
using UnityEngine.UI;

public class Hud_HpSlotUI : MonoBehaviour
{
    [SerializeField] private int slotOffsetX;
    [SerializeField] private int slotOffsetY;
    [SerializeField] private Slider Slider_Hp;

    private int _instanceId;
    private Transform _targetTransform;

    public void InitSlot(int instanceId, Transform targetTransform)
    {
        _instanceId = instanceId;
        _targetTransform = targetTransform;
        slotOffsetX = -45;
        slotOffsetY = 10;

        TryBingStatChangedEvent(targetTransform.gameObject);
    }

    private void TryBingStatChangedEvent(GameObject gObj)
    {
        // TODO : 에너미 클래스와 연동 필요
        //var enemy = gObj.GetComponent<Enemy>();
        //if (enemy != null)
        //{
        //    enemy.BindeOnStatChangedEvent(OnTargetEntitiyHpChanged);
        //    return;
        //}
    }

    private void OnTargetEntitiyHpChanged(int curHp, int maxHp)
    {
        Slider_Hp.value = (curHp / (float)maxHp);
    }

    private void Update()
    {
        if (_targetTransform != null)
        {
            Vector2 screenPos = Camera.main.WorldToScreenPoint(_targetTransform.position);

            var rectTransform = this.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                Vector2 finalScreenPos = new Vector2(screenPos.x + slotOffsetX, screenPos.y + slotOffsetY);
                rectTransform.anchoredPosition = finalScreenPos;
            }
        }
    }
}
