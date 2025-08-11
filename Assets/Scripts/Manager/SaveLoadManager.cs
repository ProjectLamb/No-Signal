//using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public SaveData(bool IsTrafficClear, bool IsCargateClear, bool IsDeerClear, bool IsChaseEvent)
    {
        IsTrafficClear = _IsTrafficClear;
        IsCargateClear = _IsCargateClear;
        IsDeerClear = _IsDeerClear;
        IsChaseEvent = _IsChaseEvent;
    }

    public bool _IsTrafficClear;
    public bool _IsCargateClear;
    public bool _IsDeerClear;
    public bool _IsChaseEvent;

}

public static class SaveSystem
{
    private static string SavePath => Application.persistentDataPath + "/saves/";

    public static void Save(SaveData saveData)
    {
        if (!Directory.Exists(SavePath))
        {
            Directory.CreateDirectory(SavePath);
        }

        string saveJson = JsonUtility.ToJson(saveData);

        string saveFilePath;

#if UNITY_EDITOR   //만약 유니티 에디터라면
        saveFilePath = Path.Combine(Application.dataPath, "UserData.json");
        
#elif UNITY_STANDALONE_WIN //운영체제가 Windows
        saveFilePath = Path.Combine(Application.dataPath, "UserData.json");
        
#elif UNITY_STANDALONE_OSX //운영체제가 Mac
        saveFilePath = Path.Combine(Application.persistentDataPath, "UserData.json");
#endif

        File.WriteAllText(saveFilePath, saveJson);
        Debug.Log("Save Success: " + saveFilePath);
    }

    public static SaveData Load()
    {
        string saveFilePath;

#if UNITY_EDITOR   //만약 유니티 에디터라면
        saveFilePath = Path.Combine(Application.dataPath, "UserData.json");
        
#elif UNITY_STANDALONE_WIN //운영체제가 Windows
        saveFilePath = Path.Combine(Application.dataPath, "UserData.json");
        
#elif UNITY_STANDALONE_OSX //운영체제가 Mac
        saveFilePath = Path.Combine(Application.persistentDataPath, "UserData.json");
#endif

        if (!File.Exists(saveFilePath))
        {
            Debug.LogError("No such saveFile exists");
            return null;
        }

        string saveFile = File.ReadAllText(saveFilePath);
        SaveData saveData = JsonUtility.FromJson<SaveData>(saveFile);
        return saveData;
    }
}
public class SaveLoadManager : MonoBehaviour
{
    private static SaveLoadManager instance;
    public static SaveLoadManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("No SaveLoadManager Instance");
            }
            return instance;
        }
    }

    public bool IsTrafficClear = false;
    public bool IsDeerClear = false;
    public bool IsCargateClear = false;
    public bool IsChaseEvent = false;

    SaveData saveData;
    SaveData loadData;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        loadData = SaveSystem.Load();

        IsTrafficClear = loadData._IsTrafficClear;
        IsCargateClear = loadData._IsCargateClear;
        IsDeerClear = loadData._IsDeerClear;
        IsChaseEvent = loadData._IsChaseEvent;
    }
    public void SaveGameData()
    {
        saveData = new SaveData(IsTrafficClear, IsDeerClear, IsCargateClear, IsChaseEvent);
        SaveSystem.Save(saveData);
    }
}
