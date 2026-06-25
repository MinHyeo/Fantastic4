using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class MainUI_Tower : UIBase
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

    // 타워덱 생성 부분
    private void ReadTowerListAndCreateSlot()
    {
        //var dataList = GameDataManager.Instance.TowerDataList; // TODO : 데이터 매니저 호출 부분이 다르기에 바꿔야 함.
        //foreach (var dataKv in dataList)
        //{
        //    var data = dataKv.Value;
        //    if (data == null)
        //    {
        //        continue;
        //    }

        //    CreateTowerDeckSlot(data.Id);
        //}
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

        slotComponent.InitTowerDeck(dataId, OnClickTowerDeckSlotSelected);
        _deckList.Add(dataId, slotComponent);
    }

    private void OnClickTowerDeckSlotSelected(string slotDataId)
    {
        // TODO : 버튼 선택시 생성 부분
        
        foreach (var slotKv in _deckList)
        {
            var slot = slotKv.Value;
            var dataId = slot.GetTowerDataId();
            slot.SetSelected(slotDataId == dataId);
        }
    }

    // 타이머 설정 부분
    private void OnClickPauseGame()
    {
        UIManager.Instance.OpenPopupUI(UIType.PauseUI);
        Time.timeScale = 0f;
    }

    private void InitMainUI()
    {
        _currentTimer = 40.0f;
        _isTimerRunning = true;
        UpdateTimerText();
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
        // 타이머 종료후 이벤트 넣기
    }
}
