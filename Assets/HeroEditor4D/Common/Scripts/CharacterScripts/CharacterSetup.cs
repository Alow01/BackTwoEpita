using System;
using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor4D.Common.Scripts.Data;
using Assets.HeroEditor4D.Common.Scripts.Enums;
using UnityEngine;

namespace Assets.HeroEditor4D.Common.Scripts.CharacterScripts
{
    public partial class Character
    {
        /// <summary>
        /// Set character's body parts.
        /// </summary>
		public void SetBody(ItemSprite item, BodyPart part, Color? color)
        {
            switch (part)
            {
                case BodyPart.Body:
                    Body = item?.Sprites.ToList();
                    BodyRenderers.ForEach(i => i.color = color ?? i.color);
                    Head = HeadRenderer.GetComponent<SpriteMapping>().FindSprite(item?.Sprites);
                    HeadRenderer.color = color ?? HeadRenderer.color;
                    break;
                case BodyPart.Head:
                    Head = HeadRenderer.GetComponent<SpriteMapping>().FindSprite(item?.Sprites);
                    HeadRenderer.color = color ?? HeadRenderer.color;
                    break;
                case BodyPart.Hair:
                    Hair = HairRenderer.GetComponent<SpriteMapping>().FindSprite(item?.Sprites);
                    HairRenderer.color = color ?? HairRenderer.color;
                    HideEars = item != null && item.Tags.Contains("HideEars");
                    if (item != null && item.Tags.Contains("NoPaint")) { HairRenderer.color = Color.white; HairRenderer.material = new Material(Shader.Find("Sprites/Default")); }
                    break;
                case BodyPart.Ears:
                    Ears = item?.Sprites.ToList();
                    EarsRenderers.ForEach(i => i.color = color ?? i.color);
                    break;
                case BodyPart.Eyebrows:
                    if (EyebrowsRenderer) Expressions[0].Eyebrows = EyebrowsRenderer.GetComponent<SpriteMapping>().FindSprite(item?.Sprites);
                    break;
                case BodyPart.Eyes:
                    if (EyesRenderer)
                    {
                        Expressions[0].Eyes = EyesRenderer.GetComponent<SpriteMapping>().FindSprite(item?.Sprites);
                        Expressions.Where(i => i.Name != "Dead").ToList().ForEach(i => i.EyesColor = color ?? EyesRenderer.color);
                        EyesRenderer.color = color ?? EyesRenderer.color;
                    }
                    break;
                case BodyPart.Mouth:
                    if (MouthRenderer) Expressions[0].Mouth = MouthRenderer.GetComponent<SpriteMapping>().FindSprite(item?.Sprites);
                    break;
                case BodyPart.Beard:
                    if (BeardRenderer)
                    {
                        Beard = BeardRenderer.GetComponent<SpriteMapping>().FindSprite(item?.Sprites);
                        BeardRenderer.color = color ?? BeardRenderer.color;
                    }
                    break;
                case BodyPart.Makeup:
                    if (MakeupRenderer)
                    {
                        Makeup = MakeupRenderer.GetComponent<SpriteMapping>().FindSprite(item?.Sprites);
                        MakeupRenderer.color = color ?? MakeupRenderer.color;
                    }
                    break;
                default: throw new NotImplementedException($"Unsupported part: {part}.");
            }

            Initialize();
        }

        public void SetBody(ItemSprite item, BodyPart part)
        {
            SetBody(item, part, null);
        }

        /// <summary>
        /// Set character's expression.
        /// </summary>
        public void SetExpression(string expression)
        {
            if (Expressions.Count < 3) throw new Exception("Character must have at least 3 basic expressions: Default, Angry and Dead.");
            if (EyesRenderer == null) return;

            var e = Expressions.Single(i => i.Name == expression);

            Expression = expression;
            EyebrowsRenderer.sprite = e.Eyebrows;
            EyesRenderer.sprite = e.Eyes;
            EyesRenderer.color = e.EyesColor;
            MouthRenderer.sprite = e.Mouth;
        }

