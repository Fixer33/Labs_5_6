using System;

namespace Labs_5_6.Models
{
    public class Item
    {
        public string Id { get; private set; }
        public string Name { get; private set; }

        public Item(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}