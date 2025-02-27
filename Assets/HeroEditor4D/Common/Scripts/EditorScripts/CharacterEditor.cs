using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Assets.HeroEditor4D.Common.Scripts.Collections;
using Assets.HeroEditor4D.Common.Scripts.Common;
using Assets.HeroEditor4D.Common.Scripts.Data;
using Assets.HeroEditor4D.Common.Scripts.Enums;
using Assets.HeroEditor4D.InventorySystem.Scripts.Data;
using Assets.HeroEditor4D.InventorySystem.Scripts.Elements;
using Assets.HeroEditor4D.Common.SimpleColorPicker.Scripts;
using Assets.HeroEditor4D.InventorySystem.Scripts;
using Assets.HeroEditor4D.InventorySystem.Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HeroEditor4D.Common.Scripts.EditorScripts
{
    /// <summary>
    /// Character editor UI and behaviour.
    /// </summary>
    public class CharacterEditor : MonoBehaviour
    {
        [Header("Main")]
        public SpriteCollection SpriteCollection;
        public IconCollection IconCollection;
        public Character4D Character;
        public Transform Tabs;
        public ScrollInventory Inventory;
        public Text ItemName;
        
        [Header("Materials")]
        public Material DefaultMaterial;
        public Material EyesPaintMaterial;
        public Material EquipmentPaintMaterial;
        public Material HuePaintMaterial;

        [Header("Other")]
        public List<string> PaintParts;
        public Button PaintButton;
        public ColorPicker ColorPicker;
        public ColorSetup ColorSetup;
        public List<string> CollectionSorting;
        public List<CollectionBackground> CollectionBackgrounds;
        public List<Button> EditorOnlyButtons;
        public string AssetUrl;
        
        [Serializable]
        public class CollectionBackground
        {
            public string Name;
            public Sprite Sprite;
        }

        public Action<Item> EquipCallback;

        private Toggle ActiveTab => Tabs.GetComponentsInChildren<Toggle>().Single(i => i.isOn);

        public void OnValidate()
        {
            if (Character == null)
            {
                Character = FindObjectOfType<Character4D>();
            }
        }

        /// <summary>
        /// Called automatically on app start.
        /// </summary>
        public void Awake()
        {
            ItemCollection.Active = ScriptableObject.CreateInstance<ItemCollection>();
            ItemCollection.Active.SpriteCollections = new List<SpriteCollection> { SpriteCollection };
            ItemCollection.Active.IconCollections = new List<IconCollection> { IconCollection };
            ItemCollection.Active.BackgroundBrown = CollectionBackgrounds[0].Sprite;
            ItemCollection.Active.GetBackgroundCustom = item => CollectionBackgrounds.SingleOrDefault(i => i.Name == item.Icon?.Collection)?.Sprite;
        }

        /// <summary>
        /// Called automatically on app start.
        /// </summary>
        public void Start()
        {
            Character.Initialize();
            OnSelectTab(true);
            EditorOnlyButtons.ForEach(i => i.interactable = Application.isEditor);
            RequestFeedback();
        }

        public void Load(Character4D character)
        {
            Character.CopyFrom(character);
        }

        /// <summary>
        /// This can be used as an example for building your own inventory UI.
        /// </summary>
        public void OnSelectTab(bool value)
        {
            if (!value) return;

            Action<Item> equipAction;
            int equippedIndex;
            var tab = Tabs.GetComponentsInChildren<Toggle>().Single(i => i.isOn);

            ItemCollection.Active.Reset();

            List<ItemSprite> SortCollection(List<ItemSprite> collection)
            {
                return collection.OrderBy(i => CollectionSorting.Contains(i.Collection) ? CollectionSorting.IndexOf(i.Collection) : 999).ThenBy(i => i.Id).ToList();
            }

            switch (tab.name)
            {
                case "Armor":
                {
                    var sprites = SortCollection(SpriteCollection.Armor);

                    ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                    equipAction = item => Character.Equip(item.Sprite, EquipmentPart.Armor);
                    equippedIndex = Character.Front.Armor == null ? -1 : sprites.FindIndex(i => i.Sprites.Contains(Character.Front.Armor.SingleOrDefault(j => j.name == "FrontBody")));
                    break;
                }
                case "Helmet":
                {
                    var sprites = SortCollection(SpriteCollection.Armor);

                    ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i, ".Armor.", ".Helmet.")).ToList();
                    equipAction = item => Character.Equip(item.Sprite, EquipmentPart.Helmet);
                    equippedIndex = sprites.FindIndex(i => i.Sprites.Contains(Character.Front.Helmet));
                    break;
                }
                case "Vest":
                case "Bracers":
                case "Leggings":
                {
                    string part;

                    switch (tab.name)
                    {
                        case "Vest": part = "FrontBody"; break;
                        case "Bracers": part = "FrontArmL"; break;
                        case "Leggings": part = "FrontLegL"; break;
                        default: throw new NotSupportedException(tab.name);
                    }

                    var sprites = SortCollection(SpriteCollection.Armor);

                    ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i, ".Armor.", $".{tab.name}.")).ToList();
                    equipAction = item => Character.Equip(item.Sprite, tab.name.ToEnum<EquipmentPart>());
                    equippedIndex = Character.Front.Armor == null ? -1 : sprites.FindIndex(i => i.Sprites.Contains(Character.Front.Armor.SingleOrDefault(j => j.name == part)));
                    break;
                }
                case "Shield":
                {
                    var sprites = SortCollection(SpriteCollection.Shield);

                    ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                    equipAction = item => Character.Equip(item.Sprite, EquipmentPart.Shield);
                    equippedIndex = Character.Front.Shield == null ? -1 : sprites.FindIndex(i => i.Sprites.SequenceEqual(Character.Front.Shield));
                    break;
                }
                case "Melee1H":
                {
                    var sprites = SortCollection(SpriteCollection.MeleeWeapon1H);

                    ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                    equipAction = item => Character.Equip(item.Sprite, EquipmentPart.MeleeWeapon1H);
                    equippedIndex = sprites.FindIndex(i => i.Sprites.Contains(Character.Front.PrimaryWeapon));
                    break;
                }
                case "Melee2H":
                {
                    var sprites = SortCollection(SpriteCollection.MeleeWeapon2H);

                    ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                    equipAction = item => Character.Equip(item.Sprite, EquipmentPart.MeleeWeapon2H);
                    equippedIndex = sprites.FindIndex(i => i.Sprites.Contains(Character.Front.PrimaryWeapon));
                    break;
                }
                case "Bow":
                {
                    var sprites = SortCollection(SpriteCollection.Bow);

                    ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                    equipAction = item => Character.Equip(item.Sprite, EquipmentPart.Bow);
                    equippedIndex = Character.Front.CompositeWeapon == null ? -1 : sprites.FindIndex(i => i.Sprites.SequenceEqual(Character.Front.CompositeWeapon));
                    break;
                }
                case "Crossbow":
                {
                    var sprites = SortCollection(SpriteCollection.Crossbow);

                    ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                    equipAction = item => Character.Equip(item.Sprite, EquipmentPart.Crossbow);
                    equippedIndex = Character.Front.CompositeWeapon == null ? -1 : sprites.FindIndex(i => i.Sprites.SequenceEqual(Character.Front.CompositeWeapon));
                    break;
                }
                case "Firearm1H":
                {
                    var sprites = SortCollection(SpriteCollection.Firearm1H);

                    ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                    equipAction = item => Character.Equip(item.Sprite, EquipmentPart.Firearm1H);
                    equippedIndex = Character.Front.SecondaryWeapon == null ? -1 : sprites.FindIndex(i => i.Sprites.Contains(Character.Front.PrimaryWeapon));
                    break;
                }
                case "Firearm2H":
                {
                    var sprites = SortCollection(SpriteCollection.Firearm2H);

                    ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                    equipAction = item => Character.Equip(item.Sprite, EquipmentPart.Firearm2H);
                    equippedIndex = Character.Front.PrimaryWeapon == null ? -1 : sprites.FindIndex(i => i.Sprites.Contains(Character.Front.PrimaryWeapon));
                    break;
                }
                case "SecondaryFirearm1H":
                {
                    var sprites = SortCollection(SpriteCollection.Firearm1H);

                    ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                    equipAction = item => Character.Equip(item.Sprite, EquipmentPart.SecondaryFirearm1H);
                    equippedIndex = Character.Front.SecondaryWeapon == null ? -1 : sprites.FindIndex(i => i.Sprites.Contains(Character.Front.SecondaryWeapon));
                    break;
                }
                case "Body":
                {
                    var sprites = SortCollection(SpriteCollection.Body);

                    ItemCollection.Active.Items = SpriteCollection.Body.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                    equipAction = item => Character.SetBody(item.Sprite, BodyPart.Body);
                    equippedIndex = Character.Front.Body == null ? -1 : sprites.FindIndex(i => i.Sprites.SequenceEqual(Character.Front.Body));
                    break;
                }
                case "Head":
                {
                    var sprites = SortCollection(SpriteCollection.Body);

                    ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                    equipAction = item => Character.SetBody(item.Sprite, BodyPart.Head);
                    equippedIndex = Character.Front.Head == null ? -1 : sprites.FindIndex(i => i.Sprites.Contains(Character.Front.Head));
                    break;
                }
                case "Ears":
                {
                    var sprites = SortCollection(SpriteCollection.Ears);

                    ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                    equipAction = item => Character.SetBody(item.Sprite, BodyPart.Ears);
                    equippedIndex = Character.Front.Ears == null ? -1 : sprites.FindIndex(i => i.Sprites.SequenceEqual(Character.Front.Ears));
                    break;
                }
                case "Eyebrows":
                {
                    var sprites = SortCollection(SpriteCollection.Eyebrows);

                    ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                    equipAction = item => Character.SetBody(item.Sprite, BodyPart.Eyebrows);
                    equippedIndex = sprites.FindIndex(i => i.Sprites.Contains(Character.Front.Expressions[0].Eyebrows));
                    break;
                }
                case "Eyes":
                {
                    var sprites = SortCollection(SpriteCollection.Eyes);

                    ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                    equipAction = item => Character.SetBody(item.Sprite, BodyPart.Eyes);
                    equippedIndex = sprites.FindIndex(i => i.Sprites.Contains(Character.Front.Expressions[0].Eyes));
                    break;
                }
                case "Hair":
                {
                    var sprites = SortCollection(SpriteCollection.Hair);

                    ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                    equipAction = item => Character.SetBody(item.Sprite, BodyPart.Hair);
                    equippedIndex = sprites.FindIndex(i => i.Sprites.Contains(Character.Front.Hair));
                    break;
                }
                case "Beard":
                {
                    var sprites = SortCollection(SpriteCollection.Beard);

                    ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                    equipAction = item => Character.SetBody(item.Sprite, BodyPart.Beard);
                    equippedIndex = sprites.FindIndex(i => i.Sprites.Contains(Character.Front.Beard));
                    break;
                }
                case "Mouth":
                {
                    var sprites = SortCollection(SpriteCollection.Mouth);

                    ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                    equipAction = item => Character.SetBody(item.Sprite, BodyPart.Mouth);
                    equippedIndex = sprites.FindIndex(i => i.Sprites.Contains(Character.Front.Expressions[0].Mouth));
                    break;
                }
                case "Makeup":
                {
                    var sprites = SortCollection(SpriteCollection.Makeup);

                    ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                    equipAction = item => Character.SetBody(item.Sprite, BodyPart.Makeup);
                    equippedIndex = sprites.FindIndex(i => i.Sprites.Contains(Character.Front.Makeup));
                    break;
                }
                case "Mask":
                {
                    var sprites = SortCollection(SpriteCollection.Mask);

                    ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                    equipAction = item => Character.Equip(item.Sprite, EquipmentPart.Mask, item.Sprite != null && item.Sprite.Tags.Contains("Paint") ? (Color?) null : Color.white);
                    equippedIndex = sprites.FindIndex(i => i.Sprites.Contains(Character.Front.Mask));
                    break;
                }
                case "Earrings":
                {
                    var sprites = SortCollection(SpriteCollection.Earrings);

                    ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                    equipAction = item => Character.Equip(item.Sprite, EquipmentPart.Earrings);
                    equippedIndex = Character.Front.Earrings == null ? -1 : sprites.FindIndex(i => i.Sprites.SequenceEqual(Character.Front.Earrings));
                    break;
                }
                case "Supplies":
                {
                    var sprites =SortCollection(SpriteCollection.Supplies);

                    ItemCollection.Active.Items = sprites.Select(i => CreateFakeItemParams(new Item(i.Id), i)).ToList();
                    equipAction = item => { if (item.Id != null) Debug.LogWarning("Supplies are present as icons only and are not displayed on a character. Can be used for inventory."); };
                    equippedIndex = -1;
                    break;
                }
                default:
                    throw new NotImplementedException(tab.name);
            }

            var items = ItemCollection.Active.Items.Select(i => new Item(i.Id)).ToList();
            var emptyItem = new Item(null);

            items.Insert(0, emptyItem);
            ItemCollection.Active.Items.Add(CreateFakeItemParams(emptyItem, null));

            InventoryItem.OnLeftClick = item =>
            {
                equipAction?.Invoke(item);
                EquipCallback?.Invoke(item);
                ItemName.text = item == emptyItem ? emptyItem.Id : item.Params.SpriteId;
                SetPaintButton(tab.name, item);
            };
            Inventory.Initialize(ref items, items[equippedIndex + 1], reset: true);
            Inventory.ScrollRect.verticalNormalizedPosition = 1;

            var equipped = items.Count > equippedIndex + 1 ? items[equippedIndex + 1] : null;

            SetPaintButton(tab.name, equipped);
        }

        private ItemParams CreateFakeItemParams(Item item, ItemSprite itemSprite, string replaceable = null, string replacement = null)
        {
            var spriteId = itemSprite?.Id;
            var iconId = itemSprite?.Id;
            var rarity = ItemRarity.Common;

            if (itemSprite != null)
            {
                switch (itemSprite.Collection)
                {
                    case "Basic":
                    case "Undead":
                        break;
                    default:
                        rarity = ItemRarity.Epic;
                        break;
                }
            }

            if (iconId != null && item.Id != null && replaceable != null && replacement != null)
            {
                iconId = iconId.Replace(replaceable, replacement);
            }

            return new ItemParams { Id = item.Id, IconId = iconId, SpriteId = spriteId, Rarity = rarity, Meta = itemSprite == null ? null : Serializer.Serialize(itemSprite.Tags) };
        }

        private void SetPaintButton(string tab, Item item)
        {
            var tags = item?.Params.MetaToList() ?? new List<string>();

            if (PaintParts.Contains(tab) && !tags.Contains("NoPaint") || tags.Contains("Paint"))
            {
                PaintButton.interactable = true;
                PaintButton.onClick.AddListener(OpenColorPicker);
            }
            else
            {
                PaintButton.interactable = false;
                //PaintButton.onClick.AddListener(OpenColorSetup);
            }
        }

        /// <summary>
        /// Remove all equipment and reset appearance.
        /// </summary>
        public void Reset()
        {
            Character.Parts.ForEach(i => i.ResetEquipment());
            new CharacterAppearance().Setup(Character.GetComponent<Character4D>());
        }

        /// <summary>
        /// Randomize character.
        /// </summary>
        public void Randomize()
        {
            Character.Randomize();
            OnSelectTab(true);
        }

        /// <summary>
	    /// Save character to json.
	    /// </summary>
	    public void SaveToJson()
	    {
            StartCoroutine(StandaloneFilePicker.SaveFile("Save as JSON", "", "New character", ".json", Encoding.Default.GetBytes(Character.ToJson()), (success, path) => { Debug.Log(success ? $"Saved as {path}" : "Error saving."); }));
		}

		/// <summary>
		/// Load character from json.
		/// </summary>
		public void LoadFromJson()
	    {
            StartCoroutine(StandaloneFilePicker.OpenFile("Open as JSON", "", ".json", (success, path, bytes) =>
            {
                if (success)
                {
                    var json = System.IO.File.ReadAllText(path);

                    Character.FromJson(json, silent: false);
                }
            }));
	    }

        #if UNITY_EDITOR

        private string _path;

        /// <summary>
        /// Save character to prefab.
        /// </summary>
        public void Save()
        {
            var path = UnityEditor.EditorUtility.SaveFilePanel("Save character prefab (should be inside Assets folder)", _path, "New character", "prefab");

            if (path.Length > 0)
            {
                if (!path.Contains("/Assets/")) throw new Exception("Unity can save prefabs only inside Assets folder!");

                Save("Assets" + path.Replace(Application.dataPath, null));
                _path = path;
            }
		}

	    /// <summary>
		/// Load character from prefab.
		/// </summary>
		public void Load()
        {
            var path = UnityEditor.EditorUtility.OpenFilePanel("Load character prefab", _path, "prefab");

            if (path.Length > 0)
            {
                Load("Assets" + path.Replace(Application.dataPath, null));
                _path = path;
            }
		}

	    public void Save(string path)
		{
			Character.transform.localScale = Vector3.one;

			#if UNITY_2018_3_OR_NEWER

			UnityEditor.PrefabUtility.SaveAsPrefabAsset(Character.gameObject, path);

			#else

			UnityEditor.PrefabUtility.CreatePrefab(path, Character.gameObject);

			#endif

            Debug.LogFormat("Prefab saved as {0}", path);
        }

        public void Load(string path)
        {
			var character = UnityEditor.AssetDatabase.LoadAssetAtPath<Character4D>(path);

			Load(character);
        }

        #else

        public void Save(string path)
        {
            throw new System.NotSupportedException();
        }

        public void Load(string path)
        {
            throw new System.NotSupportedException();
        }

        #endif

        private Color _color;

        public void OpenColorPicker()
        {
            var currentColor = ResolveParts(ActiveTab.name).FirstOrDefault()?.color ?? Color.white;

            ColorPicker.Color = _color = currentColor;
            ColorPicker.OnColorChanged = Paint;
            ColorPicker.SetActive(true);
        }

        public void CloseColorPicker(bool apply)
        {
            if (!apply) Paint(_color);

            ColorPicker.SetActive(false);
        }

        public void OpenColorSetup()
        {
            var currentColor = ResolveParts(ActiveTab.name).FirstOrDefault()?.color ?? Color.white;

            ColorSetup.Color = _color = currentColor;
            ColorSetup.OnColorChanged = SetupColor;
            ColorSetup.SetActive(true);
        }

        public void CloseColorSetup(bool apply)
        {
            Color.RGBToHSV(_color, out var h, out var s, out var v);

            if (!apply) SetupColor(0, s, v);

            ColorSetup.SetActive(false);
        }

        /// <summary>
        /// Pick color and apply to sprite.
        /// </summary>
        public void Paint(Color color)
        {
            foreach (var part in ResolveParts(ActiveTab.name))
            {
                part.color = color;
                part.sharedMaterial = color == Color.white ? DefaultMaterial : ActiveTab.name == "Eyes" ? EyesPaintMaterial : EquipmentPaintMaterial;
            }

            if (ActiveTab.name == "Eyes")
            {
                foreach (var expression in Character.Parts.SelectMany(i => i.Expressions))
                {
                    if (expression.Name != "Dead") expression.EyesColor = color;
                }
            }
        }

        /// <summary>
        /// Pick HSB and apply to sprite.
        /// </summary>
        public void SetupColor(float h, float s, float v)
        {
            var color = Color.HSVToRGB(0, s, v);

            foreach (var part in ResolveParts(ActiveTab.name))
            {
                part.color = color;
                part.sharedMaterial = h <= 0 ? DefaultMaterial : HuePaintMaterial;

                if (h > 0)
                {
                    var huePaint = part.GetComponent<HuePaint>() ?? part.gameObject.AddComponent<HuePaint>();

                    huePaint.Hue = h;
                    huePaint.ShiftHue();
                }
                else
                {
                    var huePaint = part.GetComponent<HuePaint>();

                    if (huePaint)
                    {
                        Destroy(huePaint);
                    }
                }
            }
        }

        protected List<SpriteRenderer> ResolveParts(string target)
        {
            switch (target)
            {
                case "Helmet": return Character.Parts.Select(i => i.HelmetRenderer).ToList();
                case "Ears": return Character.Parts.SelectMany(i => i.EarsRenderers).ToList();
                case "Body": return Character.Parts.SelectMany(i => i.BodyRenderers.Union(new List<SpriteRenderer> { i.HeadRenderer }).Union(i.EarsRenderers)).ToList();
                case "Head": return Character.Parts.Select(i => i.HeadRenderer).ToList();
                case "Hair": return Character.Parts.Select(i => i.HairRenderer).ToList();
                case "Beard": return Character.Parts.Select(i => i.BeardRenderer).Where(i => i != null).ToList();
                case "Eyes": return Character.Parts.Select(i => i.EyesRenderer).Where(i => i != null).ToList();
                case "Eyebrows": return Character.Parts.Select(i => i.EyebrowsRenderer).Where(i => i != null).ToList();
                case "Mouth": return Character.Parts.Select(i => i.MouthRenderer).Where(i => i != null).ToList();
                case "Mask": return Character.Parts.Select(i => i.MaskRenderer).Where(i => i != null).ToList();
                case "Makeup": return Character.Parts.Select(i => i.MakeupRenderer).Where(i => i != null).ToList();
                default: throw new NotImplementedException(target);
            }
        }

        protected void FeedbackTip()
	    {
			#if UNITY_EDITOR

		    var success = UnityEditor.EditorUtility.DisplayDialog("HeroView Editor", "Hi! Thank you for using my asset! I hope you enjoy making your game with it. The only thing I would ask you to do is to leave a review on the Asset Store. It would be awesome support for my asset, thanks!", "Review", "Later");
			
			RequestFeedbackResult(success);

            #endif
        }

        /// <summary>
        /// Navigate to URL.
        /// </summary>
        public void Navigate(string url)
        {
            Application.OpenURL(url);
        }

        private const string FeedbackRequestTimeKey = "CE:FeedbackRequestTime";
        private const string FeedbackTimeKey = "CE:FeedbackTime";

        protected void RequestFeedback()
        {
            if (PlayerPrefs.HasKey(FeedbackTimeKey) && (DateTime.UtcNow - new DateTime(long.Parse(PlayerPrefs.GetString(FeedbackTimeKey)))).TotalDays < 14)
            {
                return;
            }

            if (!PlayerPrefs.HasKey(FeedbackRequestTimeKey))
            {
                PlayerPrefs.SetString(FeedbackRequestTimeKey, DateTime.UtcNow.AddHours(-23).Ticks.ToString());
            }
            else if ((DateTime.UtcNow - new DateTime(long.Parse(PlayerPrefs.GetString(FeedbackRequestTimeKey)))).TotalDays > 1)
            {
                FeedbackTip();
            }
        }

        protected void RequestFeedbackResult(bool success)
        {
            if (success)
            {
                PlayerPrefs.SetString(FeedbackTimeKey, DateTime.UtcNow.Ticks.ToString());
                Application.OpenURL(AssetUrl);
            }
            else if (PlayerPrefs.HasKey(FeedbackTimeKey))
            {
                PlayerPrefs.SetString(FeedbackRequestTimeKey, DateTime.UtcNow.AddDays(7).Ticks.ToString());
            }
            else
            {
                PlayerPrefs.SetString(FeedbackRequestTimeKey, DateTime.UtcNow.Ticks.ToString());
            }
        }
    }
}