using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room {

    Ship ship;
    public int X { get; protected set; }
    public int Y { get; protected set; }
    public int width { get; protected set; }
    public int height { get; protected set; }
    public List<Tile> roomTiles { get; protected set; }

    public string name { get; protected set; }

    public Room(Ship ship, int x, int y, int width, int height, string name = "New Room")
    {
        this.ship = ship;
        this.X = x;
        this.Y = y;
        this.width = width;
        this.height = height;

        if (X + width > ship.Width) {
            X = ship.Width;
        }

        if (Y + height > ship.Height) {
            Y = ship.Height;
        }

        this.name = name;
    }

    public void createRoom()
    {
        roomTiles = new List<Tile>();

        for (int x = X; x <= X + width; x++)
        {
            for (int y = Y; y <= Y + height; y++)
            {
                if(ship.tiles[x, y].Type == Tile.TileType.Void)
                {
                    if (x == X || x == X + width || y == Y || y == Y + height)
                    {
                        ship.tiles[x, y].Type = Tile.TileType.Wall;
                    }
                    else {
                        ship.tiles[x, y].Type = Tile.TileType.Floor;
                        roomTiles.Add(ship.tiles[x, y]);
                    }

                    ship.tiles[x, y].setRoom(this);
                }
            }
        }
    }
}