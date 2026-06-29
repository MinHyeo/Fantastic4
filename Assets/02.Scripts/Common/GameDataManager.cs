using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance { get; set; }

    private void Awake()
    {
        Instance = this;

        // +++ C# 콘솔때와 다르게 이제 Main()함수가 아닌
        // 모노의 메서드에서 호출될 수 있으므로, 데이터 매니저가 활성화되면 바로 모든 데이터를 한번 받아오자
        // 이처리는 원하는 시점이 있다면 이전해도 된다
        GameUtil.LoadFullData();
    }

    // --- JsonUtility의 한계를 극복하기 위한 Wrapper 클래스 ---
    [Serializable]
    private class SerializationWrapper<T>
    {
        public List<T> items; // JSON 파일의 루트 키 이름이 "items"여야 함
    }
    // ---------------------------------------------------

    private Dictionary<string, object> _dataList = new Dictionary<string, object>();

    private List<string> GetAllId<T>() where T : GameDataBase
    {
        string type = typeof(T).FullName;
        object dictObj = null;
        if (_dataList.TryGetValue(type, out dictObj))
        {
            var dict = dictObj as Dictionary<string, T>;
            return dict.Keys.ToList();
        }
        return null;
    }

    public List<string> GetAllTowerIds()
    {
        return GetAllId<TowerData>();
    }
    public List<string> GetEntityIds()
    {
        return GetAllId<EntityData>();
    }

    public List<string> GetEnemyIds()
    {
        return GetAllId<EnemyData>();
    }

    public List<string> GetStageIds()
    {
        return GetAllId<StageData>();
    }

    private Dictionary<string, T> LoadJsonData<T>(string tableName) where T : GameDataBase
    {
        // 1. 경로 설정 (확장자 .json 제외!)
        // Resources/JsonOutput 폴더
        string resourcePath = $"JsonOutput/{tableName}";

        // 2. 리소스 로드
        TextAsset textAsset = Resources.Load<TextAsset>(resourcePath);

        // 3. 파일 존재 여부 체크
        if (textAsset == null)
        {
            Debug.LogError($"[Error] 리소스를 찾을 수 없습니다: Resources/{resourcePath}");
            return new Dictionary<string, T>();
        }

        try
        {
            string jsonString = textAsset.text;

            // 4. JsonUtility용 Wrapper 트릭 적용
            string wrappedJson = "{\"items\":" + jsonString + "}";
            SerializationWrapper<T> wrapper = JsonUtility.FromJson<SerializationWrapper<T>>(wrappedJson);

            if (wrapper != null && wrapper.items != null)
            {
                Debug.Log($"{typeof(T).Name} 데이터를 {wrapper.items.Count}개 로드했습니다.");
                // ToDictionary를 사용하려면 각 클래스(T)에 Id 필드가 있어야 합니다.
                return wrapper.items.ToDictionary(item => item.Id.ToString());
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"[{typeof(T).Name} JSON 로드 오류] {ex.Message}");
        }

        return new Dictionary<string, T>();
    }

    public void LoadData<T>(string tableName) where T : GameDataBase
    {
        string dataName = tableName + "Data";
        if (_dataList.ContainsKey(tableName) == false)
        {
            _dataList.Add(dataName, new Dictionary<string, T>());
        }
        _dataList[dataName] = LoadJsonData<T>(tableName);
    }


    public T GetData<T>(string id) where T : GameDataBase
    {
        string type = typeof(T).Name;
        object dictObj = null;

        if (_dataList.TryGetValue(type, out dictObj))
        {
            var dict = dictObj as Dictionary<string, T>;
            return dict[id];
        }
        return null;
    }
}