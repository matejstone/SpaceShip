using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipController : MonoBehaviour {

    public static ShipController Instance { get; protected set; }

    public Dictionary<Tile, GameObject> tileGameObjectMap;
    Dictionary<PlacedObject, GameObject> placedObjectGameObjectMap;

    public Ship Ship { get; protected set; }
    public int shipWidth = 20;
    public int shipHeight = 20;

    public TextAsset itemListXML;

    public Sprite[] sprite;

    // Use this for initialization
    void Start () {
        if (Instance != null)
        {
            Debug.LogError("There are two ship controllers present!"); 
        }
        Instance = this;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void initDictionaries() {
        tileGameObjectMap = new Dictionary<Tile, GameObject>();
        placedObjectGameObjectMap = new Dictionary<PlacedObject, GameObject>();
    }

    public void CreatePlayerShip() {
        initDictionaries();

        Ship = new Ship(shipWidth, shipHeight);

        Ship.RegisterPlacedObjectCreated((placedObject) => { OnPlacedObjectCreated(placedObject); });

        tileGameObjectMap = new Dictionary<Tile, GameObject>();

        for (int x = 0; x < Ship.Width; x++)
        {
            for (int y = 0; y < Ship.Height; y++)
            {
                Tile tile_data = Ship.GetTileAt(x, y);
                GameObject tile_go = new GameObject();

                tile_go.name = "Tile_" + x + "_" + y;
                tile_go.transform.position = new Vector3(tile_data.X, tile_data.Y);
                tile_go.transform.SetParent(this.transform, true);
                tile_go.transform.localScale = new Vector3(1.0f, 1.0f);
                tile_go.AddComponent<SpriteRenderer>();

                tileGameObjectMap.Add(tile_data, tile_go);

                tile_data.RegisterTileTypeChangedCallback((tile) => { OnTileTypeChanged(tile, tile_go); });
            }
        }

        //Ship.RandomizeTiles();
        
        Ship.buildShip();
        Ship.placeObject("Console", Ship.GetTileAt(28, 25));
    }

    void OnTileTypeChanged(Tile tile_data, GameObject tile_go)
    {
        //Debug.Log("OnTileTypeChanged");

        if (tile_data.Type == Tile.TileType.Floor) {
            tile_go.GetComponent<SpriteRenderer>().sprite = sprite[0];
            tile_data.setWalkable(true);
        }

        if (tile_data.Type == Tile.TileType.Wall)
        {
            tile_go.GetComponent<SpriteRenderer>().sprite = sprite[1];
            tile_data.setWalkable(true);
        }
    }

    public void OnPlacedObjectCreated(PlacedObject obj)
    {

        // FIXME: does not consider multi-tile objects or rotation!
        // Create a visual GameObject linked to this data
        GameObject obj_go = new GameObject();

        obj_go.name = obj.objectType + "_" + obj.tile.X + "_" + obj.tile.Y;
        obj_go.transform.position = new Vector3(obj.tile.X, obj.tile.Y, 0);
        obj_go.transform.SetParent(this.transform, true);

        obj_go.AddComponent<SpriteRenderer>().sprite = sprite[obj.spriteId];
        obj_go.GetComponent<SpriteRenderer>().sortingLayerName = "PlacedItems";

        placedObjectGameObjectMap.Add(obj, obj_go);

        obj.RegisterOnChangedCallback(OnPlacedObjectChanged);

    }

    void OnPlacedObjectChanged(PlacedObject obj)
    {
        Debug.LogError("OnInstalledObjectChanged not implemented");
    }

    

}
