using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Tile3D {
    public GameObject model;
    public int objectID;
    public Tile3D(GameObject obj, int id){
        this.objectID = id;
        this.model = obj;
    }

    public Tile3D(){
        this.objectID = -1;
        this.model = null;
    }
}