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

    private void Update() {
        PlaceTile();
        RotateCam();
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
}
