using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudMainUI : UIBase
{
    [SerializeField] private GameObject Prefab_HudHpSlot;
    [SerializeField] private GameObject Prefab_HudTowerLevelSlot;
    [SerializeField] private Transform Transform_SlotRoot;

    private Dictionary<int, Hud_HpSlotUI> _hudHpSlotList = new Dictionary<int, Hud_HpSlotUI> ();
    private Dictionary<int, Hud_TowerLevelSlotUI> _hudTowerLevelSlotList = new Dictionary<int, Hud_TowerLevelSlotUI>();

    public void AddHudHpSlot(int instanceId, Transform targetTransform)
    {
        CreateHudHpSlot(instanceId, targetTransform);
    }

    private void CreateHudHpSlot(int instanceId, Transform targetTransform)
    {
        var gObj = Instantiate(Prefab_HudHpSlot, Transform_SlotRoot);
        if (gObj == null)
        {
            return;
        }

        var slotComponent = gObj.GetComponent<Hud_HpSlotUI>();
        if (slotComponent == null)
        {
            return;
        }

        slotComponent.InitSlot(instanceId, targetTransform);

        _hudHpSlotList.Add(instanceId, slotComponent);
    }
    public void RemoveHudHpSlot(int instanceId)
    {
        if (_hudHpSlotList.ContainsKey(instanceId) == true)
        {
            var slot = _hudHpSlotList[instanceId];

            Destroy(slot.gameObject);
            _hudHpSlotList.Remove(instanceId);
        }
    }

    public void AddHudTowerLevelSlot(int instanceId, Transform targetTransform)
    {
        CreateHudTowerLevelSlot(instanceId, targetTransform);
    }

    private void CreateHudTowerLevelSlot(int instanceId, Transform targetTransform)
    {
        var gObj = Instantiate(Prefab_HudTowerLevelSlot, Transform_SlotRoot);
        if (gObj == null) 
        {
            return;
        }

        var slotComponent = gObj.GetComponent<Hud_TowerLevelSlotUI>();
        if (slotComponent == null)
        {
            return;
        }

        slotComponent.InitSlot(instanceId, targetTransform);

        _hudTowerLevelSlotList.Add(instanceId, slotComponent);
    }

    public void RemoveHudTowerLevelSlot(int instanceId)
    {
        if (_hudTowerLevelSlotList.ContainsKey(instanceId) == true)
        {
            var slot = _hudTowerLevelSlotList[instanceId];

            Destroy(slot.gameObject);
            _hudTowerLevelSlotList.Remove(instanceId);
        }
    }

    public void RemoveAllHudHpSlot()
    {
        foreach (var slot in _hudHpSlotList)
        {
            var slotKv = slot.Value;

            if (slotKv != null)
            {
                Destroy(slotKv.gameObject);
            }
        }

        _hudHpSlotList.Clear();
    }
}
