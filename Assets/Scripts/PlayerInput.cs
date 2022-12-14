using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    enum TileIds {desert, water, mountain}
    [SerializeField]
    private Tilemap3D _map;
    [SerializeField]
    private GameObject _camHolder;
    private Camera _cam;
    private TurnManager _turnManager;
    [SerializeField]
    private GameObject _highlightTile;
    private GameObject _highlightObject;

    private void Awake() {
        _cam = _camHolder.GetComponentInChildren<Camera>();
        _turnManager = GetComponent<TurnManager>();
    }

    private void OnEnable() {
        _highlightObject = Instantiate(_highlightTile, new Vector3(0,0,0), new Quaternion());
    }

    private void OnDisable(){
        Destroy(_highlightObject);
    }

    private void Update() {
        PlaceTile();
        Highlight();
        RotateCam();
        EndTurn();
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

    private void Highlight(){
        RaycastHit hit;
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hit)) {
            Transform objectHit = hit.transform;
            _highlightObject.transform.position = _map.GetCellPosition(objectHit.position);
        }
    }

    private void EndTurn(){
        if(Input.GetKeyDown(KeyCode.Return)){
            _turnManager.UpdateMap();
        }
    }
}
