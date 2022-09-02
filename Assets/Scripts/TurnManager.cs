using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {
    
    [SerializeField]
    private Tilemap3D _map;
    private Vector3Int _mapSize;
    [SerializeField] [Range(0,100)]
    private int _cityGenProbability;
    private BoundsInt _bounds;
    // for passing methods to other methods
    private delegate void RuleMethod(Vector3Int pos);

    private void Start() {
        _mapSize = _map.GetMapSize;
        Vector3Int t = _map.OffsetArrayToGrid(new Vector3Int(0,0,0));
        _bounds = new BoundsInt(t.x,t.y,t.z,_mapSize.x, _mapSize.y, _mapSize.z);
    }

    public void UpdateMap(){
        // update order
        IterateMap(GenerateCityTiles);
        IterateMap(GenerateForestTiles);
        IterateMap(GenerateSavannahTiles);

        // Rivers?
    }

    private void GenerateForestTiles(Vector3Int gridIndex){
        // only replace desert, savannah and grass
        if(_map.GetTileAt(gridIndex) == 5){
            foreach(Vector3Int neighbourIndex in GetNeighbourIndices(gridIndex)){
                // check if tile is on top
                if(isTileOnTop(neighbourIndex)){
                    if(_map.GetTileAt(neighbourIndex) == 0 || _map.GetTileAt(neighbourIndex) == 1 || _map.GetTileAt(neighbourIndex) == 6){
                        _map.PlaceTileOnIndex(neighbourIndex, 2);
                    }
                }
            }
        }
    }

    private void GenerateSavannahTiles(Vector3Int gridIndex){
        // is tile forest?
        if(_map.GetTileAt(gridIndex) == 2){
            foreach(Vector3Int neighbourIndex in GetNeighbourIndices(gridIndex)){
                if(isTileOnTop(neighbourIndex)){
                    // is neighbour desert?
                    if(_map.GetTileAt(neighbourIndex) == 0){
                        _map.PlaceTileOnIndex(neighbourIndex, 1);
                    }
                }
            }
        }
    }

    private void GenerateCityTiles(Vector3Int gridIndex){
        // is tile forest?
        if(_map.GetTileAt(gridIndex) == 2){
            // apply probability of generation
            if(Random.Range(0,100) < _cityGenProbability){
                int countForests = 1;
                foreach(Vector3Int neighbourIndex in GetNeighbourIndices(gridIndex)){
                    if(isTileOnTop(neighbourIndex)){
                        if(_map.GetTileAt(neighbourIndex) == 2){
                            countForests++;
                        }
                    }
                }
                if(countForests > 3){
                    _map.PlaceTileOnIndex(gridIndex, 7);
                }
            }
        }
    }

    private void IterateMap(RuleMethod ruleMethod){
        for (int x = 0; x < _mapSize.x; x++) {
            for (int y = 0; y < _mapSize.y; y++) {
                for (int z = 0; z < _mapSize.z; z++) {
                    Vector3Int gridIndex = _map.OffsetArrayToGrid(new Vector3Int(x,y,z));
                    if(isTileOnTop(gridIndex)){
                        ruleMethod(gridIndex);
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

    private bool isTileOnTop(Vector3Int pos){
        for(int i = _mapSize.z-1; i > pos.z;i--){
            if(_map.GetTileAt(new Vector3Int(pos.x,pos.y,i)) >= 0){
                return false;
            }
        }
        return true;
    }

    // TODO: make TileStructList have a fixed order

}