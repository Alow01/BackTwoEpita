using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Assets.HeroEditor4D.Common.Scripts.Common;
using Assets.HeroEditor4D.Common.Scripts.Data;
using Assets.HeroEditor4D.Common.Scripts.Enums;
using UnityEngine;

namespace Assets.HeroEditor4D.Common.Scripts.CharacterScripts
{
    public partial class Character
    {
        public string ToJson()
        {
            if (SpriteCollection == null) throw new Exception("SpriteCollection is null!");

            var description = new Dictionary<string, string>
            {
                { "Body", SpriteToString(SpriteCollection.Body, BodyRenderers[0]) },
                { "Ears", SpriteToString(SpriteCollection.Ears, EarsRenderers[0]) },
                { "Hair", SpriteToString(SpriteCollection.Hair, HairRenderer) },
                { "Beard", SpriteToString(SpriteCollection.Beard, BeardRenderer) },
                { "Helmet", SpriteToString(SpriteCollection.Armor, HelmetRenderer) },
                { "Armor", SpritesToString(SpriteCollection.Armor, ArmorRenderers) },
                { "PrimaryWeapon", SpriteToString(GetWeaponCollection(WeaponType), PrimaryWeaponRenderer) },
                { "SecondaryWeapon", SpriteToString(SpriteCollection.Firearm1H, SecondaryWeaponRenderer) }, // TODO:
                { "Cape", SpriteToString(SpriteCollection.Cape, CapeRenderer) },
                { "Backpack", SpriteToString(SpriteCollection.Backpack, BackpackRenderer) },
                { "Shield", SpriteToString(SpriteCollection.Shield, ShieldRenderers[0]) },
                { "Bow", SpriteToString(SpriteCollection.Bow, BowRenderers[0]) },
                { "Crossbow", SpriteToString(GetWeaponCollection(WeaponType), PrimaryWeaponRenderer) },
                { "WeaponType", WeaponType.ToString() },
                { "Expression", Expression },
                { "HideEars", HideEars.ToString() },
                { "CropHair", CropHair.ToString() },
                { "Makeup", SpriteToString(SpriteCollection.Makeup, MakeupRenderer) },
                { "Mask", SpriteToString(SpriteCollection.Mask, MaskRenderer) },
                { "Earrings", SpriteToString(SpriteCollection.Earrings, EarringsRenderers[0]) }
            };

            foreach (var expression in Expressions)
            {
                description.Add($"Expression.{expression.Name}.Eyebrows", SpriteToString(SpriteCollection.Eyebrows, expression.Eyebrows, EyebrowsRenderer.color));
                description.Add($"Expression.{expression.Name}.Eyes", SpriteToString(SpriteCollection.Eyes, expression.Eyes, EyesRenderer.color));
                description.Add($"Expression.{expression.Name}.EyesColor", "#" + ColorUtility.ToHtmlStringRGBA(expression.EyesColor));
                description.Add($"Expression.{expression.Name}.Mouth", SpriteToString(SpriteCollection.Mouth, expression.Mouth, MouthRenderer.color));
            }

            return Serializer.Serialize(description);
        }

