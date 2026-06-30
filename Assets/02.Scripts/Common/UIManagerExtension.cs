using UnityEngine;

public enum UIRootType
{
    None = 0,
    BackgroundUI,
    MainUI,
    ContentUI,
    PopupUI,
    VeryFrontUI
}

public enum UIType
{
    SimplePopup,
    LobbyBackgroundUI,
    MainUI,
    LobbyUI,
    HudMainUI,
    StageUI,
    ResultFailUI,
    ResultSuccessUI,
    TowerUpgradeUI,
    TowerInfoPopupUI,
    EnemyInfoPopupUI,
    PauseUI,
    SettingUI
}

public static class UIManagerExtension
{
    public static string GetUIPath(this UIManager uiManager, UIRootType uiRootType, UIType uiType)
    {
        string path = string.Empty; // "" == string.Empty

        // 신규UI추가 2) Resources.Load를 할 경로를 직접 명시한다
        // 해당 경로는 프로젝트창에서 Resources/Prefabs/UI폴더 내에 있는 RootType 폴더명과 UIType 프리팹 이름과 동일해야 한다! (ex. ContentUI/DNMyProfilePopup)
        path = $"UI/{uiRootType}/{uiType}";
        return path;
    }

    public static void ShowStartupUIOnGameStart(this UIManager uiManager)
    {
        
        uiManager.OpenUI(UIRootType.MainUI, UIType.LobbyUI);
        uiManager.OpenUI(UIRootType.BackgroundUI, UIType.LobbyBackgroundUI);
        // 게임 로비 UI를 여기서 오픈해주자 -> uiManager.
        // MainUI도
    }

    // 신규UI추가 3) 이렇게 어떤 팝업을 열고, 열때 전달해야하는 파라미터가 있다면 이렇게 전달한다.
    // 추가하기 편하게 그냥 빼둔 확장 메서드이므로, uiManager과 this는 우선 넘어가자

    public static void AddHudSlot(this UIManager uIManager, int instanceId, Transform targetTransform)
    {
        var uiBase = uIManager.GetOpenUI(UIRootType.MainUI, UIType.HudMainUI);
        if (uiBase == null) return;

        if (uiBase is HudMainUI hudMainUI)
        {
            hudMainUI.AddHudHpSlot(instanceId, targetTransform);
        }
    }

    public static void RemoveHudSlot(this UIManager uIManager, int instanceId)
    {
        var uiBase = uIManager.GetOpenUI(UIRootType.MainUI, UIType.HudMainUI);
        if (uiBase == null) return;

        if (uiBase is HudMainUI hudMainUI)
        {
            hudMainUI.RemoveHudHpSlot(instanceId);
        }
    }
}
