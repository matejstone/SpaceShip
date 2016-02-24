using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PlacedObject
{

    // BASE tile of the object
    public Tile tile { get; protected set; }
    public string objectType { get; protected set; }
    public int spriteId { get; protected set; }
    public string spriteName { get; protected set; }
    public Sprite[] sprites { get; protected set; }

    public enum Status { Placed, Built, Destroyed };
    public enum Rotation { W, N, E, S };
    private Rotation _rotation = Rotation.W;
    Action<PlacedObject> cbOnRotationChanges;

    public Rotation rotation
    {
        get
        {
            return _rotation;
        }
        set
        {
            Rotation oldRotation = _rotation;
            _rotation = value;

            if (cbOnRotationChanges != null && oldRotation != _rotation)
                cbOnRotationChanges(this);
        }
    }

    int width;
    int height;
    bool obstacle;
    

    Action<PlacedObject> cbOnChanged;

    // TODO: implement larger objects
    // TODO: implement object rotation

    protected PlacedObject() { }

    // object prototype
    static public PlacedObject CreatePrototype(string objectType, int width = 1, int height = 1, bool obstacle = false, int spriteId = 0, string spriteName = "new_item")
    {
        PlacedObject obj = new PlacedObject();
        obj.objectType = objectType;
        obj.width = width;
        obj.height = height;
        obj.obstacle = obstacle;
        obj.spriteId = spriteId;
        obj.spriteName = spriteName;

        return obj;
    }

    static public PlacedObject PlaceObject(PlacedObject proto, Tile tile, Rotation rotation = Rotation.W)
    {
        PlacedObject obj = new PlacedObject();

        obj.objectType = proto.objectType;
        obj.width = proto.width;
        obj.height = proto.height;
        obj.obstacle = proto.obstacle;
        obj.spriteId = proto.spriteId;
        obj.spriteName = proto.spriteName;

        obj.rotation = rotation;
        obj.tile = tile;

        if (obj.FindObjectSprites().Length == 0)
        {
            // Object doesnt have a sprite, we can't display it
            Debug.LogError("Object has no sprite");
            return null;
        }



        if (tile.InstallObject(obj) == false)
        {
            // For some reason we werent able to place the object on this tile
            return null;
        }

        return obj;
    }

    private Sprite[] FindObjectSprites()
    {
        Debug.Log(spriteName);
        Debug.Log(spriteId);
        List<Sprite> spriteList = new List<Sprite>();
        Debug.Log(ShipController.Instance);
        spriteList = ShipController.Instance.GetPlacedObjectSpritesByName(spriteName, spriteId);
        sprites = new Sprite[spriteList.Count];
        sprites = spriteList.ToArray();
        return sprites;
    }

    public string getType() {
        return objectType;
    }

    public void RegisterOnChangedCallback(Action<PlacedObject> callback)
    {
        cbOnChanged += callback;
    }

    public void UnregisterOnChangedCallback(Action<PlacedObject> callback)
    {
        cbOnChanged -= callback;
    }

    public void RegisterOnRotationChangedCallback(Action<PlacedObject> callback)
    {
        cbOnRotationChanges += callback;
    }

    public void UnregisterOnRotationChangedCallback(Action<PlacedObject> callback)
    {
        cbOnRotationChanges += callback;
    }
}
