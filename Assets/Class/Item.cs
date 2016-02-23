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
    public int spriteId = 0;

    public Item(string name = "New Item", int width = 1, int height = 1, bool obstacle = false) {
        this.Name = name;
        this.Width = width;
        this.Height = height;
        this.Obstacle = obstacle;
    }

    public void AddSprite(int spriteId) {
        this.spriteId = spriteId;
    }

    public Item() { }
}