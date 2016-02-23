using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    public MouseController MouseController;
    public ShipController ShipController;
    public CameraController CameraController;
    public UIController UIController;

    public static GameController Instance { get; protected set; }

    public bool debugMode;


    // Use this for initialization
    void Start () {
        if (Instance != null)
        {
            Debug.LogError("There are two game controllers present!");
        }
        Instance = this;


        ShipController.CreatePlayerShip();

        CameraController.moveCameraTo(ShipController.shipWidth / 2, ShipController.shipHeight / 2);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Tile GetTileAtWorldCoord(Vector3 coord)
    {
        int x = Mathf.FloorToInt(coord.x);
        int y = Mathf.FloorToInt(coord.y);

        return ShipController.Ship.GetTileAt(x, y);
    }
}
