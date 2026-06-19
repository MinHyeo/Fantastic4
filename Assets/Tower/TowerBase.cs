using UnityEngine;

public enum PlacementPreviewRenderMode
{
    Invalid,
    Valid
}

public abstract partial class TowerBase : MonoBehaviour
{
    [SerializeField, ReadOnly] private MeshRenderer _renderer;

    [SerializeField, ReadOnly] private Animator _animator;

    [SerializeField] protected Detector _detector;

    [SerializeField] protected Dragger _dragger;


    public LayerMask PlacementLayerMask => _dragger.PlacementLayerMask;


    protected virtual void Awake()
    {
        _renderer = GetComponentInChildren<MeshRenderer>();
        _animator = GetComponentInChildren<Animator>();
        _detector ??= new();
    }

    protected virtual void OnEnable() { }

    protected virtual void OnDisable() { }

    protected virtual void Update()
    {
        _detector.FindEnemiesInRange();
    }

    protected virtual void OnDrawGizmos() { }
}
