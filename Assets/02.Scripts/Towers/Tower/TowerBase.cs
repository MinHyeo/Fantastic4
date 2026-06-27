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

    private bool _isToggleRangeVisualizer = false;


    public TowerData Data => _data;

    public LayerMask PlacementLayerMask => _placementLayerMask;


    protected virtual void Awake()
    {
        _renderer = GetComponentInChildren<MeshRenderer>();
        _animator = GetComponentInChildren<Animator>();
    }

    public virtual void Init(string towerId)
    {
        _data = GameDataManager.Instance.GetData<TowerData>(towerId);
    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update() 
    { 
       
    }

    protected virtual void OnEnable() { }

    protected virtual void OnDisable() { }

    protected virtual void OnDrawGizmos() { }

    public void OnPointerClick(PointerEventData eventData)
    {
        _isToggleRangeVisualizer = _isToggleRangeVisualizer ? false : true;
        ToggleRangeVisualizer(_isToggleRangeVisualizer);
    }

    public void Upgrade()
    {
        if (_data == null)
        {
            return;
        }

        string nextId = _data.UpgradeId;
        if (string.IsNullOrEmpty(nextId))
        {
            Debug.Log("이미 최대 강화상태임");
            return;
        }

        Init(nextId);

        Debug.LogWarning($"강화 후 데미지{_data.AttackDamage}, 사거리 {_data.AttackRange}, 공속 {_data.AttackSpeed}, 투사체 속도 {_data.ProjectileSpeed}");
    }

    public void ToggleRangeVisualizer(bool show)
    {
        if (_rangeVisualizer == null)
        {
            return;
        }

        if (show)
        {
            _rangeVisualizer.ShowRange();
        }
        else
        {
            _rangeVisualizer.HideRange();
        }
    }
}
