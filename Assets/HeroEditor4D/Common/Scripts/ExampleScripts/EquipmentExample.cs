using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Assets.HeroEditor4D.Common.Scripts.Enums;
using UnityEngine;

namespace Assets.HeroEditor4D.Common.Scripts.ExampleScripts
{
    /// <summary>
    /// An example of how to change character's equipment.
    /// </summary>
    public class EquipmentExample : MonoBehaviour
    {
        public Character4D Character;
        public AppearanceExample AppearanceExample;

        public void EquipRandomArmor()
        {
            var randomIndex = Random.Range(0, Character.SpriteCollection.Armor.Count);
            var randomItem = Character.SpriteCollection.Armor[randomIndex];

            Character.Equip(randomItem, EquipmentPart.Armor); AppearanceExample.Refresh();
        }

        public void RemoveArmor()
        {
            Character.UnEquip(EquipmentPart.Armor);
            AppearanceExample.Refresh();
        }

        public void EquipRandomHelmet()
        {
            var randomIndex = Random.Range(0, Character.SpriteCollection.Armor.Count);
            var randomItem = Character.SpriteCollection.Armor[randomIndex];

            Character.Equip(randomItem, EquipmentPart.Helmet);
            AppearanceExample.Refresh();
        }

        public void RemoveHelmet()
        {
            Character.UnEquip(EquipmentPart.Helmet);
            AppearanceExample.Refresh();
        }

        public void EquipRandomShield()
        {
            var randomIndex = Random.Range(0, Character.SpriteCollection.Shield.Count);
            var randomItem = Character.SpriteCollection.Shield[randomIndex];

            Character.Equip(randomItem, EquipmentPart.Shield);
        }

        public void RemoveShield()
        {
            Character.UnEquip(EquipmentPart.Shield);
        }

        public void EquipRandomWeapon()
        {
            var randomIndex = Random.Range(0, Character.SpriteCollection.MeleeWeapon1H.Count);
            var randomItem = Character.SpriteCollection.MeleeWeapon1H[randomIndex];

            Character.Equip(randomItem, EquipmentPart.MeleeWeapon1H);
        }

        public void RemoveWeapon()
        {
            Character.UnEquip(EquipmentPart.MeleeWeapon1H);
        }
    }
}