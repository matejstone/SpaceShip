using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

[XmlRoot("ItemCollection")]
public class ItemContainer
{
    [XmlArray("Items"), XmlArrayItem("Item")]
    public List<Item> Items;
    
    
    public ItemContainer() {
        Items = new List<Item>();
    }

    public void AddItem(Item item) {
        Items.Add(item);
    }

    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(ItemContainer));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static ItemContainer Load(string path)
    {
        var serializer = new XmlSerializer(typeof(ItemContainer));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as ItemContainer;
        }   
    }

    //Loads the xml directly from the given string. Useful in combination with www.text.
    public static ItemContainer LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(ItemContainer));
        return serializer.Deserialize(new StringReader(text)) as ItemContainer;
    }
}