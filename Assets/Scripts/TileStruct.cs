using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileStruct{
    [SerializeField]
    private string name;
    [SerializeField]
    private bool randomTile;
    [SerializeField]
    private List<GameObject> gameObjects;

    public GameObject GetRandomTile(){
        if(randomTile){
            return gameObjects[Random.Range(0, gameObjects.Count)];
        }else{
            return gameObjects[0]; // TODO: specific logics
        }
    }
}