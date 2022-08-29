using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorInput : MonoBehaviour
{
    public Tilemap3D _map;
    public GameObject _camHolder;
    private Camera _cam;
    private int _selectedTileID = 0;
    private int _maxTileID;

    private void Awake() {
        _cam = _camHolder.GetComponentInChildren<Camera>();
        _maxTileID = _map._gameObjectList.Count;
    }

    private void Update() {
        PlaceTile();
        PlaceTileOnTop();
        DeleteTile();
        RotateCam();
        MoveCamera();
        SelectTile();
        SaveMap();
    }

    private void PlaceTile(){
        if(Input.GetMouseButtonDown(0)){
            RaycastHit hit;
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit)) {
                Transform objectHit = hit.transform;
                _map.PlaceTileOnPosition(objectHit.position, _selectedTileID);
            }
        }
    }

    private void PlaceTileOnTop(){
        if(Input.GetMouseButtonDown(1)){
            RaycastHit hit;
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit)) {
                Transform objectHit = hit.transform;
                _map.PlaceTileOnPosition(objectHit.position + new Vector3(0,_map.GetBaseMap.cellSize.z,0), _selectedTileID);
            }
        }
    }

    private void DeleteTile(){
        if(Input.GetMouseButtonDown(2)){
            RaycastHit hit;
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit)) {
                Transform objectHit = hit.transform;
                if(objectHit.position.y > 0){
                    _map.PlaceTileOnPosition(objectHit.position, -1);
                }
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

    private void SelectTile(){
        if(Input.GetKeyDown(KeyCode.Period)){
            _selectedTileID = (int)Mathf.Repeat(_selectedTileID + 1, _maxTileID);
        }
        if(Input.GetKeyDown(KeyCode.Comma)){
            _selectedTileID = (int)Mathf.Repeat(_selectedTileID - 1, _maxTileID);
        }
    }

    private void SaveMap(){
        if(Input.GetKeyDown(KeyCode.Tab)){
            _map.SaveTileMap();
        }
    }
}
