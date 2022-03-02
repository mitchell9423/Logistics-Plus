using System;
using System.IO;
using UnityEngine;

public class JSONManager : MonoBehaviour
{
    public static JSONManager Instance { get; private set; }
    private string path = "";

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void SetPath()
    {
#if UNITY_EDITOR
        path = Application.dataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
#else
        path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
#endif
    }

    public void SaveData(object obj)
    {
        SetPath();
        string json = JsonUtility.ToJson(obj);
        try
        {
            // Create an instance of StreamWriter to write to a file.
            // The using statement also closes the StreamWriter.
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.Write(json);
            }
        }
        catch (Exception e)
        {
            // Let the user know what went wrong.
            Debug.Log("The file could not be read:");
            Debug.Log(e.Message);
        }
    }

    public T LoadData<T>()
    {
        SetPath();
        try
        {
            // Create an instance of StreamReader to read from a file.
            // The using statement also closes the StreamReader.
            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                return JsonUtility.FromJson<T>(json);
            }
        }
        catch (Exception e)
        {
            SaveData(new GameData());
            using (StreamReader reader = new StreamReader(path))
            {
                // Let the user know what went wrong.
                Debug.Log("The file could not be read:");
                Debug.Log(e.Message);
                string json = reader.ReadToEnd();
                return JsonUtility.FromJson<T>(json);
            }
        }
	}
}