        /// <summary>
        /// Equip something from SpriteCollection.
        /// </summary>
        public void Equip(ItemSprite item, EquipmentPart part, Color? color)
        {
            switch (part)
            {
                case EquipmentPart.MeleeWeapon1H:
                    CompositeWeapon = null;
                    break;
                case EquipmentPart.MeleeWeapon2H:
                    CompositeWeapon = null;
                    Shield = null;
                    break;
                case EquipmentPart.Bow:
                    PrimaryWeapon = SecondaryWeapon = null;
                    Shield = null;
                    break;
                case EquipmentPart.Crossbow:
                    PrimaryWeapon = SecondaryWeapon = null;
                    Shield = null;
                    break;
                case EquipmentPart.SecondaryFirearm1H:
                    CompositeWeapon = null;
                    Shield = null;
                    break;
                case EquipmentPart.Firearm1H:
                    CompositeWeapon = null;
                    Shield = null;
                    break;
                case EquipmentPart.Firearm2H:
                    CompositeWeapon = null;
                    Shield = null;
                    break;
            }

            Backpack = null;

            switch (part)
            {
                case EquipmentPart.Helmet:
                    HideEars = item != null && !item.Tags.Contains("ShowEars");
                    CropHair = item != null && !item.Tags.Contains("FullHair");
                    Helmet = HelmetRenderer.GetComponent<SpriteMapping>().FindSprite(item?.Sprites);
                    HelmetRenderer.color = color ?? HelmetRenderer.color;
                    break;
                case EquipmentPart.Armor:
                    Armor = item?.Sprites.ToList().ToList();
                    ArmorRenderers.ForEach(i => i.color = color ?? i.color);
                    break;
                case EquipmentPart.Vest:
                    SetArmorParts(VestRenderers, item?.Sprites, color);
                    break;
                case EquipmentPart.Bracers:
                    SetArmorParts(BracersRenderers, item?.Sprites, color);
                    break;
                case EquipmentPart.Leggings:
                    SetArmorParts(LeggingsRenderers, item?.Sprites, color);
                    break;
                case EquipmentPart.MeleeWeapon1H:
                    PrimaryWeapon = item?.Sprite;
                    PrimaryWeaponRenderer.color = color ?? (item != null && item.Tags.Contains("Paint") ? PrimaryWeaponRenderer.color : Color.white);
                    if (WeaponType != WeaponType.Paired) WeaponType = WeaponType.Melee1H;
                    break;
                case EquipmentPart.MeleeWeapon2H:
                    PrimaryWeapon = item?.Sprite;
                    PrimaryWeaponRenderer.color = color ?? (item != null && item.Tags.Contains("Paint") ? PrimaryWeaponRenderer.color : Color.white);
                    WeaponType = WeaponType.Melee2H;
                    break;
                case EquipmentPart.Bow:
                    CompositeWeapon = item?.Sprites.ToList();
                    WeaponType = WeaponType.Bow;
                    break;
                case EquipmentPart.Crossbow:
                    Backpack = item?.Sprites.Single(i => i.name == "Quiver");
                    PrimaryWeapon = item?.Sprites.Single(i => i.name == "Gun");
                    CompositeWeapon = item?.Sprites;
                    WeaponType = WeaponType.Crossbow;
                    break;
                case EquipmentPart.Firearm1H:
                    PrimaryWeapon = item?.Sprites.Single(i => i.name == "Gun");
                    WeaponType = WeaponType.Firearm1H;
                    break;
                case EquipmentPart.Firearm2H:
                    PrimaryWeapon = item?.Sprites.Single(i => i.name == "Gun");
                    WeaponType = WeaponType.Firearm2H;
                    break;
                case EquipmentPart.SecondaryFirearm1H:
                    SecondaryWeapon = SecondaryWeaponRenderer.GetComponent<SpriteMapping>().FindSprite(item?.Sprites);
                    WeaponType = WeaponType.Paired;
                    break;
                case EquipmentPart.Shield:
                    Shield = item?.Sprites.ToList();
                    WeaponType = WeaponType.Melee1H;
                    break;
                case EquipmentPart.Cape:
                    Cape = item?.Sprite;
                    CapeRenderer.color = color ?? CapeRenderer.color;
                    break;
                case EquipmentPart.Backpack:
                    Backpack = item?.Sprite;
                    BackpackRenderer.color = color ?? BackpackRenderer.color;
                    break;
                case EquipmentPart.Earrings:
                    Earrings = item?.Sprites.ToList();
                    EarringsRenderers[0].color = EarringsRenderers[1].color = color ?? EarringsRenderers[0].color;
                    break;
                case EquipmentPart.Mask:
                    Mask = MaskRenderer.GetComponent<SpriteMapping>().FindSprite(item?.Sprites);
                    MaskRenderer.color = color ?? MaskRenderer.color;
                    break;
                default: throw new NotImplementedException($"Unsupported part: {part}.");
            }

            Initialize();

            if (part == EquipmentPart.Firearm1H || part == EquipmentPart.Firearm2H)
            {
                SetFirearmMuzzle(item);
            }
        }

        public void Equip(ItemSprite item, EquipmentPart part)
        {
            Equip(item, part, null);
        }

        /// <summary>
        /// Remove equipment partially.
        /// </summary>
        public void UnEquip(EquipmentPart part)
        {
            Equip(null, part);
        }

        /// <summary>
        /// Remove all equipment.
        /// </summary>
        public void ResetEquipment()
        {
            Armor = new List<Sprite>();
            CompositeWeapon = new List<Sprite>();
            Shield = new List<Sprite>();
            Helmet = PrimaryWeapon = SecondaryWeapon = Mask = Cape = Backpack = null;
            Earrings = new List<Sprite>();
            HideEars = CropHair = false;
            Initialize();
        }

        private void SetArmorParts(List<SpriteRenderer> renderers, List<Sprite> armor, Color? color)
        {
            if (Armor == null)
            {
                Armor = new List<Sprite>();
            }
            else
            {
                Armor.RemoveAll(i => i == null);
            }

            foreach (var r in renderers)
            {
                var mapping = r.GetComponent<SpriteMapping>();
                var sprite = armor?.SingleOrDefault(j => mapping.SpriteName == j.name || mapping.SpriteNameFallback.Contains(j.name));

                Armor.RemoveAll(i => i.name == mapping.SpriteName || mapping.SpriteNameFallback.Contains(i.name));

                if (sprite != null)
                {
                    Armor.Add(sprite);
                }

                if (color != null) r.color = color.Value;
            }            
        }

        private void SetFirearmMuzzle(ItemSprite item)
        {
            if (item == null || PrimaryWeaponRenderer.sprite == null)
            {
                AnchorFireMuzzle.localPosition = Vector3.zero;
                return;
            }

            var mx = 0.5f;
            var my = int.Parse(item.MetaDict["Muzzle"]) / 100f;
            var bounds = PrimaryWeaponRenderer.sprite.bounds;
            var p = new Vector2(bounds.min.x + mx * (bounds.max.x - bounds.min.x), bounds.min.y + my * (bounds.max.y - bounds.min.y));

            if (PrimaryWeaponRenderer.flipX) p.x *= -1;

            AnchorFireMuzzle.localPosition = p;
        }
    }
}