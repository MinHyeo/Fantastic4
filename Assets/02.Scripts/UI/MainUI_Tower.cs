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
    [SerializeField] private GameObject TowerDeck;
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
        
    }

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
