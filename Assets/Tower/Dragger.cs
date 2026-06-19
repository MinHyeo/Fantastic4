using System;
using UnityEngine;

[Serializable]
public class Dragger
{
    [SerializeField] private Transform _ownerTransform;

    [SerializeField] private LayerMask _placementLayerMask;

    [SerializeField] private float _fallbackDistance = 10f;


    public LayerMask PlacementLayerMask => _placementLayerMask;



    /// <summary>
    /// 현재 타워 프리팹의 배치 가능 레이어를 설정합니다.
    /// </summary>
    public void SetPlacementLayerMask(LayerMask placementLayerMask)
    {
        _placementLayerMask = placementLayerMask;
    }

    /// <summary>
    /// 마우스 위치의 월드 충돌점으로 미리보기를 이동합니다.
    /// 충돌이 없으면 화면에 계속 보이도록 카메라 앞의 고정 거리로 이동합니다.
    /// </summary>
    public bool Drag(Camera camera, out RaycastHit hit)
    {
        hit = default;

        if (camera == null || _ownerTransform == null)
        {
            return false;
        }

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            _ownerTransform.position = hit.point;
            return true;
        }

        float distance = Mathf.Max(_fallbackDistance, camera.nearClipPlane + 0.01f);
        _ownerTransform.position = ray.GetPoint(distance);
        return false;
    }
}
