using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {
    
    [SerializeField]
    private Tilemap3D _map;
    private Vector3Int _mapSize;

    private void Start() {
        _mapSize = _map.GetMapSize;
    }

    public void UpdateMap(){
        // update order
        GenerateForestTiles();
        // Savannah
        // Cities

        // Rivers?
    }

    private void GenerateForestTiles(){
        for (int x = 0; x < _mapSize.x; x++) {
            for (int y = 0; y < _mapSize.y; y++) {
                for (int z = 0; z < _mapSize.z; z++) {
                    Vector3Int gridIndex = _map.OffsetArrayToGrid(new Vector3Int(x,y,z));
                    Vector3Int t = _map.OffsetArrayToGrid(new Vector3Int(0,0,0));
                    BoundsInt bounds = new BoundsInt(t.x,t.y,t.z,_mapSize.x, _mapSize.y, _mapSize.z);
                    if(_map.GetTileAt(gridIndex) == 0 || _map.GetTileAt(gridIndex) == 1 || _map.GetTileAt(gridIndex) == 6){
                        
                        foreach(Vector3Int neighbourIndex in GetNeighbourIndices(gridIndex)){
                            if(bounds.Contains(neighbourIndex)){
                                if(_map.GetTileAt(neighbourIndex) == 5){
                                    _map.PlaceTileOnIndex(gridIndex, 2);
                                    break;
                                }
                            }
                        }
                    }
                }
            }   
        }
    }

    private List<Vector3Int> GetNeighbourIndices(Vector3Int pos){
        List<Vector3Int> neighbours = new List<Vector3Int>();
        // fix hex grid indexing
        int xoffset = 0;
        if(Mathf.Abs(pos.y)%2 == 1) xoffset = 1;

        neighbours.Add(new Vector3Int(pos.x-1 + xoffset, pos.y-1, pos.z));
        neighbours.Add(new Vector3Int(pos.x + xoffset, pos.y-1, pos.z));
        neighbours.Add(new Vector3Int(pos.x-1, pos.y, pos.z));
        neighbours.Add(new Vector3Int(pos.x+1, pos.y, pos.z));
        neighbours.Add(new Vector3Int(pos.x-1 + xoffset, pos.y+1, pos.z));
        neighbours.Add(new Vector3Int(pos.x + xoffset, pos.y+1, pos.z));

        return neighbours;
    }

    // TODO: make TileStructList have a fixed order

}
