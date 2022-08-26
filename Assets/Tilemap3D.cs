using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Tilemap3D : MonoBehaviour
{
    class Tile3D{
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

        public void DestroyObject(){
            Destroy(this.model);
        }
    }

    private Tilemap _map;
    private Tile3D[,,] _tile3DArray = new Tile3D[50,50,5];
    private Vector3Int _offset = new Vector3Int(-25,-25,0);
    private BoundsInt _bounds;
    public List<GameObject> _gameObjectList;
    private GameObject _container;

    private void Awake() {
        _map = GetComponent<Tilemap>();
        _bounds = new BoundsInt(_offset.x,_offset.y,_offset.z,_tile3DArray.GetLength(0),_tile3DArray.GetLength(1),_tile3DArray.GetLength(2));
        _container = new GameObject("Tile Container");
        
        ResetMap();
    }

    // Add or replace Tile3D on map
    public void PlaceTileOnIndex(Vector3Int gridPosition, int id){
        if(!_bounds.Contains(gridPosition)){
            // print($"position {gridPosition} out of bounds {_bounds}");
            return;
        }

        Vector3Int index = OffsetGridToArray(gridPosition);
        // print(index);

        if(id == _tile3DArray[index.x, index.y, index.z].objectID){
            // print("unable to place same tile");
            return;
        }

        // Destroy old model
        if(_tile3DArray[index.x, index.y, index.z].model != null){
            _tile3DArray[index.x, index.y, index.z].DestroyObject();
        }

        GameObject obj = Instantiate(_gameObjectList[id], _map.CellToLocal(gridPosition), new Quaternion(), _container.transform);
        _tile3DArray[index.x, index.y, index.z] = new Tile3D(obj, id);
    }

    // call PlaceTileOnIndex using cell index
    public void PlaceTileOnPosition(Vector3 pos, int id){
        Vector3Int index = _map.LocalToCell(pos);
        PlaceTileOnIndex(index, id);
    }

    private Vector3Int OffsetArrayToGrid(Vector3Int arrayIndex){
        return arrayIndex + _offset;
    }

    private Vector3Int OffsetGridToArray(Vector3Int gridIndex){
        return gridIndex - _offset;
    }

    // call when cell scale / offsets have changed
    private void UpdateTilePositions(){
        for (int x = 0; x < _tile3DArray.GetLength(0); x++){
            for (int y = 0; y < _tile3DArray.GetLength(1); y++){
                for (int z = 0; z < _tile3DArray.GetLength(2); z++){
                    _tile3DArray[x,y,z].model.transform.position = _map.CellToLocal(OffsetArrayToGrid(new Vector3Int(x,y,z)));
                }
            }
        }
    }

    public int GetTileAt(Vector3Int gridPosition){
        Vector3Int i = OffsetGridToArray(gridPosition);
        return _tile3DArray[i.x, i.y, i.z].objectID;
    }

    private void ResetMap(){
        for (int x = 0; x < _tile3DArray.GetLength(0); x++){
            for (int y = 0; y < _tile3DArray.GetLength(1); y++){
                for (int z = 0; z < _tile3DArray.GetLength(2); z++){
                    if(_tile3DArray[x, y, z] != null){
                        if(_tile3DArray[x, y, z].model != null){
                            _tile3DArray[x, y, z].DestroyObject();
                        }
                    }

                    _tile3DArray[x,y,z] = new Tile3D();
                }
            }
        }
    }
}