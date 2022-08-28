using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadedGameData : MonoBehaviour {
    public int fileID;

    public int[,,] tilemap;

    private void Awake() {
        if (GameObject.Find("File")) {
            Destroy(gameObject);
        }
        else {
            gameObject.name = "File";

            DontDestroyOnLoad(gameObject);
            LoadGame(fileID);
        }
    }

    public void SaveGame() {
        SaveSystem.SaveData(this);
    }

    public void LoadGame(int saveFile) {
        GameData gd = SaveSystem.LoadData(saveFile);

        fileID = gd.fileID;
        tilemap = gd.tilemap;
    }

    public void ResetFile() {
        tilemap = new int[50,50,5];
        SaveGame();
    }
}