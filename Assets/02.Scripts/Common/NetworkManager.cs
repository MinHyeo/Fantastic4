using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    // 파일 저장 경로 설정 (C:/Users/이름/.../projectName/save.json)
    private string GetPath()
    {
        return Path.Combine(Application.persistentDataPath, "DaniTechSaveData.json");
    } 
}
