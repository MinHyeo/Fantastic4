using System;
using UnityEngine;

public class PlacementIndicator : MonoBehaviour
{
    [SerializeField] protected Dragger _dragger;

    [Header("랜더링 설정")]

    [SerializeField] protected Material _validPlacementMaterial;

    [SerializeField] protected Material _invalidPlacementMaterial;

    [SerializeField, ReadOnly] private GameObject _renderTarget;

    private PlacementPreviewRenderMode _renderMode = PlacementPreviewRenderMode.Invalid;


    protected void Awake()
    {
        _dragger ??= new Dragger();
    }

    protected void OnEnable()
    {
        SetPlacementPreviewMode(PlacementPreviewRenderMode.Invalid);
    }

    protected void OnDisable()
    {
        DestroyRenderTarget();
    }

    protected void Update()
    {
        // 마우스 누르고 있으면
        if (Input.GetMouseButton(0))
        {
            // 인디케이터를 드래그로 이동
            Camera mainCamera = Camera.main;
            bool hasHit = _dragger.Drag(mainCamera, out RaycastHit hit);

            // 배치 가능 여부에 따른 색상 변형
            if (hasHit && TowerManager.Instance.CanPlaceTower(hit, _dragger.PlacementLayerMask))
            {
                SetPlacementPreviewMode(PlacementPreviewRenderMode.Valid);
            }
            else
            {
                SetPlacementPreviewMode(PlacementPreviewRenderMode.Invalid);
            }
        }
    }

    /// <summary>
    /// 인디케이터용 타워를 인스턴싱, 설정
    /// </summary>
    public void SetRenderTarget(GameObject renderTargetPrefab, LayerMask placementLayerMask)
    {
        _renderTarget = Instantiate(renderTargetPrefab, transform);

        // 미리보기 콜라이더가 카메라 레이를 가로막지 않도록 비활성화
        Collider[] colliders = _renderTarget.GetComponentsInChildren<Collider>(true);
        foreach (Collider targetCollider in colliders)
        {
            targetCollider.enabled = false;
        }

        // 타워의 배치 가능 레이어를 읽어오기
        _dragger.SetPlacementLayerMask(placementLayerMask);
    }

    /// <summary>
    /// mode에 맞춰서 매쉬 랜더러의 material를 설정
    /// </summary>
    /// <param name="mode"></param>
    public void SetPlacementPreviewMode(PlacementPreviewRenderMode mode)
    {
        _renderMode = mode;

        switch (mode)
        {
            case PlacementPreviewRenderMode.Invalid:
                SetMaterial(_invalidPlacementMaterial);
                break;

            case PlacementPreviewRenderMode.Valid:
            default:
                SetMaterial(_validPlacementMaterial);
                break;
        }
    }

    /// <summary>
    /// 디케이터용 타워를 월드에 랜더링
    /// </summary>
    private void DestroyRenderTarget()
    {
        Destroy(_renderTarget);
    }

    /// <summary>
    /// 인디케이터의 모든 메쉬 랜더러를 material로 설정
    /// </summary>
    private void SetMaterial(Material material)
    {
        if (_renderTarget == null) return;

        MeshRenderer[] meshRenderers = _renderTarget.GetComponentsInChildren<MeshRenderer>(true);

        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            Material[] materials = meshRenderer.sharedMaterials;

            // 모든 머터리얼 슬롯을 같은 머터리얼로 교체
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = material;
            }

            // 변경된 머터리얼 배열을 다시 적용
            meshRenderer.sharedMaterials = materials;
        }
    }
}
