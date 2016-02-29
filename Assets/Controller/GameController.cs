using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
    /*
    public MouseController MouseController;
    public ShipController ShipController;
    public CameraController CameraController;
    public UIController UIController;
    public UniverseController UniverseController;
    */
    public static GameController Instance { get; protected set; }

    public bool debugMode;
    public bool isInitDone { get; protected set; }

    // Use this for initialization
    void Start () {
        if (Instance != null)
        {
            Debug.LogError("There are two game controllers present!");
        }
        Instance = this;
        isInitDone = false;



    }
	
	// Update is called once per frame
	void Update () {

        if (isInitDone == false) {
            isInitDone = true;

            UniverseController.Instance.createUniverse();
            ShipController.Instance.initShip();
            CameraController.Instance.moveCameraTo(ShipController.Instance.shipWidth / 2, ShipController.Instance.shipHeight / 2, 50f);
        }
	}

    public Tile GetTileAtWorldCoord(Vector3 coord)
    {
        int x = Mathf.FloorToInt(coord.x);
        int y = Mathf.FloorToInt(coord.y);

        return ShipController.Instance.Ship.GetTileAt(x, y);
    }
}
