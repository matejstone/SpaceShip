using UnityEngine;
using System.Collections;
using System;

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

    public bool InstallObject(PlacedObject objInstance)
    {
        if (objInstance == null)
        {
            // we are uninstalling whatever was here before
            placedObject = null;
            return true;
        }

        if (placedObject != null)
        {
            //Debug.LogError("Trying to install an object to a tile that already has one");
            return false;
        }

        placedObject = objInstance;
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


}
