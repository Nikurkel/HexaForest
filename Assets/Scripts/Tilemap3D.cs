using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tilemap3D : MonoBehaviour {
    private Tilemap _baseMap;
    private Tile3D[,,] _tile3DArray = new Tile3D[50,50,5];
    public Vector3Int GetMapSize {get => new Vector3Int(_tile3DArray.GetLength(0), _tile3DArray.GetLength(1), _tile3DArray.GetLength(2));}
    private Vector3Int _offset = new Vector3Int(-25,-25,0);
    private BoundsInt _bounds;
    [SerializeField]
    private List<TileStruct> _tileStructs;
    public int GetTileStructsSize {get => _tileStructs.Count;}
    private GameObject _container;
    private LoadedGameData _loadedGameData;
    public Tilemap GetBaseMap {get => _baseMap;}

    private void Awake() {
        _baseMap = GetComponent<Tilemap>();
        _bounds = new BoundsInt(_offset.x,_offset.y,_offset.z,_tile3DArray.GetLength(0),_tile3DArray.GetLength(1),_tile3DArray.GetLength(2));
        _container = new GameObject("Tile Container");
        
        ResetMap();
    }

    private void Start() {
        _loadedGameData = GameObject.Find("File").GetComponent<LoadedGameData>();
        if(_loadedGameData.tilemap != null){
            LoadTileMap();
        }
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
            Destroy(_tile3DArray[index.x, index.y, index.z].model);
        }

        if(id < 0){
            // "place" empty tile (remove)
            _tile3DArray[index.x, index.y, index.z] = new Tile3D();
            return;
        }
        int randomYRotation = Random.Range(0,6) * 60;
        //print(randomYRotation);
        GameObject obj = Instantiate(_tileStructs[id].GetRandomTile(), _baseMap.CellToLocal(gridPosition), new Quaternion(), _container.transform);
        obj.transform.Rotate(new Vector3(0,randomYRotation,0), Space.World);
        _tile3DArray[index.x, index.y, index.z] = new Tile3D(obj, id);
    }

    // call PlaceTileOnIndex using cell index
    public void PlaceTileOnPosition(Vector3 pos, int id){
        Vector3Int index = _baseMap.LocalToCell(pos);
        PlaceTileOnIndex(index, id);
    }

    // make grid positions public
    public Vector3 GetCellPosition(Vector3 pos){
        return _baseMap.CellToLocal(_baseMap.LocalToCell(pos));
    }

    public Vector3Int OffsetArrayToGrid(Vector3Int arrayIndex){
        return arrayIndex + _offset;
    }

    public Vector3Int OffsetGridToArray(Vector3Int gridIndex){
        return gridIndex - _offset;
    }

    // call when cell scale / offsets have changed
    private void UpdateTilePositions(){
        for (int x = 0; x < _tile3DArray.GetLength(0); x++){
            for (int y = 0; y < _tile3DArray.GetLength(1); y++){
                for (int z = 0; z < _tile3DArray.GetLength(2); z++){
                    if(_tile3DArray[x,y,z].model != null){
                        _tile3DArray[x,y,z].model.transform.position = _baseMap.CellToLocal(OffsetArrayToGrid(new Vector3Int(x,y,z)));
                    }
                }
            }
        }
    }

    private void Update() {
        UpdateTilePositions();
    }

    public int GetTileAt(Vector3Int gridPosition){
        Vector3Int i = OffsetGridToArray(gridPosition);
        return _tile3DArray[i.x, i.y, i.z].objectID;
    }

    // called for initializing and resetting _tile3DArray
    private void ResetMap(){
        for (int x = 0; x < _tile3DArray.GetLength(0); x++){
            for (int y = 0; y < _tile3DArray.GetLength(1); y++){
                for (int z = 0; z < _tile3DArray.GetLength(2); z++){
                    if(_tile3DArray[x, y, z] != null){
                        if(_tile3DArray[x, y, z].model != null){
                            Destroy(_tile3DArray[x, y, z].model);
                        }
                    }

                    _tile3DArray[x,y,z] = new Tile3D();
                }
            }
        }
    }

    public void SaveTileMap(){
        int[,,] map = new int[_tile3DArray.GetLength(0),_tile3DArray.GetLength(1),_tile3DArray.GetLength(2)];
        for (int x = 0; x < map.GetLength(0); x++){
            for (int y = 0; y < map.GetLength(1); y++){
                for (int z = 0; z < map.GetLength(2); z++){
                    map[x,y,z] = _tile3DArray[x,y,z].objectID;
                }
            }
        }
        _loadedGameData.tilemap = map;
        _loadedGameData.SaveGame();
        print($"saved tilemap on File {_loadedGameData.fileID}");
    }

    private void LoadTileMap(){
        int[,,] map = _loadedGameData.tilemap;

        _tile3DArray = new Tile3D[map.GetLength(0),map.GetLength(1),map.GetLength(2)];
        ResetMap();
        _offset = new Vector3Int(-Mathf.RoundToInt(map.GetLength(0)/2),-Mathf.RoundToInt(map.GetLength(1)/2),0);
        _bounds = new BoundsInt(_offset.x,_offset.y,_offset.z,_tile3DArray.GetLength(0),_tile3DArray.GetLength(1),_tile3DArray.GetLength(2));

        for (int x = 0; x < map.GetLength(0); x++){
            for (int y = 0; y < map.GetLength(1); y++){
                for (int z = 0; z < map.GetLength(2); z++){
                    PlaceTileOnIndex(OffsetArrayToGrid(new Vector3Int(x,y,z)), map[x,y,z]);
                }
            }
        }
        print($"loaded tilemap from File {_loadedGameData.fileID}");
    }
}