using UnityEngine;

public class PlacementIndicator : MonoBehaviour
{
    [SerializeField] protected Dragger _dragger;

    [SerializeField] private Snapper _snapper;

    [Header("랜더링 설정")]

    [SerializeField] protected Material _validPlacementMaterial;

    [SerializeField] protected Material _invalidPlacementMaterial;

    [SerializeField, ReadOnly] private GameObject _renderTarget;

    protected LayerMask _placementLayerMask;

    private bool _canPlaceTower = false;

    private PlacementPreviewRenderMode _renderMode = PlacementPreviewRenderMode.Invalid;



    public bool CanPlaceTower => _canPlaceTower;



    protected void Awake()
    {
        _dragger ??= new Dragger();
        _snapper ??= new Snapper();
    }

    protected void OnEnable()
    {
        ResetPlacementState();
    }

    protected void OnDisable()
    {
        ClearRenderTarget();
        ResetPlacementState();
    }

    protected void Update()
    {
        // 마우스 누르고 있으면
        if (Input.GetMouseButton(0))
        {
            Camera mainCamera = Camera.main;
            _dragger.Drag(mainCamera, out _);

            // snap 가능한 레이어에 닿았을 때만 인디케이터를 snap 위치로 이동합니다.
            bool hasSnapHit = _snapper.Snap(mainCamera, transform, out RaycastHit snapHit);
            if (hasSnapHit)
            {
                transform.position = TowerManager.Instance.GetGridSnappedPosition(snapHit.collider);
            }

            if (hasSnapHit && TowerManager.Instance.CanPlaceTower(snapHit.collider, _placementLayerMask))
            {
                _canPlaceTower = true;
                SetPlacementPreviewMode(PlacementPreviewRenderMode.Valid);
            }
            else
            {
                _canPlaceTower = false;
                SetPlacementPreviewMode(PlacementPreviewRenderMode.Invalid);
            }
        }
    }

    /// <summary>
    /// 인디케이터용 타워를 인스턴싱, 설정
    /// </summary>
    public void SetRenderTarget(GameObject renderTargetPrefab, LayerMask placementLayerMask)
    {
        ClearRenderTarget();

        _renderTarget = Instantiate(renderTargetPrefab, transform, false);
        _renderTarget.transform.localPosition = Vector3.zero;
        _renderTarget.transform.localRotation = Quaternion.identity;

        // 미리보기는 시각화 전용이므로 타워 공격/탐지 같은 기능 스크립트를 끕니다.
        MonoBehaviour[] behaviours = _renderTarget.GetComponentsInChildren<MonoBehaviour>(true);
        foreach (MonoBehaviour behaviour in behaviours)
        {
            behaviour.enabled = false;
        }

        // 미리보기 콜라이더가 카메라 레이를 가로막지 않도록 비활성화
        Collider[] colliders = _renderTarget.GetComponentsInChildren<Collider>(true);
        foreach (Collider targetCollider in colliders)
        {
            targetCollider.enabled = false;
        }

        // 타워의 배치 가능 레이어를 읽어오기
        _placementLayerMask = placementLayerMask;
        _snapper.SetSnapLayerMask(placementLayerMask);
        SetPlacementPreviewMode(PlacementPreviewRenderMode.Invalid);
    }

    /// <summary>
    /// mode에 맞춰서 렌더러의 material를 설정
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
    private void ResetPlacementState()
    {
        _canPlaceTower = false;
        transform.position = Vector3.zero;
        SetPlacementPreviewMode(PlacementPreviewRenderMode.Invalid);
    }

    private void ClearRenderTarget()
    {
        if (_renderTarget == null) return;

        Destroy(_renderTarget);
        _renderTarget = null;
    }

    /// <summary>
    /// 인디케이터의 모든 렌더러를 material로 설정
    /// </summary>
    private void SetMaterial(Material material)
    {
        if (_renderTarget == null) return;

        Renderer[] renderers = _renderTarget.GetComponentsInChildren<Renderer>(true);

        foreach (Renderer targetRenderer in renderers)
        {
            Material[] materials = targetRenderer.sharedMaterials;

            // 모든 머터리얼 슬롯을 같은 머터리얼로 교체
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = material;
            }

            // 변경된 머터리얼 배열을 다시 적용
            targetRenderer.sharedMaterials = materials;
        }
    }
}
