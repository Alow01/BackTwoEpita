using System;
using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor4D.Common.Scripts.Common;
using Assets.HeroEditor4D.InventorySystem.Scripts.Data;
using Assets.HeroEditor4D.InventorySystem.Scripts.Enums;
using Assets.HeroEditor4D.InventorySystem.Scripts.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HeroEditor4D.InventorySystem.Scripts
{
    /// <summary>
    /// High-level inventory interface.
    /// </summary>
    public class InventoryBase : ItemWorkspace
    {
        public Equipment Equipment;
        public ScrollInventory PlayerInventory;
        public ScrollInventory Materials;
        public Button EquipButton;
        public Button RemoveButton;
        public Button CraftButton;
        public Button LearnButton;
        public Button UseButton;
        public Button AssembleButton;
        public AudioClip EquipSound;
        public AudioClip CraftSound;
        public AudioClip UseSound;
        public AudioSource AudioSource;
        public bool InitializeExample;

        // These callbacks can be used outside;
        public Action<Item> OnRefresh;
        public Action<Item> OnEquip;
        public Func<Item, bool> CanEquip = i => true;

        public void Awake()
        {
            ItemCollection.Active = ItemCollection;
        }

        public void Start()
        {
            if (InitializeExample)
            {
                TestInitialize();
            }
        }

        /// <summary>
        /// Initialize owned items (just for example).
        /// </summary>
        public void TestInitialize()
        {
            var inventory = ItemCollection.Active.Items.Select(i => new Item(i.Id)).ToList(); // inventory.Clear();
			var equipped = new List<Item>();

            Initialize(ref inventory, ref equipped, 6, null);
		}

        public void Initialize(ref List<Item> inventory, ref  List<Item> equipped, int bagSize, Action onRefresh)
        {
            RegisterCallbacks();
            PlayerInventory.Initialize(ref inventory);
            Equipment.SetBagSize(bagSize);
            Equipment.Initialize(ref equipped);
            Equipment.OnRefresh = onRefresh;

            if (!Equipment.SelectAny() && !PlayerInventory.SelectAny())
            {
                ItemInfo.Reset();
            }
        }

        public void RegisterCallbacks()
        {
            InventoryItem.OnLeftClick = SelectItem;
            InventoryItem.OnRightClick = InventoryItem.OnDoubleClick = QuickAction;
        }

        private void QuickAction(Item item)
        {
            SelectItem(item);

            if (Equipment.Items.Contains(item))
            {
                Remove();
            }
            else if (CanEquipSelectedItem())
            {
                Equip();
            }
        }

        public void SelectItem(Item item)
        {
            SelectedItem = item;
            ItemInfo.Initialize(SelectedItem, SelectedItem.Params.Price);
            Refresh();
        }

        public void Equip()
        {
            if (!CanEquip(SelectedItem)) return;

            var equipped = SelectedItem.IsFirearm
                ? Equipment.Items.Where(i => i.IsFirearm).ToList()
                : Equipment.Items.Where(i => i.Params.Type == SelectedItem.Params.Type && !i.IsFirearm).ToList();

            if (equipped.Any())
            {
                AutoRemove(equipped, Equipment.Slots.Count(i => i.Supports(SelectedItem)));
            }

            if (SelectedItem.IsTwoHanded) AutoRemove(Equipment.Items.Where(i => i.IsShield).ToList());
            if (SelectedItem.IsShield) AutoRemove(Equipment.Items.Where(i => i.IsWeapon && i.IsTwoHanded).ToList());

            if (SelectedItem.IsFirearm) AutoRemove(Equipment.Items.Where(i => i.IsShield).ToList());
            if (SelectedItem.IsFirearm) AutoRemove(Equipment.Items.Where(i => i.IsWeapon && i.IsTwoHanded).ToList());
            if (SelectedItem.IsTwoHanded || SelectedItem.IsShield) AutoRemove(Equipment.Items.Where(i => i.IsWeapon && i.IsFirearm).ToList());

            MoveItem(SelectedItem, PlayerInventory, Equipment);
            AudioSource.PlayOneShot(EquipSound, SfxVolume);
            OnEquip?.Invoke(SelectedItem);
        }

        public void Remove()
        {
            MoveItem(SelectedItem, Equipment, PlayerInventory);
            SelectItem(SelectedItem);
            AudioSource.PlayOneShot(EquipSound, SfxVolume);
        }

        public void Craft()
        {
            var materials = MaterialList;

            if (CanCraft(materials))
            {
                materials.ForEach(i => PlayerInventory.Items.Single(j => j.Hash == i.Hash).Count -= i.Count);
                PlayerInventory.Items.RemoveAll(i => i.Count == 0);

                var itemId = SelectedItem.Params.FindProperty(PropertyId.Craft).Value;
                var existed = PlayerInventory.Items.SingleOrDefault(i => i.Id == itemId && i.Modifier == null);

                if (existed == null)
                {
                    PlayerInventory.Items.Add(new Item(itemId));
                }
                else
                {
                    existed.Count++;
                }

                PlayerInventory.Refresh(SelectedItem);
                CraftButton.interactable = CanCraft(materials);
                AudioSource.PlayOneShot(CraftSound, SfxVolume);
            }
            else
            {
                Debug.Log("No materials.");
            }
        }

        public void Learn()
        {
            // Implement your logic here!
        }

        public void Use()
        {
            Use(UseSound);
        }

        public void Use(AudioClip sound)
        {
            if (SelectedItem.Count == 1)
            {
                PlayerInventory.Items.Remove(SelectedItem);
                SelectedItem = PlayerInventory.Items.FirstOrDefault();

                if (SelectedItem == null)
                {
                    PlayerInventory.Refresh(null);
                    SelectedItem = Equipment.Items.FirstOrDefault();

                    if (SelectedItem != null)
                    {
                        Equipment.Refresh(SelectedItem);
                    }
                }
                else
                {
                    PlayerInventory.Refresh(SelectedItem);
                }
            }
            else
            {
                SelectedItem.Count--;
                PlayerInventory.Refresh(SelectedItem);
            }

            Equipment.OnRefresh?.Invoke();

            if (sound != null)
            {
                AudioSource.PlayOneShot(sound, SfxVolume);
            }
        }

        public Item Assemble()
        {
            if (SelectedItem != null && SelectedItem.Params.Type == ItemType.Fragment && SelectedItem.Count >= SelectedItem.Params.FindProperty(PropertyId.Fragments).ValueInt)
            {
                SelectedItem.Count -= SelectedItem.Params.FindProperty(PropertyId.Fragments).ValueInt;

                var crafted = new Item(SelectedItem.Params.FindProperty(PropertyId.Craft).Value);
                var existed = PlayerInventory.Items.SingleOrDefault(i => i.Hash == crafted.Hash);

                if (existed == null)
                {
                    PlayerInventory.Items.Add(crafted);
                }
                else
                {
                    existed.Count++;
                }

                if (SelectedItem.Count == 0)
                {
                    PlayerInventory.Items.Remove(SelectedItem);
                    SelectedItem = crafted;
                }

                PlayerInventory.Refresh(SelectedItem);

                return crafted;
            }

            return null;
        }

        public override void Refresh()
        {
            if (SelectedItem == null)
            {
                ItemInfo.Reset();
                EquipButton.SetActive(false);
                RemoveButton.SetActive(false);
            }
            else
            {
                var equipped = Equipment.Items.Contains(SelectedItem);

                EquipButton.SetActive(!equipped && CanEquipSelectedItem());
                RemoveButton.SetActive(equipped);
            }

            UseButton.SetActive(SelectedItem != null && CanUse());
            AssembleButton.SetActive(SelectedItem != null && SelectedItem.Params.Type == ItemType.Fragment && SelectedItem.Count >= SelectedItem.Params.FindProperty(PropertyId.Fragments).ValueInt);

            var receipt = SelectedItem != null && SelectedItem.Params.Type == ItemType.Recipe;

            if (CraftButton != null) CraftButton.SetActive(false);
            if (LearnButton != null) LearnButton.SetActive(false);

            if (receipt)
            {
                if (LearnButton == null)
                {
                    var materialSelected = !PlayerInventory.Items.Contains(SelectedItem) && !Equipment.Items.Contains(SelectedItem);

                    CraftButton.SetActive(true);
                    Materials.SetActive(materialSelected);
                    Equipment.Scheme.SetActive(!materialSelected);

                    var materials = MaterialList;

                    Materials.Initialize(ref materials);
                }
                else
                {
                    LearnButton.SetActive(true);
                }
            }

            OnRefresh?.Invoke(SelectedItem);
        }

        private List<Item> MaterialList => SelectedItem.Params.FindProperty(PropertyId.Materials).Value.Split(',').Select(i => i.Split(':')).Select(i => new Item(i[0], int.Parse(i[1]))).ToList();

        private bool CanEquipSelectedItem()
        {
            return PlayerInventory.Items.Contains(SelectedItem) && Equipment.Slots.Any(i => i.Supports(SelectedItem));
        }

        private bool CanUse()
        {
            switch (SelectedItem.Params.Type)
            {
                case ItemType.Container:
                case ItemType.Booster:
                case ItemType.Coupon:
                    return true;
                default:
                    return false;
            }
        }

        private bool CanCraft(List<Item> materials)
        {
            return materials.All(i => PlayerInventory.Items.Any(j => j.Hash == i.Hash && j.Count >= i.Count));
        }

        /// <summary>
        /// Automatically removes items if target slot is busy.
        /// </summary>
        private void AutoRemove(List<Item> items, int max = 1)
        {
            long sum = 0;

            foreach (var p in items)
            {
                sum += p.Count;
            }

            if (sum == max)
            {
                MoveItemSilent(items.LastOrDefault(i => i.Id != SelectedItem.Id) ?? items.Last(), Equipment, PlayerInventory);
            }
        }
    }
}