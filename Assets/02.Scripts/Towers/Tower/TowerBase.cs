using UnityEngine;

public enum PlacementPreviewRenderMode
{
    Invalid,
    Valid
}

public abstract partial class TowerBase : MonoBehaviour
{
    [Header(nameof(TowerBase))]

    [SerializeField, ReadOnly] private MeshRenderer _renderer;

    [SerializeField, ReadOnly] private Animator _animator;

    [SerializeField] protected LayerMask _placementLayerMask;

    [SerializeField] protected Detector _detector;

    [SerializeField] protected Dragger _dragger;

    [SerializeField] protected TowerRangeVisualizer _rangeVisualizer;


    public LayerMask PlacementLayerMask => _placementLayerMask;


    protected virtual void Awake()
    {
        _renderer = GetComponentInChildren<MeshRenderer>();
        _animator = GetComponentInChildren<Animator>();
        _detector ??= new();
    }

    protected virtual void Start()
    {

    }

    protected virtual void OnEnable() { }

    protected virtual void OnDisable() { }

    protected virtual void Update()
    {
        _detector.FindEnemiesInRange();
    }

    protected virtual void OnDrawGizmos() { }
}
