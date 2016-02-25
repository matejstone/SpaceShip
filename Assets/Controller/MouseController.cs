using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MouseController : MonoBehaviour {

    public GameObject CursorPrefab;
    public GameObject RoomOverlayPrefab;

    List<GameObject> roomOverlayObjects;

    public static MouseController Instance { get; protected set; }

    public PlacedObject selectedObject;

    // Use this for initialization
    void Start () {
        if (Instance != null)
        {
            Debug.LogError("There are two Mouse controllers present!");
        }
        Instance = this;

        roomOverlayObjects = new List<GameObject>();
        SimplePool.Preload(RoomOverlayPrefab, 20);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCursor();
    }

    void UpdateCursor()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        Vector3 currFramePosition = CameraController.Instance.getCurFramePosition();
        // Set mouse position over the tile
        Tile tileUnderMouse = GameController.Instance.GetTileAtWorldCoord(currFramePosition);
        PlacedObject objectUnderMouse = null;

        string roomName = "Space";
        

        destroyRoomOverlay();
        hideCursor();

        if (tileUnderMouse != null)
        {
           
            if (tileUnderMouse.Type == Tile.TileType.Floor)
            {
                roomName = tileUnderMouse.room != null ? tileUnderMouse.room.name : "Floor";
                showRoomOverlay(tileUnderMouse.room);
            }
            else if (tileUnderMouse.Type == Tile.TileType.Wall) {
                roomName = "Wall";
            }

            showCursor(tileUnderMouse.X, tileUnderMouse.Y);

            UIController.Instance.updateMainText(roomName);
            UIController.Instance.updateXYCoord(tileUnderMouse.X, tileUnderMouse.Y);

            string objectText = "";

            if (tileUnderMouse.hasItem())
            {
                objectUnderMouse = tileUnderMouse.getPlacedItem();
                objectText = objectUnderMouse.getType();
            }

            UIController.Instance.updateObjectText(objectText);
        }
        else {
            UIController.Instance.updateMainText(roomName);
            UIController.Instance.updateXYCoord(-1,-1);
        }


        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            if (Input.GetMouseButton(0) && tileUnderMouse != null && tileUnderMouse.hasItem())
            {
                selectPlacedObject(objectUnderMouse);
            }
            else if (Input.GetMouseButton(1)) {
                deselectPlacedObject();
            }
                
        }


    }

    public enum RotDirection { Left, Right };

    public bool RotateSelectedObject(RotDirection direction) {
        Debug.Log("RotateSelectedObject");
        if (selectedObject != null)
        {
            Debug.Log("Not null!");
            if (direction == RotDirection.Left)
            {
                Debug.Log("Left!");
                if (selectedObject.rotation == PlacedObject.Rotation.E)
                {
                    selectedObject.rotation = PlacedObject.Rotation.N;
                }
                else if (selectedObject.rotation == PlacedObject.Rotation.W)
                {
                    selectedObject.rotation = PlacedObject.Rotation.S;
                }
                else if (selectedObject.rotation == PlacedObject.Rotation.N)
                {
                    selectedObject.rotation = PlacedObject.Rotation.W;
                }
                else if (selectedObject.rotation == PlacedObject.Rotation.S) {
                    selectedObject.rotation = PlacedObject.Rotation.E;
                }
            }
            else if (direction == RotDirection.Right)
            {
                Debug.Log("Right!");
                if (selectedObject.rotation == PlacedObject.Rotation.E)
                {
                    selectedObject.rotation = PlacedObject.Rotation.S;
                }
                else if (selectedObject.rotation == PlacedObject.Rotation.W)
                {
                    selectedObject.rotation = PlacedObject.Rotation.N;
                }
                else if (selectedObject.rotation == PlacedObject.Rotation.N)
                {
                    selectedObject.rotation = PlacedObject.Rotation.E;
                }
                else if (selectedObject.rotation == PlacedObject.Rotation.S)
                {
                    selectedObject.rotation = PlacedObject.Rotation.W;
                }
            }
        }
        else {
            Debug.LogError("No object selected");
        }
            return false;
    }

    public void deselectPlacedObject() {
        selectedObject = null;
        UIController.Instance.toggleSelectPanel(false);
    }

    public void selectPlacedObject(PlacedObject pObject) {
        if (selectedObject != pObject) {
            selectedObject = pObject;
            UIController.Instance.toggleSelectPanel(true, selectedObject);
        }
    }

    void showRoomOverlay(Room room) {
        room.roomTiles.ForEach(tile => {
            GameObject go = (GameObject)SimplePool.Spawn(RoomOverlayPrefab, new Vector3(tile.X, tile.Y, 0), Quaternion.identity);
            go.transform.parent = this.transform;
            roomOverlayObjects.Add(go);
        });
    }

    void destroyRoomOverlay() {
        // Clean up old room overlays
        while (roomOverlayObjects.Count > 0)
        {
            GameObject go = roomOverlayObjects[0];
            roomOverlayObjects.RemoveAt(0);
            SimplePool.Despawn(go);
        }
    }

    void hideCursor() {
        CursorPrefab.SetActive(false);
    }

    void showCursor(int x, int y) {
        CursorPrefab.SetActive(true);
        Vector3 cursorPosition = new Vector3(x, y, 0);
        CursorPrefab.transform.position = cursorPosition;
    }
}
