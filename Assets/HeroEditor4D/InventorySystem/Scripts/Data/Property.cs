using System;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using Assets.HeroEditor4D.Common.Scripts.Common;
using Assets.HeroEditor4D.InventorySystem.Scripts.Enums;
using UnityEngine;

namespace Assets.HeroEditor4D.InventorySystem.Scripts.Data
{
    /// <summary>
    /// Represents key-value pair for storing item params.
    /// </summary>
    [Serializable]
    public class Property
    {
        public PropertyId Id;
        public string Value;

        [HideInInspector] [NonSerialized] public int ValueInt;
        [HideInInspector] [NonSerialized] public int Min;
        [HideInInspector] [NonSerialized] public int Max;
        [HideInInspector] [NonSerialized] public int Duration;
        [HideInInspector] [NonSerialized] public ElementId Element;
        [HideInInspector] [NonSerialized] public bool Percentage;

        public Property()
        {
        }

        public Property(PropertyId id, object value)
        {
            Id = id;
            Value = value.ToString();
            ParseValue();
        }

        public void ParseValue()
        {
            var parts = Value.Split('/');

            if (Id == PropertyId.Damage || Id == PropertyId.Resistance)
            {
                switch (parts.Length)
                {
                    case 2:
                        Element = parts[1].ToEnum<ElementId>();
                        break;
                    case 3:
                        Element = parts[1].ToEnum<ElementId>();
                        Duration = int.Parse(parts[2]);
                        break;
                    default:
                        Element = ElementId.Physic;
                        break;
                }
            }

            if (Regex.IsMatch(parts[0], @"^\d+-\d+$"))
            {
                parts = parts[0].Split('-');
                Min = int.Parse(parts[0]);
                Max = int.Parse(parts[1]);
            }
            else if (parts[0].EndsWith("%"))
            {
                ValueInt = int.Parse(parts[0].Replace("%", null));
                Percentage = true;
            }
            else
            {
                if (int.TryParse(parts[0], out var valueInt))
                {
                    ValueInt = valueInt;
                }
            }
        }

        public void ReplaceValue(string value)
        {
            Value = value;
            ParseValue();
        }

        public void ReplaceValue(float value)
        {
            ReplaceValue(Mathf.RoundToInt(value));
        }

        public void ReplaceValue(int value)
        {
            Value = value.ToString();
            ParseValue();
        }

        public void Add(float value)
        {
            Add(Mathf.RoundToInt(value));
        }

        public void Add(int value)
        {
            if (Min > 0)
            {
                Min += value;
                Max += value;
                Value = $"{Min}-{Max}" + (Element == ElementId.Physic ? null : "/" + Element);
            }
            else
            {
                ValueInt += value;
                Value = ValueInt + (Element == ElementId.Physic ? null : "/" + Element);
            }
        }

        public void AddInPercentage(float value)
        {
            if (Min > 0)
            {
                Min = Mathf.RoundToInt(Min * (1 + value / 100f));
                Max = Mathf.RoundToInt(Max * (1 + value / 100f));
                Value = $"{Min}-{Max}" + (Element == ElementId.Physic ? null : "/" + Element);
            }
            else
            {
                ValueInt = Mathf.RoundToInt(ValueInt * (1 + value / 100f));
                Value = ValueInt + (Element == ElementId.Physic ? null : "/" + Element);
            }
        }

        public static Property Parse(string value)
        {
            var parts = value.Split('=');
            var property = new Property
            {
                Id = parts[0].ToEnum<PropertyId>(),
                Value = parts[1]
            };

            property.ParseValue();

            return property;
        }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            ParseValue();
        }
    }
}