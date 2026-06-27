using UnityEngine;
using UnityEngine.EventSystems;

public enum PlacementPreviewRenderMode
{
    Invalid,
    Valid
}

public abstract partial class TowerBase : MonoBehaviour, IPointerClickHandler
{
    [Header(nameof(TowerBase))]

    [SerializeField, ReadOnly] private MeshRenderer _renderer;

    [SerializeField, ReadOnly] private Animator _animator;

    [SerializeField] protected LayerMask _placementLayerMask;

    [SerializeField] protected Detector _detector;

    [SerializeField] protected Dragger _dragger;

    [SerializeField] protected Rotator _rotator;

    [SerializeField] protected TowerData _data;

    [SerializeField] protected TowerRangeVisualizer _rangeVisualizer;


    public TowerData Data => _data;

    public LayerMask PlacementLayerMask => _placementLayerMask;


    protected virtual void Awake()
    {
        _renderer = GetComponentInChildren<MeshRenderer>();
        _animator = GetComponentInChildren<Animator>();
    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update() { }

    protected virtual void OnEnable() { }

    protected virtual void OnDisable() { }

    protected virtual void OnDrawGizmos() { }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Test");
    }
}
