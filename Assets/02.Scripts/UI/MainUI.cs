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
    
    // 타이머 변수
    private float _currentTimer;
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

        _currentTimer = 40.0f;
        _isTimerRunning = true;
        UpdateTimerText();
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

            CreateTowerDeckSlot(towerId);
        }
    }

    private void CreateTowerDeckSlot(string dataId)
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

        slotComponent.InitTowerDeck(dataId);
        _deckList.Add(dataId, slotComponent);
    }

    // 타이머 설정 부분
    private void OnClickPauseGame()
    {
        UIManager.Instance.OpenPopupUI(UIType.PauseUI);
        Time.timeScale = 0f;
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
