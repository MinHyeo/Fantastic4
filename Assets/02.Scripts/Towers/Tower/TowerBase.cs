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

    [SerializeField] protected Transform _firePoint;

    [SerializeField] protected LayerMask _placementLayerMask;

    [SerializeField] protected Dragger _dragger;

    [SerializeField] protected Rotator _rotator;

    [SerializeField] protected TowerData _data;

    protected bool _isToggleRangeVisualizer = false;


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
        SetRangeVisualizerVisible(false);
    }

    protected virtual void Start() { }

    protected virtual void Update() { }

    protected virtual void OnEnable() { }

    protected virtual void OnDisable()
    {
        if (TowerManager.Instance != null)
        {
            TowerManager.Instance.ClearSelectedTower(this);
        }
    }

    protected virtual void OnDrawGizmos() { }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (TowerManager.Instance != null)
        {
            eventData.Use();
            return;
        }

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

    public virtual void ToggleRangeVisualizer()
    {
        if (TowerManager.Instance != null)
        {
            TowerManager.Instance.ToggleTowerRangeVisualizer(this);
            return;
        }

        SetRangeVisualizerVisible(!_isToggleRangeVisualizer);
    }

    /// <summary>
    /// 사거리 표시 오브젝트와 내부 선택 상태를 함께 갱신합니다.
    /// </summary>
    public void SetRangeVisualizerVisible(bool isVisible)
    {
        if (_rangeVisualizer == null)
        {
            return;
        }

        _isToggleRangeVisualizer = isVisible;
        _rangeVisualizer.SetVisible(_isToggleRangeVisualizer);
    }
}
