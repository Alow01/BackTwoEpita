using Assets.HeroEditor4D.Common.Scripts.Common;
using Assets.HeroEditor4D.Common.Scripts.Enums;
using UnityEngine;

namespace Assets.HeroEditor4D.Common.Scripts.CharacterScripts
{
    /// <summary>
    /// You can extend 'Character' class here.
    /// </summary>
    public static class CharacterExtensions
    {
        public static Color RandomColor => new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f), 1f);

        public static void Randomize(this Character4D character)
        {
            character.ResetEquipment();
            character.SetBody(character.Front.SpriteCollection.Eyes.Random(), BodyPart.Eyes);

            if (character.Front.SpriteCollection.Hair.Count > 0) character.SetBody(character.Front.SpriteCollection.Hair.Random(), BodyPart.Hair, RandomColor);
            if (character.Front.SpriteCollection.Eyebrows.Count > 0) character.SetBody(character.Front.SpriteCollection.Eyebrows.Random(), BodyPart.Eyebrows);
            if (character.Front.SpriteCollection.Eyes.Count > 0) character.SetBody(character.Front.SpriteCollection.Eyes.Random(), BodyPart.Eyes, RandomColor);
            if (character.Front.SpriteCollection.Ears.Count > 0) character.SetBody(character.Front.SpriteCollection.Ears.Random(), BodyPart.Ears);
            if (character.Front.SpriteCollection.Mouth.Count > 0) character.SetBody(character.Front.SpriteCollection.Mouth.Random(), BodyPart.Mouth);

            character.Equip(character.Front.SpriteCollection.Armor.Random(), EquipmentPart.Helmet);
            character.Equip(character.Front.SpriteCollection.Armor.Random(), EquipmentPart.Armor);

            switch (Random.Range(0, 5))
            {
                case 0:
                    character.Equip(character.Front.SpriteCollection.MeleeWeapon1H.Random(), EquipmentPart.MeleeWeapon1H);
                    character.UnEquip(EquipmentPart.Shield);
                    break;
                case 1:
                    character.Equip(character.Front.SpriteCollection.MeleeWeapon1H.Random(), EquipmentPart.MeleeWeapon1H);
                    character.Equip(character.Front.SpriteCollection.Shield.Random(), EquipmentPart.Shield);
                    break;
                case 2:
                    character.Equip(character.Front.SpriteCollection.MeleeWeapon2H.Random(), EquipmentPart.MeleeWeapon2H);
                    break;
                case 3:
                    character.Equip(character.Front.SpriteCollection.Bow.Random(), EquipmentPart.Bow);
                    break;
                case 4:
                    character.Equip(character.Front.SpriteCollection.Firearm1H.Random(), EquipmentPart.SecondaryFirearm1H);
                    break;
            }
        }
    }
}