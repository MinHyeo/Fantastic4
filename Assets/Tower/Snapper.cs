using System;
using UnityEngine;

[Serializable]
public class Snapper
{
    private LayerMask snapLayerMask;


    /// <summary>
    /// 현재 타워가 snap할 수 있는 레이어를 설정합니다.
    /// </summary>
    public void SetSnapLayerMask(LayerMask layerMask)
    {
        snapLayerMask = layerMask;
    }

    /// <summary>
    /// 마우스 레이가 snap 가능한 레이어에 닿으면 첫번째 대상으로 targetTransform을 이동합니다.
    /// </summary>
    public bool Snap(Camera camera, Transform targetTransform, out RaycastHit snapHit)
    {
        snapHit = default;

        if (camera == null || targetTransform == null)
        {
            return false;
        }

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);


        // snap 가능한 레이어에 닿으면 첫번째 대상으로 targetTransform을 이동합니다.
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, snapLayerMask);
        foreach (RaycastHit hit in hits)
        {
            Vector3 snappedWorldPos = hit.transform.position;
            snappedWorldPos.y += hit.collider.bounds.size.y / 2f;
            targetTransform.position = snappedWorldPos;
            snapHit = hit;
            return true;
        }

        return false;
    }
}
