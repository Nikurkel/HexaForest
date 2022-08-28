using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem {
    private static string path = Application.persistentDataPath;

    public static void SaveData(LoadedGameData d) {
        string extPath = path + "/data_" + d.fileID + ".s";

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(extPath, FileMode.Create);

        GameData data = new GameData(d);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void CreateData(int file) {
        GameData d = new GameData(file);
        d.fileID = file;
        string extPath = path + "/data_" + file + ".s";

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(extPath, FileMode.Create);

        formatter.Serialize(stream, d);
        stream.Close();
    }

    public static GameData LoadData(int file) {
        // C:/Users/nikla/AppData/LocalLow/DefaultCompany/HexaForest
        string extPath = path + "/data_" + file + ".s";

        if (!File.Exists(extPath)) {
            Debug.LogError("Save file not found in " + extPath);
            Debug.LogError("creating new File at " + extPath);

            CreateData(file);
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(extPath, FileMode.Open);

        GameData data = formatter.Deserialize(stream) as GameData;
        stream.Close();

        return data;
    }
}