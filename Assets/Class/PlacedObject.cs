using UnityEngine;
using System.Collections;
using System;

public class PlacedObject
{

    // BASE tile of the object
    public Tile tile { get; protected set; }
    public string objectType { get; protected set; }
    public int spriteId;

    public enum Status { Placed, Built, Destroyed };

    // Movement speed multiplier (multiplier of 2 means twice as slow as normal)
    // 1f = normal speed
    // "rough" tile = 2
    // table = 3
    // rough + table = (2+3) = 5 so only 1/5th speed
    public float movementCost { get; protected set; }

    int width;
    int height;
    bool obstacle;
    

    Action<PlacedObject> cbOnChanged;

    // TODO: implement larger objects
    // TODO: implement object rotation

    protected PlacedObject() { }

    // object prototype
    static public PlacedObject CreatePrototype(string objectType, int width = 1, int height = 1, bool obstacle = false, int spriteId = 0)
    {
        PlacedObject obj = new PlacedObject();
        obj.objectType = objectType;
        obj.width = width;
        obj.height = height;
        obj.obstacle = obstacle;
        obj.spriteId = spriteId;

        return obj;
    }

    static public PlacedObject PlaceObject(PlacedObject proto, Tile tile)
    {
        PlacedObject obj = new PlacedObject();

        obj.objectType = proto.objectType;
        obj.width = proto.width;
        obj.height = proto.height;
        obj.obstacle = proto.obstacle;
        obj.spriteId = proto.spriteId;

        obj.tile = tile;

        if (tile.InstallObject(obj) == false)
        {
            // For some reason we werent able to place the object on this tile
            return null;
        }

        return obj;
    }

    

    public void RegisterOnChangedCallback(Action<PlacedObject> callback)
    {
        cbOnChanged += callback;
    }

    public void UnregisterOnChangedCallback(Action<PlacedObject> callback)
    {
        cbOnChanged -= callback;
    }
}
