using System;
using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor4D.Common.Scripts.Collections;
using Assets.HeroEditor4D.Common.Scripts.Common;
using Assets.HeroEditor4D.Common.Scripts.Data;
using Assets.HeroEditor4D.Common.Scripts.Enums;
using Assets.HeroEditor4D.InventorySystem.Scripts;
using Assets.HeroEditor4D.InventorySystem.Scripts.Data;
using Assets.HeroEditor4D.InventorySystem.Scripts.Enums;
using UnityEngine;

namespace Assets.HeroEditor4D.Common.Scripts.CharacterScripts
{
	/// <summary>
	/// Controls 4 characters (for each direction).
	/// </summary>
	public class Character4D : MonoBehaviour
    {
        [Header("Parts")]
        public Character Front;
        public Character Back;
        public Character Left;
        public Character Right;
        public List<Character> Parts;
        public List<GameObject> Shadows;

        [Header("Animation")]
        public Animator Animator;
        public AnimationManager AnimationManager;

        [Header("Other")]
        public LayerManager LayerManager;
        public Color BodyColor;
        
        public SpriteCollection SpriteCollection => Parts[0].SpriteCollection;
        private List<Character> PartsExceptBack => new List<Character> { Front, Left, Right };

        public List<Sprite> Body { set { Parts.ForEach(i => i.Body = value.ToList()); } }
        public List<Sprite> Head { set { Parts.ForEach(i => i.Head = i.HairRenderer.GetComponent<SpriteMapping>().FindSprite(value)); } }
        public List<Sprite> Hair { set { Parts.ForEach(i => i.Hair = i.HairRenderer.GetComponent<SpriteMapping>().FindSprite(value)); } }
        public List<Sprite> Beard { set { Parts.ForEach(i => { if (i.BeardRenderer) i.Beard = i.BeardRenderer.GetComponent<SpriteMapping>().FindSprite(value); }); } }
        public List<Sprite> Ears { set { Parts.ForEach(i => i.Ears = value.ToList()); } }
        public List<Sprite> Eyebrows { set { PartsExceptBack.ForEach(i => i.Expressions[0].Eyebrows = i.EyebrowsRenderer.GetComponent<SpriteMapping>().FindSprite(value)); } }
        public List<Sprite> Eyes { set { PartsExceptBack.ForEach(i => i.Expressions[0].Eyes = i.EyesRenderer.GetComponent<SpriteMapping>().FindSprite(value)); } }
        public List<Sprite> Mouth { set { PartsExceptBack.ForEach(i => i.Expressions[0].Mouth = i.MouthRenderer.GetComponent<SpriteMapping>().FindSprite(value)); } }
        public List<Sprite> Helmet { set { Parts.ForEach(i => i.Helmet = i.HelmetRenderer.GetComponent<SpriteMapping>().FindSprite(value)); } }
        public List<Sprite> Armor { set { Parts.ForEach(i => i.Armor = value.ToList()); } }
        public Sprite PrimaryWeapon { set { Parts.ForEach(i => i.PrimaryWeapon = value); } }
        public Sprite SecondaryWeapon { set { Parts.ForEach(i => i.SecondaryWeapon = value); } }
        public List<Sprite> Shield { set { Parts.ForEach(i => i.Shield = value.ToList()); } }
        public List<Sprite> CompositeWeapon { set { Parts.ForEach(i => i.CompositeWeapon = value.ToList()); } }
        public List<Sprite> Makeup { set { Parts.ForEach(i => { if (i.MakeupRenderer) i.Makeup = i.MakeupRenderer.GetComponent<SpriteMapping>().FindSprite(value); }); } }
        public List<Sprite> Mask { set { Parts.ForEach(i => { if (i.MaskRenderer) i.Mask = i.MaskRenderer.GetComponent<SpriteMapping>().FindSprite(value); }); } }
        public List<Sprite> Earrings { set { Parts.ForEach(i => i.Earrings = value.ToList()); } }
        public WeaponType WeaponType { get => Front.WeaponType; set { Parts.ForEach(i => i.WeaponType = value); } }

        public void OnValidate()
        {
            Parts = new List<Character> { Front, Back, Left, Right };
            Parts.ForEach(i => i.BodyRenderers.ForEach(j => j.color = BodyColor));
            Parts.ForEach(i => i.EarsRenderers.ForEach(j => j.color = BodyColor));
        }

