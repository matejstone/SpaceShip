using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Tile {
    Ship ship;
    public int X { get; protected set; }
    public int Y { get; protected set; }
    public bool walkable { get; protected set; }

    public Tile(Ship ship, int x, int y)
    {
        this.ship = ship;
        this.X = x;
        this.Y = y;
        setWalkable(false);
    }

    public enum TileType { Void, Floor, Wall, Error };

    Action<Tile> cbTileTypeChanged;

    private TileType _type = TileType.Void;

    public Room room { get; protected set; }

    PlacedObject placedObject;

    public void setRoom(Room room) {
        this.room = room;
    }

    public void setWalkable(bool walkable) {
        this.walkable = walkable;
    }

    public TileType Type
    {
        get
        {
            return _type;
        }
        set
        {
            TileType oldType = _type;
            _type = value;

            // Callback
            if (cbTileTypeChanged != null && oldType != _type)
                cbTileTypeChanged(this);
        }
    }

    // multiTilePlacement = true means it will not check the other tiles for placement of the object
    public bool InstallObject(PlacedObject objInstance, bool multiTilePlacement = false)
    {
        if (objInstance == null)
        {
            // we are uninstalling whatever was here before
            placedObject = null;
            return true;
        }

        if (placedObject != null)
        {
            Debug.LogError("Trying to install an object to a tile that already has one");
            return false;
        }

        if ((objInstance.width > 1 || objInstance.height > 1) && multiTilePlacement == false) {
            // Find all the tiles it needs to also affect
            
            int width = 0;
            int height = 0;

            if (objInstance.rotation == PlacedObject.Rotation.E || objInstance.rotation == PlacedObject.Rotation.W)
            {
                width = objInstance.height;
                height = objInstance.width;
            }
            else {
                width = objInstance.width;
                height = objInstance.height;
            }

            List<Tile> affectedTiles = new List<Tile>();

            // we need to start at this tile and go right first
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Tile tile = ship.GetTileAt(X + i, Y + j);
                    if (tile != this) {
                        affectedTiles.Add(tile);
                        tile.InstallObject(objInstance, true);
                    }
                }
            }
        }

        // we need to assign the object to this tile as the main tile
        placedObject = objInstance;

        // for objects that are bigger than 1x1 we need to also assign a placedobject to them
        // we determine what tiles it also occupies based on the width, height and the direction it is facing

        // At this point everything's fine!
        return true;
    }

    public void RegisterTileTypeChangedCallback(Action<Tile> callback)
    {
        cbTileTypeChanged += callback;
    }

    public void UnregisterTileTypeChangedCallback(Action<Tile> callback)
    {
        cbTileTypeChanged -= callback;
    }

    public bool hasItem() {
        return (placedObject != null ? true : false);
    }

    public PlacedObject getPlacedItem() {
        return placedObject;
    }
}
