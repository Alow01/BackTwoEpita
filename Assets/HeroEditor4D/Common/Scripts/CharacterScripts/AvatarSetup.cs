using System;
using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor4D.Common.Scripts.Collections;
using Assets.HeroEditor4D.Common.Scripts.Common;
using Assets.HeroEditor4D.Common.Scripts.Data;
using UnityEngine;

namespace Assets.HeroEditor4D.Common.Scripts.CharacterScripts
{
    public class AvatarSetup : MonoBehaviour
    {
        public List<SpriteCollection> SpriteCollections;
        public SpriteRenderer Head;
        public SpriteRenderer Hair;
        public List<SpriteRenderer> Ears;
        public SpriteRenderer Eyes;
        public SpriteRenderer Eyebrows;
        public SpriteRenderer Mouth;
        public SpriteRenderer Beard;
        public SpriteRenderer Helmet;

        public void Initialize(CharacterAppearance appearance, string helmetId)
        {
            if (SpriteCollections.Count == 0) throw new Exception("Please set sprite collections for avatar setup.");

            var ears = SpriteCollections.SelectMany(i => i.Ears).Single(i => i.Id == appearance.Ears).Sprites[1];

            Head.sprite = SpriteCollections.SelectMany(i => i.Body).Single(i => i.Id == appearance.Body).Sprites.Single(i => i.name == "FrontHead");
            Head.color = Ears[0].color = Ears[1].color = appearance.BodyColor;

            ItemSprite hair = null;

            if (appearance.Hair.IsEmpty())
            {
                Hair.enabled = false;
            }
            else
            {
                hair = SpriteCollections.SelectMany(i => i.Hair).Single(i => i.Id == appearance.Hair);
                Hair.enabled = true;
                Hair.sprite = hair.Sprites[1];
                Hair.color = hair.Tags.Contains("NoPaint") ? (Color32) Color.white : appearance.HairColor;
            }

            Beard.sprite = appearance.Beard.IsEmpty() ? null : SpriteCollections.SelectMany(i => i.Beard).Single(i => i.Id == appearance.Beard).Sprite;
            Beard.color = appearance.BeardColor;
            Eyes.sprite = SpriteCollections.SelectMany(i => i.Eyes).Single(i => i.Id == appearance.Eyes).Sprite;
            Eyes.color = appearance.EyesColor;

            if (appearance.Eyebrows.IsEmpty())
            {
                Eyebrows.enabled = false;
            }
            else
            {
                Eyebrows.enabled = true;
                Eyebrows.sprite = SpriteCollections.SelectMany(i => i.Eyebrows).Single(i => i.Id == appearance.Eyebrows).Sprite;
            }

            Mouth.sprite = SpriteCollections.SelectMany(i => i.Mouth).Single(i => i.Id == appearance.Mouth).Sprite;
            Mouth.transform.localPosition = new Vector3(0, appearance.Type == 0 ?  -0.1f : 0.25f);

            if (helmetId == null)
            {
                var hideEars = hair != null && hair.Tags.Contains("HideEars");

                Helmet.enabled = false;
                Ears.ForEach(j => { j.sprite = ears; j.enabled = !hideEars; });
            }
            else
            {
                Helmet.enabled = true;

                var helmet = SpriteCollections.SelectMany(i => i.Armor).Single(i => i.Id == helmetId);
                var fullHair = helmet.Tags.Contains("FullHair");

                Helmet.sprite = helmet.Sprites.Single(i => i.name == "FrontHead");
                Ears.ForEach(j => { j.sprite = ears; j.enabled = true; });

                if (!fullHair)
                {
                    Hair.sprite = SpriteCollections.SelectMany(i => i.Hair).SingleOrDefault(i => i.Id == "Common.Basic.Hair.Default")?.Sprites[1];
                    Hair.enabled = Hair.sprite != null;
                }
            }

            Ears[0].transform.localPosition = appearance.Type == 0 ? new Vector3(-1f, 0.5f) : new Vector3(-0.9f, 0.7f);
            Ears[1].transform.localPosition = appearance.Type == 0 ? new Vector3(1f, 0.5f) : new Vector3(0.9f, 0.7f);
        }
    }
}