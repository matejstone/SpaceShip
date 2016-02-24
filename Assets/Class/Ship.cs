using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class Ship {

    public string[,] tileMap;
    public Tile[,] tiles;

    Dictionary<string, PlacedObject> placedObjectPrototypes;
    ItemContainer itemContainer;

    public int Width { get; protected set; }
    public int Height { get; protected set; }
    public ItemContainer ItemContainer { get; private set; }

    Action<PlacedObject> cbPlacedObjectCreated;

    public Ship(int width = 10, int height = 10)
    {
        Width = width;
        Height = height;

        tiles = new Tile[width, height];
        tileMap = new string[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tiles[x, y] = new Tile(this, x, y);
            }
        }

        Debug.Log("Ship created! Height: " + height + " Width: " + width);

        CreatePlacedObjectPrototypes();
        Debug.Log("Prototypes created!");

    }

    public Tile GetTileAt(int x, int y)
    {
        if (x > Width - 1 || x < 0 || y > Height - 1 || y < 0)
        {
            if (GameController.Instance.debugMode == true)
            {
                Debug.LogError("Tile (" + x + "," + y + ")is out of range");
            }

            return null;
        }
        return tiles[x, y];
    }

    public void RandomizeTiles()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                int randomNum = UnityEngine.Random.Range(0, 2);
                //Debug.Log("RandomNum is " + randomNum);
                if (randomNum == 0)
                {
                    tiles[x, y].Type = Tile.TileType.Floor;
                    //tiles[x, y].Editable = true;
                }
                else {
                    tiles[x, y].Type = Tile.TileType.Void;
                    //tiles[x, y].Editable = true;
                }

            }
        }
    }

    public void buildShip()
    {
        // first build the rooms
        buildRooms();
    }

    private void buildRooms()
    {
        // get the middle of the ship
        int middleX = (int) (Width / 2);
        int middleY = (int)(Height / 2);

        // Start with the bridge
        int bridgeWidth = 4;
        int bridgeHeight = 4;

        int bridgeX = middleX;
        int bridgeY = middleY - (int)(bridgeHeight / 2);
        Room bridge = new Room(this, bridgeX, bridgeY, bridgeWidth, bridgeHeight, "Bridge");
        bridge.createRoom();

        // next we do a basic cargo bay
        int cargoBayWidth = 10;
        int cargoBayHeight = 8;

        int cargoBayX = bridgeX - cargoBayWidth;
        int cargoBayY = middleY - (int)(cargoBayHeight / 2);

        Room cargoBay = new Room(this, cargoBayX, cargoBayY, cargoBayWidth, cargoBayHeight, "Cargo Bay");
        cargoBay.createRoom();

        // lets also do the engine room
        int engiWidth = 5;
        int engiHeight = 6;

        int engiX = cargoBayX - engiWidth;
        int engiY = middleY - (int)(engiHeight / 2);

        Room engineering = new Room(this, engiX, engiY, engiWidth, engiHeight, "Engine Room");
        engineering.createRoom();

        // create passageways between rooms
        createDoor(bridgeX, middleY);
        createDoor(cargoBayX, middleY);
    }

    void createDoor(int x, int y)
    {
        // Creates a simple passageway
        tiles[x, y].Type = Tile.TileType.Floor;
    }

    public void CreatePlacedObjectPrototypes()
    {
        placedObjectPrototypes = new Dictionary<string, PlacedObject>();
        itemContainer = ItemContainer.Load(Path.Combine(Application.dataPath, "./xml/items.xml"));
        itemContainer.Items.ForEach((item) => {
                CreateItem(item.Name, item.Width, item.Height, item.Obstacle, item.SpriteId, item.SpriteName);
            });
    }

    public void CreateItem(string name, int width, int height, bool obstacle = false, int spriteId = 0, string spriteName = "new_item") {
        PlacedObject objectProto = PlacedObject.CreatePrototype(
            name,
            width,
            height,
            obstacle,
            spriteId,
            spriteName
        );

        placedObjectPrototypes.Add(name, objectProto);
    }

    public void placeObject(string objectType, Tile t, PlacedObject.Rotation rotation = PlacedObject.Rotation.W)
    {

        if (placedObjectPrototypes.ContainsKey(objectType) == false)
        {
            Debug.LogError("installedObjectPrototypes does not contain a prototype for key: " + objectType);
            return;
        }

        PlacedObject obj = PlacedObject.PlaceObject(placedObjectPrototypes[objectType], t, rotation);

        if (obj == null)
        {
            // Failed to place object -- most likely already something there.
            return;
        }

        if (cbPlacedObjectCreated != null)
        {
            cbPlacedObjectCreated(obj);
        }
    }

    

    public void RegisterPlacedObjectCreated(Action<PlacedObject> callback)
    {
        cbPlacedObjectCreated += callback;
    }

    public void UnregisterPlacedObjectCreated(Action<PlacedObject> callback)
    {
        cbPlacedObjectCreated -= callback;
    }



}
