using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorInput : MonoBehaviour
{
    [SerializeField]
    private Tilemap3D _map;
    [SerializeField]
    private GameObject _camHolder;
    private Camera _cam;
    private int _selectedTileID = 0;
    private int _maxTileID;

    [SerializeField]
    private bool _freeCam = false;
    [SerializeField]
    private float _freeCamMovementSpeed;
    [SerializeField]
    private float _freecamSensitivity = 3f;
    [SerializeField]
    private float _freeCamMaxYAngle = 80f;

    Vector3 _freeCamInput;

    private Vector2 _freeCamCurrentRotation;
    private bool _freeCamLockRotation;

    private Rigidbody _freeCamRB;
    private TurnManager _turnManager;

    private void Awake() {
        _cam = _camHolder.GetComponentInChildren<Camera>();
        _maxTileID = _map.GetTileStructsSize;
        _freeCamRB = _cam.gameObject.GetComponent<Rigidbody>();
        _turnManager = GetComponent<TurnManager>();
    }

    private void Update() {
        PlaceTile();
        PlaceTileOnTop();
        DeleteTile();
        if(_freeCam){
            FreeCamUpdate();
        }else{
            RotateCam();
            MoveCamera();
        }
        SelectTile();
        SaveMap();
        EndTurn();
    }

    private void FixedUpdate() {
        if(_freeCam){
            FreeCamFixedUpdate();
        }
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

    private void FreeCamUpdate(){

        // movement input
        _freeCamInput = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.A))  _freeCamInput.x += -1;
        if (Input.GetKey(KeyCode.D)) _freeCamInput.x += 1;
        if (Input.GetKey(KeyCode.LeftShift))  _freeCamInput.y += -1;
        if (Input.GetKey(KeyCode.Space)) _freeCamInput.y += 1;
        if (Input.GetKey(KeyCode.W)) _freeCamInput.z += 1;
        if (Input.GetKey(KeyCode.S)) _freeCamInput.z += -1;
        
        // exit
        if (Input.GetKey(KeyCode.Escape)) Application.Quit();

        // camera rotation input
        if (!_freeCamLockRotation) {
            _freeCamCurrentRotation.x += Input.GetAxis("Mouse X") * _freecamSensitivity * (_cam.fieldOfView / 90);
            _freeCamCurrentRotation.y -= Input.GetAxis("Mouse Y") * _freecamSensitivity * (_cam.fieldOfView / 90);
            _freeCamCurrentRotation.x = Mathf.Repeat(_freeCamCurrentRotation.x, 360);
            _freeCamCurrentRotation.y = Mathf.Clamp(_freeCamCurrentRotation.y, -_freeCamMaxYAngle, _freeCamMaxYAngle);
            _cam.transform.rotation = Quaternion.Euler(_freeCamCurrentRotation.y, _freeCamCurrentRotation.x, 0);
        }

        // speed adjustment
        //_freeCamMovementSpeed = Mathf.Clamp(_freeCamMovementSpeed + Input.mouseScrollDelta.y, 0, 30);

        // lock/release mouse cursor in Game
        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            if (Cursor.lockState == CursorLockMode.Locked) {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                _freeCamLockRotation = true;
            }
            else {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                _freeCamLockRotation = false;
            }
        }
    }

    private void FreeCamFixedUpdate(){
        // get forward and right direction of camera
        Vector3 forward = _cam.transform.forward;
        Vector3 right = _cam.transform.right;
        Vector3 upward = new Vector3(0,1,0);

        // don't change y direction to move
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // get direction and speed to move:
        Vector3 desiredMoveDirection = forward * _freeCamInput.z + right * _freeCamInput.x + upward * _freeCamInput.y;
        desiredMoveDirection.Normalize();

        _freeCamRB.velocity = desiredMoveDirection * _freeCamMovementSpeed;
    }

    private void SelectTile(){
        if(Input.GetKeyDown(KeyCode.Period)){
            _selectedTileID = (int)Mathf.Repeat(_selectedTileID + 1, _maxTileID);
        }
        if(Input.GetKeyDown(KeyCode.Comma)){
            _selectedTileID = (int)Mathf.Repeat(_selectedTileID - 1, _maxTileID);
        }

        _selectedTileID = (int)Mathf.Repeat(_selectedTileID + Input.mouseScrollDelta.y, _maxTileID);
        
    }

    private void SaveMap(){
        if(Input.GetKeyDown(KeyCode.Tab)){
            _map.SaveTileMap();
        }
    }

    private void EndTurn(){
        if(Input.GetKeyDown(KeyCode.Return)){
            _turnManager.UpdateMap();
        }
    }
}
