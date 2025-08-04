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
    SaveData saveData;
    SaveData loadData;
    void Update()
    {
        if (Input.GetKeyDown("s"))
        {
            saveData = new SaveData(false,false,false,false);

            SaveSystem.Save(saveData);
        }

        if (Input.GetKeyDown("l"))
        {
            loadData = SaveSystem.Load();
            Debug.Log(string.Format(
                "LoadData Result => IsTraffic : {0}, IsCargate : {1}, IsDeer : {2}, IsChase : {3}",
                loadData._IsTrafficClear, loadData._IsCargateClear, loadData._IsDeerClear, loadData._IsChaseEvent));
        }
    }
}
