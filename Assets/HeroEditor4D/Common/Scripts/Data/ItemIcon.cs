using System;
using UnityEngine;

namespace Assets.HeroEditor4D.Common.Scripts.Data
{
	[Serializable]
    public class ItemIcon
    {
        public string Name;
        public string Id;
        public string Edition;
        public string Collection;
        public string Path;
        public Sprite Sprite;

        public ItemIcon(string edition, string collection, string type, string name, string path, Sprite sprite)
        {
            Id = $"{edition}.{collection}.{type}.{name}";
            Name = name;
            Collection = collection;
            Edition = edition;
            Path = path;
            Sprite = sprite;
        }
    }
}