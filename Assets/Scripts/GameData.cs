using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int fileID;

    public int[,,] tilemap;

    public GameData(LoadedGameData d)
    {
        fileID = d.fileID;
        tilemap = d.tilemap;
    }

    public GameData(int id){
        fileID = id;
        tilemap = new int[50,50,5];

        for (int x = 0; x < tilemap.GetLength(0); x++){
            for (int y = 0; y < tilemap.GetLength(1); y++){
                for (int z = 0; z < tilemap.GetLength(2); z++){
                    tilemap[x,y,z] = 0;
                }
            }
        }
    }
}