        public void LoadFromJson(string json, bool silent)
        {
            var description = Serializer.DeserializeDict(json); 

            if (SpriteCollection == null) throw new Exception("SpriteCollection is null!");

            RestoreFromString(ref Body, BodyRenderers, SpriteCollection.Body, description["Body"], silent);
            RestoreFromString(ref Head, HeadRenderer, SpriteCollection.Body, description["Body"], silent);
            RestoreFromString(ref Ears, EarsRenderers, SpriteCollection.Ears, description["Ears"], silent);
            RestoreFromString(ref Hair, HairRenderer, SpriteCollection.Hair, description["Hair"], silent);
            RestoreFromString(ref Beard, BeardRenderer, SpriteCollection.Beard, description["Beard"], silent);
            RestoreFromString(ref Helmet, HelmetRenderer, SpriteCollection.Armor, description["Helmet"], silent);
            RestoreFromString(ref Armor, ArmorRenderers, SpriteCollection.Armor, description["Armor"], silent);
            WeaponType = (WeaponType) Enum.Parse(typeof(WeaponType), description["WeaponType"], silent);
            RestoreFromString(ref PrimaryWeapon, PrimaryWeaponRenderer, GetWeaponCollection(WeaponType), description["PrimaryWeapon"], silent);
            RestoreFromString(ref SecondaryWeapon, SecondaryWeaponRenderer, SpriteCollection.Firearm1H, description["SecondaryWeapon"], silent);
            RestoreFromString(ref Cape, CapeRenderer, SpriteCollection.Cape, description["Cape"], silent);
            RestoreFromString(ref Backpack, BackpackRenderer, SpriteCollection.Backpack, description["Backpack"], silent);
            RestoreFromString(ref Shield, ShieldRenderers, SpriteCollection.Shield, description["Shield"], silent);
            RestoreFromString(ref CompositeWeapon, BowRenderers, SpriteCollection.Bow, description["Bow"], silent);
            Expression = description["Expression"];
            Expressions = new List<Expression>();
            HideEars = description.ContainsKey("HideEars") && bool.Parse(description["HideEars"]);
            CropHair = description.ContainsKey("HideHair") && bool.Parse(description["CropHair"]);

            RestoreFromString(ref Makeup, MakeupRenderer, SpriteCollection.Makeup, description["Makeup"], silent);
            RestoreFromString(ref Mask, MaskRenderer, SpriteCollection.Mask, description["Mask"], silent);
            RestoreFromString(ref Earrings, EarringsRenderers, SpriteCollection.Earrings, description["Earrings"], silent);
            
            foreach (var key in description.Keys)
            {
                if (key.Contains("Expression."))
                {
                    var parts = key.Split('.');
                    var expressionName = parts[1];
                    var expressionPart = parts[2];
                    var expression = Expressions.SingleOrDefault(i => i.Name == expressionName);

                    if (expression == null)
                    {
                        expression = new Expression { Name = expressionName };
                        Expressions.Add(expression);
                    }

                    switch (expressionPart)
                    {
                        case "Eyebrows":
                            RestoreFromString(ref expression.Eyebrows, EyebrowsRenderer, SpriteCollection.Eyebrows, description[key]);
                            break;
                        case "Eyes":
                            RestoreFromString(ref expression.Eyes, EyesRenderer, SpriteCollection.Eyes, description[key]);
                            break;
                        case "EyesColor":
                            ColorUtility.TryParseHtmlString(description[key], out var color);
                            expression.EyesColor = color;
                            break;
                        case "Mouth":
                            RestoreFromString(ref expression.Mouth, MouthRenderer, SpriteCollection.Mouth, description[key]);
                            break;
                        default:
                            throw new NotSupportedException(expressionPart);
                    }
                }
            }

            Initialize();
        }

        private IEnumerable<ItemSprite> GetWeaponCollection(WeaponType weaponType)
        {
            switch (weaponType)
            {
                case WeaponType.Melee1H: return SpriteCollection.MeleeWeapon1H;
                case WeaponType.Paired: return SpriteCollection.MeleeWeapon1H;
                case WeaponType.Melee2H: return SpriteCollection.MeleeWeapon2H;
                case WeaponType.Bow: return SpriteCollection.Bow;
                case WeaponType.Crossbow: return SpriteCollection.Crossbow;
                case WeaponType.Firearm1H: return SpriteCollection.Firearm1H;
                case WeaponType.Firearm2H: return SpriteCollection.Firearm2H;
                case WeaponType.Throwable: return SpriteCollection.Throwable;
                default:
                    throw new NotSupportedException(weaponType.ToString());
            }
        }

