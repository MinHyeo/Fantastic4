using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Unity.Collections;

public class MainUI : UIBase
{
    [SerializeField] private UIButton Button_Pause;
    [SerializeField] private TextMeshProUGUI Text_Wave;
    [SerializeField] private TextMeshProUGUI Text_Gold;
    [SerializeField] private TextMeshProUGUI Text_Timer;

    [Header("타워덱")]
    [SerializeField] private GameObject Prefab_TowerDeck;
    [SerializeField] private Transform Transform_TowerDeckRoot;
    private Dictionary<string, TowerDeck> _deckList = new Dictionary<string, TowerDeck>();

    private int _stageGold = 0;

    // 타이머 변수
    private float _currentTimer = 40.0f;
    private bool _isTimerRunning = false;

    private void OnEnable()
    {
        Button_Pause.BindOnClickButtonEvent(OnClickPauseGame);
        InitMainUI();
    }

    private void Update()
    {
        FinalTimeUpdate();
    }

    private void OnDisable()
    {
        if (_deckList.Count > 0)
        {
            foreach (var slotKv in _deckList)
            {
                var slot = slotKv.Value;
                DestroyImmediate(slot.gameObject);
            }

            _deckList.Clear();
        }
    }

    private void InitMainUI()
    {
        ReadTowerListAndCreateSlot();
        SetStageGoldData();

        Text_Gold.text = $"{_stageGold}";
        _isTimerRunning = true;
        UpdateTimerText();
    }

    // 골드 부분
    private void SetStageGoldData()
    {
        var stageIdList = GameDataManager.Instance.GetStageIds();
        foreach (var stageId in stageIdList)
        {
            if (stageId == null)
            {
                continue;
            }

            GetStageGoldData(stageId);
        }
    }

    private void GetStageGoldData(string stageId)
    {
        var stagedata = GameDataManager.Instance.GetData<StageData>(stageId);
        _stageGold = stagedata.StartGold;
    }

    public void IncreseGold(int gold)
    {
        _stageGold += gold;
    }

    public void DecreaseGold(int gold)
    {
        _stageGold -= gold;
    }

    // 타워덱 생성 부분
    private void ReadTowerListAndCreateSlot()
    {
        var towerIdList = GameDataManager.Instance.GetAllTowerIds();
        foreach (var towerId in towerIdList)
        {
            if (towerId == null)
            {
                continue;
            }

            if (!towerId.Contains("Level1"))
            {
                continue;
            }

            CreateTowerDeckSlot(towerId);
        }
    }

    private void CreateTowerDeckSlot(string towerId)
    {
        var gObj = Instantiate(Prefab_TowerDeck, Transform_TowerDeckRoot);
        if (gObj == null)
        {
            return;
        }

        var slotComponent = gObj.GetComponent<TowerDeck>();
        if (slotComponent == null)
        {
            return;
        }

        var towerUIComponent = gObj.GetComponent<TowerDragUI>();
        towerUIComponent.SetTowerID(towerId);

        slotComponent.InitTowerDeck(towerId);
        _deckList.Add(towerId, slotComponent);
    }

    // 타이머 설정 부분
    private void OnClickPauseGame()
    {
        UIManager.Instance.OpenPopupUI(UIType.PauseUI);
        Time.timeScale = 0f;
    }

    public void SetTimerTime(float time)
    {
        _currentTimer = time;
    }

    private void FinalTimeUpdate()
    {
        if (_isTimerRunning)
        {
            UpdateTimer();
        }
    }

    private void UpdateTimer()
    {
        if (_currentTimer > 0f)
        {
            _currentTimer -= Time.deltaTime;

            if (_currentTimer <= 0f)
            {
                _currentTimer = 0f;
                _isTimerRunning = false;
                OnTimerFinished();
            }

            UpdateTimerText();
        }
    }

    private void UpdateTimerText()
    {
        Text_Timer.text = Mathf.CeilToInt(_currentTimer).ToString();
    }

    private void OnTimerFinished()
    {
        // TODO : 타이머 종료후 이벤트 넣기
    }
}
