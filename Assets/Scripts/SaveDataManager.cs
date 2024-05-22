using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveDataManager {

    public static string SavePath = Application.persistentDataPath + "/Progress.Bin";

    public static void SaveData(SaveData saveData) {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(SavePath, FileMode.Create);

        formatter.Serialize(stream, saveData);
        stream.Close();

        Debug.Log("Player data saved successfully.");
    }
    public static SaveData LoadData() {
        if(File.Exists(SavePath)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(SavePath, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            Debug.Log("Player data loaded successfully.");
            return data;
        } else {
            Debug.Log("No data to load at " + SavePath);
            return null;
        }
    }
    public static void DeleteData() {
        if(File.Exists(SavePath)) {
            File.Delete(SavePath);
            Debug.Log("Deleted player save data successfully.");
        } else {
            Debug.Log("No data to delete at " + SavePath);
            return;
        }
    }

}