        private static string SpriteToString(IEnumerable<ItemSprite> collection, SpriteRenderer renderer)
        {
            if (renderer == null) return null;

            return SpriteToString(collection, renderer.sprite, renderer.color);
        }

        private static string SpritesToString(IEnumerable<ItemSprite> collection, List<SpriteRenderer> renderers)
        {
            var values = renderers.Select(i => SpriteToString(collection, i));

            return string.Join(",", values);
        }

        private static string SpriteToString(IEnumerable<ItemSprite> collection, Sprite sprite, Color color)
        {
            if (sprite == null) return null;

            var entry = collection.SingleOrDefault(i => i.Sprite == sprite || i.Sprites.Any(j => j == sprite));

            if (entry == null)
            {
                throw new Exception($"Can't find {sprite.name} in SpriteCollection.");
            }

            var result = color == Color.white ? entry.Id : entry.Id + "#" + ColorUtility.ToHtmlStringRGBA(color);

            return result;
        }

        private static void RestoreFromString(ref Sprite sprite, SpriteRenderer renderer, IEnumerable<ItemSprite> collection, string serialized, bool silent = false)
        {
            if (renderer == null) return;

            if (string.IsNullOrEmpty(serialized))
            {
                sprite = renderer.sprite = null;
                //renderer.color = Color.white;
                return;
            }

            var parts = serialized.Split('#');
            var id = parts[0];
            var color = Color.white;

            if (parts.Length > 1)
            {
                ColorUtility.TryParseHtmlString("#" + parts[1], out color);
            }

            var entries = collection.Where(i => i.Id == id).ToList();

            switch (entries.Count)
            {
                case 1:
                    sprite = entries[0].Sprites.Count == 1 ? entries[0].Sprites[0] : renderer.GetComponent<SpriteMapping>().FindSprite(entries[0].Sprites);
                    renderer.color = color;
                    break;
                case 0:
                    if (silent) Debug.LogWarning("entries.Count = " + entries.Count); else throw new Exception($"Entry with id {id} not found in SpriteCollection."); break;
                default:
                    if (silent) Debug.LogWarning("entries.Count = " + entries.Count); else throw new Exception($"Multiple entries with id {id} found in SpriteCollection."); break;
            }
        }

        private static void RestoreFromString(ref List<Sprite> sprites, List<SpriteRenderer> renderers, List<ItemSprite> collection, string serialized, bool silent = false)
        {
            if (string.IsNullOrEmpty(serialized))
            {
                sprites = new List<Sprite>();

                foreach (var renderer in renderers)
                {
                    renderer.sprite = null;
                }

                return;
            }

            sprites.Clear();

            var values = serialized.Split(',');

            for (var i = 0; i < values.Length; i++)
            {
                if (values[i] == "") continue;

                var match = Regex.Match(values[i], @"(?<Id>[\w\. \[\]]+)(?<Color>#\w+)*");
                var id = match.Groups["Id"].Value;
                var color = Color.white;

                if (match.Groups["Color"].Success)
                {
                    ColorUtility.TryParseHtmlString(match.Groups["Color"].Value, out color);
                }

                var entries = collection.Where(i => i.Id == id).ToList();

                switch (entries.Count)
                {
                    case 1:
                        if (values.Length == 1)
                        {
                            sprites = entries[0].Sprites.ToList();
                            renderers.ForEach(j => j.color = color);
                        }
                        else
                        {
                            sprites.Add(renderers[i].GetComponent<SpriteMapping>().FindSprite(entries[0].Sprites));
                            renderers[i].color = color;
                        }
                        break;
                    case 0:
                        if (silent) Debug.LogWarning("entries.Count = " + entries.Count); else throw new Exception($"Entry with id {id} not found in SpriteCollection."); break;
                    default:
                        if (silent) Debug.LogWarning("entries.Count = " + entries.Count); else throw new Exception($"Multiple entries with id {id} found in SpriteCollection."); break;
                }
            }
        }
    }
}