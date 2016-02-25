using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ExternalObject
{

    // BASE tile of the object
    public Tile tile { get; protected set; }
    public string objectType { get; protected set; }
    public int spriteId { get; protected set; }
    public string spriteName { get; protected set; }
    public Sprite[] sprites { get; protected set; }
    public int width { get; protected set; }
    public int height { get; protected set; }
    public bool obstacle { get; protected set; }

    public enum Status { Placed, Built, Destroyed };
    public enum Rotation { W, N, E, S };

    private Rotation _rotation = Rotation.W;

    Action<ExternalObject> cbOnRotationChanges;

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

    Action<ExternalObject> cbOnChanged;

    protected ExternalObject() { }

    // object prototype
    static public ExternalObject CreatePrototype(string objectType, int width = 1, int height = 1, bool obstacle = false, int spriteId = 0, string spriteName = "new_item")
    {
        ExternalObject obj = new ExternalObject();
        obj.objectType = objectType;
        obj.width = width;
        obj.height = height;
        obj.obstacle = obstacle;
        obj.spriteId = spriteId;
        obj.spriteName = spriteName;

        return obj;
    }

    static public ExternalObject PlaceObject(ExternalObject proto, Tile tile, Rotation rotation = Rotation.W)
    {
        ExternalObject obj = new ExternalObject();

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



        //if (tile.InstallObject(obj) == false)
        //{
        //    // For some reason we werent able to place the object on this tile
        //    return null;
        //}

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

    public string getType()
    {
        return objectType;
    }

    public void RegisterOnChangedCallback(Action<ExternalObject> callback)
    {
        cbOnChanged += callback;
    }

    public void UnregisterOnChangedCallback(Action<ExternalObject> callback)
    {
        cbOnChanged -= callback;
    }

    public void RegisterOnRotationChangedCallback(Action<ExternalObject> callback)
    {
        cbOnRotationChanges += callback;
    }

    public void UnregisterOnRotationChangedCallback(Action<ExternalObject> callback)
    {
        cbOnRotationChanges += callback;
    }
}