        public void Start()
        {
            var stateHandler = Animator.GetBehaviours<StateHandler>().SingleOrDefault(i => i.Name == "Death");

            if (stateHandler != null)
            {
                stateHandler.StateExit.AddListener(() => SetExpression("Default"));
            }

            Animator.keepAnimatorStateOnDisable = true;
        }

        public void Initialize()
        {
            Parts.ForEach(i => i.Initialize());
        }

        public void SetBody(ItemSprite item, BodyPart part)
        {
            Parts.ForEach(i => i.SetBody(item, part));
        }

        public void SetBody(ItemSprite item, BodyPart part, Color? color)
        {
            Parts.ForEach(i => i.SetBody(item, part, color));
        }

        public void SetExpression(string expression)
        {
            Parts.ForEach(i => i.SetExpression(expression));
        }

        public void Equip(ItemSprite item, EquipmentPart part)
        {
            Parts.ForEach(i => i.Equip(item, part));
            UpdateWeaponType(part);
        }

        public void Equip(ItemSprite item, EquipmentPart part, Color? color)
        {
            Parts.ForEach(i => i.Equip(item, part, color));
            UpdateWeaponType(part);
        }

        private void UpdateWeaponType(EquipmentPart part)
        {
            switch (part)
            {
                case EquipmentPart.MeleeWeapon1H: Animator.SetInteger("WeaponType", (int)WeaponType.Melee1H); break;
                case EquipmentPart.MeleeWeapon2H: Animator.SetInteger("WeaponType", (int)WeaponType.Melee2H); break;
                case EquipmentPart.Bow: Animator.SetInteger("WeaponType", (int)WeaponType.Bow); break;
                case EquipmentPart.Crossbow: Animator.SetInteger("WeaponType", (int)WeaponType.Crossbow); break;
                case EquipmentPart.Firearm1H: Animator.SetInteger("WeaponType", (int)WeaponType.Firearm1H); break;
                case EquipmentPart.Firearm2H: Animator.SetInteger("WeaponType", (int)WeaponType.Firearm2H); break;
                case EquipmentPart.SecondaryFirearm1H: Animator.SetInteger("WeaponType", (int)WeaponType.Paired); break;
            }
        }

        public void UnEquip(EquipmentPart part)
        {
            Parts.ForEach(i => i.UnEquip(part));
        }

        public void ResetEquipment()
        {
            Parts.ForEach(i => i.ResetEquipment());
            Animator.SetInteger("WeaponType", (int)WeaponType.Melee1H);
        }

        public Vector2 Direction { get; private set; }
        public Character Active { get; private set; }

        public void SetDirection(Vector2 direction)
		{
            if (Direction == direction) return;

			Direction = direction;

            if (Direction == Vector2.zero)
            {
                Parts.ForEach(i => i.SetActive(true));
                Shadows.ForEach(i => i.SetActive(true));

                Parts[0].transform.localPosition = Shadows[0].transform.localPosition = new Vector3(0, -1.25f);
                Parts[1].transform.localPosition = Shadows[1].transform.localPosition = new Vector3(0, 1.25f);
                Parts[2].transform.localPosition = Shadows[2].transform.localPosition = new Vector3(-1.5f, 0);
                Parts[3].transform.localPosition = Shadows[3].transform.localPosition = new Vector3(1.5f, 0);

                return;
            }

			Parts.ForEach(i => i.transform.localPosition = Vector3.zero);
			Shadows.ForEach(i => i.transform.localPosition = Vector3.zero);

			int index;

			if (direction == Vector2.left)
			{
				index = 2;
			}
			else if (direction == Vector2.right)
			{
				index = 3;
			}
			else if (direction == Vector2.up)
			{
				index = 1;
			}
			else if (direction == Vector2.down)
			{
				index = 0;
			}
            else
			{
				throw new NotSupportedException();
			}

			for (var i = 0; i < Parts.Count; i++)
			{
                Parts[i].SetActive(i == index);
				Shadows[i].SetActive(i == index);
			}

            Active = Parts[index];
        }

