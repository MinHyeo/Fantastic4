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

    [SerializeField, ReadOnly] protected Detector _detector;

    [SerializeField, ReadOnly] protected RangeVisualizer _rangeVisualizer;

    [SerializeField] protected LayerMask _placementLayerMask;

    [SerializeField] protected Dragger _dragger;

    [SerializeField] protected Rotator _rotator;

    [SerializeField] protected TowerData _data;

    private bool _isToggleRangeVisualizer = false;


    public TowerData Data => _data;

    public LayerMask PlacementLayerMask => _placementLayerMask;


    protected virtual void Awake()
    {
        _renderer = GetComponentInChildren<MeshRenderer>(true);
        _animator = GetComponentInChildren<Animator>(true);
        _detector = GetComponentInChildren<Detector>(true);
        _rangeVisualizer = GetComponentInChildren<RangeVisualizer>(true);
    }

    public virtual void Init(string towerId)
    {
        _data = GameDataManager.Instance.GetData<TowerData>(towerId);

        _detector.SetRange(_data.AttackRange);

        _rangeVisualizer.SetRadius(_data.AttackRange);
        _rangeVisualizer.SetVisible(false);
        _isToggleRangeVisualizer = false;
    }

    protected virtual void Start() { }

    protected virtual void Update() { }

    protected virtual void OnEnable() { }

    protected virtual void OnDisable() { }

    protected virtual void OnDrawGizmos() { }

    public void OnPointerClick(PointerEventData eventData)
    {
        ToggleRangeVisualizer();
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

    public void ToggleRangeVisualizer()
    {
        if (_rangeVisualizer == null)
        {
            return;
        }

        _isToggleRangeVisualizer = _isToggleRangeVisualizer ? false : true;
        _rangeVisualizer.SetVisible(_isToggleRangeVisualizer);
    }
}
