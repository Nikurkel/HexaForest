using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    enum TileIds {desert, water, mountain}
    public Tilemap3D _map;
    public GameObject _camHolder;
    private Camera _cam;

    private void Awake() {
        _cam = _camHolder.GetComponentInChildren<Camera>();
    }

    private void Start() {
        //TempMapGeneration();
    }

    private void TempMapGeneration(){
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
        PlaceTile();
        RotateCam();
        MoveCamera();
        SaveMap();
    }

    private void PlaceTile(){
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

    private void RotateCam(){
        if(Input.GetKey(KeyCode.A)){
            _camHolder.transform.Rotate(new Vector3(0,45 * Time.deltaTime,0));
        }
        if(Input.GetKey(KeyCode.D)){
            _camHolder.transform.Rotate(new Vector3(0,-45 * Time.deltaTime,0));
        }
    }

    private void MoveCamera(){
        // debug only -> define in actual game before any level
        GameObject g = _cam.gameObject;
        if(Input.GetKey(KeyCode.W)){
            g.transform.position += g.transform.forward * 10 * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.S)){
            g.transform.position -= g.transform.forward * 10 * Time.deltaTime;
        }

        if(Input.GetKey(KeyCode.Space)){
            g.transform.position += g.transform.up * 10 * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.LeftShift)){
            g.transform.position -= g.transform.up * 10 * Time.deltaTime;
        }
    }

    private void SaveMap(){
        if(Input.GetKeyDown(KeyCode.Tab)){
            _map.SaveTileMap();
        }
    }
}
