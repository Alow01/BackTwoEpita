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
    public class WarehouseBase : ItemWorkspace
    {
        public ScrollInventory WarehouseInventory;
        public ScrollInventory PlayerInventory;
        public InputField AmountInput;
        public Button PutButton;
        public Button TakeButton;
        public AudioClip MoveSound;
        public AudioSource AudioSource;
        public bool ExampleInitialize;

        public string CurrencyId = "Gold";
        public int Amount;
        
        public void Awake()
        {
            // You must to set an active collection (as there may be several different collections in Resources).
            ItemCollection.Active = ItemCollection;
        }

        public void Start()
        {
            if (ExampleInitialize)
            {
                TestInitialize();
            }
        }

        /// <summary>
        /// Initialize owned items and trader items (just for example).
        /// </summary>
        public void TestInitialize()
        {
            var warehouse = new List<Item>();
            var inventory = ItemCollection.Active.Items.Select(i => new Item(i.Id, 2)).ToList();

            RegisterCallbacks();
            WarehouseInventory.Initialize(ref warehouse);
            PlayerInventory.Initialize(ref inventory);

            if (!WarehouseInventory.SelectAny() && !PlayerInventory.SelectAny())
            {
                ItemInfo.Reset();
            }
        }

        public void Initialize(ref List<Item> playerItems, ref  List<Item> storageItems)
        {
            RegisterCallbacks();
            PlayerInventory.Initialize(ref playerItems);
            WarehouseInventory.Initialize(ref storageItems);

            if (!PlayerInventory.SelectAny() && !WarehouseInventory.SelectAny())
            {
                ItemInfo.Reset();
            }
        }

        public void RegisterCallbacks()
        {
            InventoryItem.OnLeftClick = SelectItem;
            InventoryItem.OnRightClick = OnDoubleClick;
        }

        private void OnDoubleClick(Item item)
        {
            SelectItem(item);

            if (PlayerInventory.Items.Contains(item))
            {
                Take();
            }
            else if (CanMoveSelectedItem())
            {
                Put();
            }
        }

        public void SelectItem(Item item)
        {
            SelectedItem = item;
            SetAmount(1);
            ItemInfo.Initialize(SelectedItem, SelectedItem.Params.Price * Amount);
            Refresh();
        }

        public void Put()
        {
            if (!CanMoveSelectedItem()) return;

            MoveItem(SelectedItem, PlayerInventory, WarehouseInventory, Amount);
            SelectItem(SelectedItem);
            AudioSource.PlayOneShot(MoveSound, SfxVolume);
        }

        public void Take()
        {
            if (!CanMoveSelectedItem()) return;

            MoveItem(SelectedItem, WarehouseInventory, PlayerInventory, Amount);
            SelectItem(SelectedItem);
            AudioSource.PlayOneShot(MoveSound, SfxVolume);
        }

        public override void Refresh()
        {
            if (SelectedItem == null)
            {
                ItemInfo.Reset();
                PutButton.SetActive(false);
                TakeButton.SetActive(false);
            }
            else
            {
                var stored = WarehouseInventory.Items.Contains(SelectedItem);

                PutButton.SetActive(!stored && CanMoveSelectedItem());
                TakeButton.SetActive(stored && CanMoveSelectedItem());
            }
        }

        public void SetMinAmount()
        {
            SetAmount(1);
        }

        public void IncAmount(int value)
        {
            SetAmount(Amount + value);
        }

        public void SetMaxAmount()
        {
            SetAmount(SelectedItem.Count);
        }

        public void OnAmountChanged(string value)
        {
            if (value.IsEmpty()) return;

            SetAmount(int.Parse(value));
        }

        public void OnAmountEndEdit(string value)
        {
            if (value.IsEmpty())
            {
                SetAmount(1);
            }
        }

        public void Drop()
        {
            foreach (var item in PlayerInventory.Items.ToList())
            {
                if (item.Params.Type != ItemType.Currency && !item.Params.Tags.Contains(ItemTag.Quest))
                {
                    #if TAP_HEROES

                    if (item.Params.Class == ItemClass.Gunpowder) continue;

                    #endif

                    MoveItem(item, PlayerInventory, WarehouseInventory, item.Count);
                }
            }

            AudioSource.PlayOneShot(MoveSound, SfxVolume);
        }

        private void SetAmount(int amount)
        {
            Amount = Mathf.Max(1, Mathf.Min(SelectedItem.Count, amount));
            AmountInput?.SetTextWithoutNotify(Amount.ToString());
            ItemInfo.UpdatePrice(SelectedItem, SelectedItem.Params.Price * Amount, false);
        }

        protected virtual bool CanMoveSelectedItem()
        {
            return true;
        }
    }
}