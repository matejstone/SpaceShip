using System.Xml;
using System.Xml.Serialization;

public class Item
{
    [XmlElement("name")]
    public string Name;

    [XmlElement("width")]
    public int Width;

    [XmlElement("height")]
    public int Height;

    [XmlElement("obstacle")]
    public bool Obstacle;

    [XmlElement("spriteId")]
    public int SpriteId = 0;

    [XmlElement("spriteName")]
    public string SpriteName;

    [XmlElement("type")]
    public string ItemType;

    public Item(string name = "New Item", int width = 1, int height = 1, bool obstacle = false, int spriteId = 0, string spriteName = "new_item", string type = "internal") {
        this.Name = name;
        this.Width = width;
        this.Height = height;
        this.Obstacle = obstacle;
        this.SpriteId = spriteId;
        this.SpriteName = spriteName;
        this.ItemType = type;
    }

    public void AddSprite(int spriteId) {
        this.SpriteId = spriteId;
    }

    public Item() { }
}