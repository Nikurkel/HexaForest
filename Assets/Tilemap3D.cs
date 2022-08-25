using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Tilemap3D : MonoBehaviour
{
    class Tile3D{
        public GameObject model;
        public Vector3Int gridIndex;
        public int objectID;
        public Tile3D(GameObject obj, Vector3Int index, int id){
            this.gridIndex = index;
            this.objectID = id;
            this.model = obj;
        }

        public void DestroyObject(){
            Destroy(this.model);
        }
    }

    private Tilemap _map;
    private List<Tile3D> _tile3DList;
    public List<GameObject> _gameObjectList;

    private void Awake() {
        _tile3DList = new List<Tile3D>();
        _map = GetComponent<Tilemap>();
    }


    public void PlaceTileOnIndex(Vector3Int index, int id){
        // Add or replace Tile3D on map
        bool existsOnMap = false;
        for (int i = 0; i < _tile3DList.Count; i++){
            if(_tile3DList[i].gridIndex == index){
                if(_tile3DList[i].objectID != id){
                    _tile3DList[i].DestroyObject();

                    GameObject obj = Instantiate(_gameObjectList[id], _map.CellToLocal(index), new Quaternion());
                    Tile3D t = new Tile3D(obj, index, id);
                    _tile3DList[i] = t;
                }
                existsOnMap = true;
                break;
            }
        }
        if(!existsOnMap){
            GameObject obj = Instantiate(_gameObjectList[id], _map.CellToLocal(index), new Quaternion());
            Tile3D t = new Tile3D(obj, index, id);
            _tile3DList.Add(t);
        }
    }

    public void PlaceTileOnPosition(Vector3 pos, int id){
        // call PlaceTileOnIndex using cell index
        Vector3Int index = _map.LocalToCell(pos);
        PlaceTileOnIndex(index, id);
    }

    // call when cell scale / offsets have changed
    void UpdateTilePositions(){
        foreach (Tile3D tile in _tile3DList) {
            tile.model.transform.position = _map.CellToLocal(tile.gridIndex);
        }
    }
}