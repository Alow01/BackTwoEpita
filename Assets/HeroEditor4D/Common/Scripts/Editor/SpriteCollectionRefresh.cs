using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Assets.HeroEditor4D.Common.Scripts.Collections;
using Assets.HeroEditor4D.Common.Scripts.Data;
using Assets.HeroEditor4D.Common.Scripts.Editor;
using UnityEditor;
using UnityEngine;

namespace HeroEditor4D.Common.Editor
{
    /// <summary>
    /// Refreshes the main sprite collection when importing new sprite bundles.
    /// </summary>
    public class SpriteCollectionRefresh : AssetPostprocessor
    {
		public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            var spriteCollection = UnityEngine.Object.FindObjectOfType<SpriteCollection>();

            if (spriteCollection != null)
            {
                Refresh(spriteCollection);
            }
        }

        public static void Refresh(SpriteCollection spriteCollection)
        {
            spriteCollection.Body = LoadSprites(spriteCollection, "/BodyParts/Body");
            spriteCollection.Ears = LoadSprites(spriteCollection, "/BodyParts/Ears");
            spriteCollection.Hair = LoadSprites(spriteCollection, "/BodyParts/Hair");
            spriteCollection.Beard = LoadSprites(spriteCollection, "/BodyParts/Beard");
            spriteCollection.Eyebrows = LoadSprites(spriteCollection, "/BodyParts/Eyebrows");
            spriteCollection.Eyes = LoadSprites(spriteCollection, "/BodyParts/Eyes");
            spriteCollection.Mouth = LoadSprites(spriteCollection, "/BodyParts/Mouth");
            
            spriteCollection.Armor = LoadSprites(spriteCollection, "/Equipment/Armor");
            spriteCollection.Cape = LoadSprites(spriteCollection, "/Equipment/Cape");
            spriteCollection.Backpack = LoadSprites(spriteCollection, "/Equipment/Back");
            spriteCollection.MeleeWeapon1H = LoadSprites(spriteCollection, "/Equipment/MeleeWeapon1H", 1);
            spriteCollection.MeleeWeapon2H = LoadSprites(spriteCollection, "/Equipment/MeleeWeapon2H", 1);
            spriteCollection.Shield = LoadSprites(spriteCollection, "/Equipment/Shield");
	        spriteCollection.Supplies = LoadSprites(spriteCollection, "/Equipment/Supplies", 1);
			spriteCollection.Bow = LoadSprites(spriteCollection, "/Equipment/Bow");
            spriteCollection.Crossbow = LoadSprites(spriteCollection, "/Equipment/Crossbow");
            spriteCollection.Firearm1H = LoadSprites(spriteCollection, "/Equipment/Firearm1H");
            spriteCollection.Firearm2H = LoadSprites(spriteCollection, "/Equipment/Firearm2H");
            spriteCollection.Throwable = LoadSprites(spriteCollection, "/Equipment/Throwable");

            spriteCollection.Makeup = LoadSprites(spriteCollection, "/Equipment/Makeup");
            spriteCollection.Mask = LoadSprites(spriteCollection, "/Equipment/Mask");
            spriteCollection.Earrings = LoadSprites(spriteCollection, "/Equipment/Earrings");

            spriteCollection.Firearm1H.ForEach(FirearmMuzzleResolver.Resolve);
            spriteCollection.Firearm2H.ForEach(FirearmMuzzleResolver.Resolve);

            EditorUtility.SetDirty(spriteCollection);

            if (spriteCollection.DebugLogging) Debug.Log("<color=yellow>SpriteCollection: refreshed.</color>");
        }
		
        private static List<ItemSprite> LoadSprites(SpriteCollection sc, string subPath, int nesting = 0)
        {
            var type = subPath.Split('/')[2];
            var collection = new List<ItemSprite>();

            foreach (var asset in sc.SpriteFolders)
            {
                if (asset == null) continue;

                var path = AssetDatabase.GetAssetPath(asset) + subPath;

                if (!Directory.Exists(path)) continue;

                var include = sc.CollectionFilter.Where(i => !i.StartsWith("!")).ToList();
                var exclude = sc.CollectionFilter.Where(i => i.StartsWith("!")).Select(i => i.Replace("!", "")).ToList();

                var extensions = new List<string> { "*.png" };

                if (sc.IncludePsd) extensions.Add("*.psd");
 
                foreach (var ext in extensions)
                {
                    var entries = Directory.GetFiles(path, ext, SearchOption.AllDirectories)
                        .Select(p => p.Replace("\\", "/"))
                        .Select(p => new ItemSprite(GetEditionName(p), GetCollectionName(p, nesting), type, Path.GetFileNameWithoutExtension(p), p, AssetDatabase.LoadAssetAtPath<Sprite>(p), AssetDatabase.LoadAllAssetsAtPath(p).OfType<Sprite>().ToList()))
                        .Where(i => (include.Count == 0 || include.Contains(i.Collection)) && !exclude.Contains(i.Collection) || sc.CollectionFilterIgnore.Any(i.Path.Contains)).ToList();

                    collection.AddRange(entries);
                }
            }

            foreach (var entry in collection)
            {
                foreach (Match match in Regex.Matches(entry.Path, @"\[(.+?)\]"))
                {
                    entry.Tags.Add(match.Groups[1].Value);
                }
            }

            return collection.OrderBy(i => i.Name).ToList();
        }

        private static string GetEditionName(string path)
        {
            var edition = path.Split('/')[2];

            return edition;
        }

        private static string GetCollectionName(string path, int nesting)
        {
            var parent = Directory.GetParent(path);

            for (var i = 0; i < nesting; i++)
            {
				parent = parent?.Parent;
			}

            if (parent == null) throw new NotSupportedException();

            return parent.Name;
        }
    }
}