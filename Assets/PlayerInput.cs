using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    enum TileIds {desert, water, mountain}
    public Tilemap3D _map;
    private Camera _cam;

    private void Awake() {
        _cam = Camera.main;
    }

    // map generation prototype
    private void Start() {
        for (int i = -25; i < 25; i++){
            for (int j = -25; j < 25; j++){
                for (int k = 0; k <= 1; k++){
                    if(Random.Range(0,k+1) == 0){
                        _map.PlaceTileOnIndex(new Vector3Int(i,j,k), ((int)TileIds.desert));
                    }
                }
            }
        }
    }

    // tile placement prototype
    private void Update() {
        if(Input.GetMouseButtonDown(0)){
            RaycastHit hit;
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit)) {
                Transform objectHit = hit.transform;
                // print(objectHit.position);
                _map.PlaceTileOnPosition(objectHit.position, ((int)TileIds.water));
            }
        }
    }
}
