using System.Collections.Generic;
using UnityEngine;

public class HudMainUI : UIBase
{
    [SerializeField] private GameObject Prefab_HudSlot;
    [SerializeField] private Transform Transform_SlotRoot;

    private Dictionary<int, HudSlotUI> _hudSlotList = new Dictionary<int, HudSlotUI> ();

    public void AddHudSlot(int instanceId, Transform targetTransform)
    {
        CreateHudSlot(instanceId, targetTransform);
    }

    private void CreateHudSlot(int instanceId, Transform targetTransform)
    {
        var gObj = Instantiate(Prefab_HudSlot, Transform_SlotRoot);
        if (gObj == null) return;

        var slotComponent = gObj.GetComponent<HudSlotUI>();
        if (slotComponent == null) return;

        slotComponent.InitSlot(instanceId, targetTransform);

        _hudSlotList.Add(instanceId, slotComponent);
    }

    public void RemoveHudSlot(int instanceId)
    {
        if (_hudSlotList.ContainsKey(instanceId) == true)
        {
            var slot = _hudSlotList[instanceId];

            Destroy(slot.gameObject);
            _hudSlotList.Remove(instanceId);
        }
    }
}
