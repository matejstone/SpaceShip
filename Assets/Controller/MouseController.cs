using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MouseController : MonoBehaviour {

    public GameObject CursorPrefab;
    public GameObject RoomOverlayPrefab;

    List<GameObject> roomOverlayObjects;

    // Use this for initialization
    void Start () {
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
        Vector3 currFramePosition = GameController.Instance.CameraController.getCurFramePosition();
        // Set mouse position over the tile
        Tile tileUnderMouse = GameController.Instance.GetTileAtWorldCoord(currFramePosition);

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

            GameController.Instance.UIController.updateMainText(roomName);
            GameController.Instance.UIController.updateXYCoord(tileUnderMouse.X, tileUnderMouse.Y);
        }
        else {
            GameController.Instance.UIController.updateMainText(roomName);
            GameController.Instance.UIController.updateXYCoord(-1,-1);
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