        public void CopyFrom(Character4D character)
        {
            for (var i = 0; i < Parts.Count; i++)
            {
                Parts[i].CopyFrom(character.Parts[i]);
                Parts[i].WeaponType = character.Parts[i].WeaponType;
                Parts[i].HideEars = character.Parts[i].HideEars;
                Parts[i].CropHair = character.Parts[i].CropHair;
            }
        }

        public string ToJson()
        {
            return Front.ToJson();
        }

        public void FromJson(string json, bool silent)
        {
            Parts.ForEach(i => i.LoadFromJson(json, silent));
        }

        #region Setup Examples

        public void Equip(Item item)
        {
            var itemParams = ItemCollection.Active.GetItemParams(item);

            switch (itemParams.Type)
            {
                case ItemType.Helmet: EquipHelmet(item); break;
                case ItemType.Armor: EquipArmor(item); break;
                case ItemType.Vest: EquipVest(item); break;
                case ItemType.Bracers: EquipBracers(item); break;
                case ItemType.Leggings: EquipLeggings(item); break;
                case ItemType.Shield: EquipShield(item); break;
                case ItemType.Weapon:
                {
                    switch (itemParams.Class)
                    {
                            case ItemClass.Bow: EquipBow(item); break;
                            case ItemClass.Firearm: EquipSecondaryFirearm(item); break;
                            default:
                                if (itemParams.Tags.Contains(ItemTag.TwoHanded))
                                {
                                    EquipMeleeWeapon2H(item);
                                }
                                else
                                {
                                    EquipMeleeWeapon1H(item);
                                }
                                break;
                    }
                    break;
                }
            }
        }

        public void EquipArmor(Item item)
        {
            if (item == null) UnEquip(EquipmentPart.Armor);
            else Equip(SpriteCollection.Armor.Single(i => i.Id == item.Params.SpriteId), EquipmentPart.Armor);
        }

        public void EquipHelmet(Item item)
        {
            if (item == null) UnEquip(EquipmentPart.Helmet);
            else Equip(SpriteCollection.Armor.Single(i => i.Id == item.Params.SpriteId), EquipmentPart.Helmet);
        }

        public void EquipVest(Item item)
        {
            if (item == null) UnEquip(EquipmentPart.Vest);
            else Equip(SpriteCollection.Armor.Single(i => i.Id == item.Params.SpriteId), EquipmentPart.Vest);
        }

        public void EquipBracers(Item item)
        {
            if (item == null) UnEquip(EquipmentPart.Bracers);
            else Equip(SpriteCollection.Armor.Single(i => i.Id == item.Params.SpriteId), EquipmentPart.Bracers);
        }

        public void EquipLeggings(Item item)
        {
            if (item == null) UnEquip(EquipmentPart.Leggings);
            else Equip(SpriteCollection.Armor.Single(i => i.Id == item.Params.SpriteId), EquipmentPart.Leggings);
        }

        public void EquipShield(Item item)
        {
            Equip(SpriteCollection.Shield.SingleOrDefault(i => i.Id == item.Params.SpriteId), EquipmentPart.Shield);
        }
        
        public void EquipMeleeWeapon1H(Item item)
        {
            Equip(SpriteCollection.MeleeWeapon1H.SingleOrDefault(i => i.Id == item.Params.SpriteId), EquipmentPart.MeleeWeapon1H);
        }

        public void EquipMeleeWeapon2H(Item item)
        {
            Equip(SpriteCollection.MeleeWeapon2H.SingleOrDefault(i => i.Id == item.Params.SpriteId), EquipmentPart.MeleeWeapon2H);
        }

        public void EquipBow(Item item)
        {
            Equip(SpriteCollection.Bow.SingleOrDefault(i => i.Id == item.Params.SpriteId), EquipmentPart.Bow);
        }

        public void EquipCrossbow(Item item)
        {
            Equip(SpriteCollection.Crossbow.SingleOrDefault(i => i.Id == item.Params.SpriteId), EquipmentPart.Crossbow);
        }

        public void EquipSecondaryFirearm(Item item)
        {
            Equip(SpriteCollection.Firearm1H.SingleOrDefault(i => i.Id == item.Params.SpriteId), EquipmentPart.SecondaryFirearm1H);
        }

        #endregion
    }
}