using System;
using System.Linq;
using Assets.HeroEditor4D.Common.Scripts.Common;
using Assets.HeroEditor4D.Common.Scripts.Data;
using UnityEngine;

namespace Assets.HeroEditor4D.Common.Scripts.CharacterScripts
{
    [Serializable]
    public class CharacterAppearance
    {
        public string Hair = "Common.Basic.Hair.BuzzCut";
        public string Beard = null;
        public string Ears = "Common.Basic.Ears.Human";
        public string Eyebrows = "Common.Basic.Eyebrows.Default";
        public string Eyes = "Common.Basic.Eyes.Boy";
        public string Mouth = "Common.Basic.Mouth.Default";
        public string Body = "Common.Basic.Body.HumanPants";
        public string Underwear = "Common.Underwear.Armor.MaleUnderwear";
        
        public Color32 HairColor = new Color32(150, 50, 0, 255);
        public Color32 BeardColor = new Color32(150, 50, 0, 255);
        public Color32 EyesColor = new Color32(0, 200, 255, 255);
        public Color32 BodyColor = new Color32(255, 200, 120, 255);
        public Color32 UnderwearColor = new Color32(120, 100, 80, 255);

        public bool ShowHelmet = true;
        public int Type;

        public void Setup(Character4D character)
        {
            character.Parts.ForEach(i => Setup(i));
        }

        public void Setup(Character character, bool initialize = true)
        {
            if (character.SpriteCollection.Id != "FantasyHeroes" && character.SpriteCollection.Id != "MilitaryHeroes") return; // Not supported yet.

            var hair = Hair.IsEmpty() ? null : character.SpriteCollection.Hair.Single(i => i.Id == Hair);

            character.Hair = hair == null ? null : character.HairRenderer.GetComponent<SpriteMapping>().FindSprite(hair.Sprites);
            character.HairRenderer.color = hair != null && hair.Tags.Contains("NoPaint") ? (Color32) Color.white : HairColor;

            if (character.BeardRenderer)
            {
                var beard = Beard.IsEmpty() ? null : character.SpriteCollection.Beard.Single(i => i.Id == Beard);

                character.Beard = beard == null ? null : character.BeardRenderer.GetComponent<SpriteMapping>().FindSprite(beard.Sprites);
                character.BeardRenderer.color = BeardColor;
            }

            character.Ears = Ears.IsEmpty() ? null : character.SpriteCollection.Ears.FindSprites(Ears);

            if (character.Expressions.Count > 0)
            {
                character.Expressions[0] = new Expression { Name = "Default" };

                if (character.name != "Back")
                {
                    character.Expressions[0].Eyebrows = Eyebrows.IsEmpty() ? null : character.EyebrowsRenderer.GetComponent<SpriteMapping>().FindSprite(character.SpriteCollection.Eyebrows.FindSprites(Eyebrows));
                    character.Expressions[0].Eyes = character.EyesRenderer.GetComponent<SpriteMapping>().FindSprite(character.SpriteCollection.Eyes.FindSprites(Eyes));
                    character.Expressions[0].Mouth = character.MouthRenderer.GetComponent<SpriteMapping>().FindSprite(character.SpriteCollection.Mouth.FindSprites(Mouth));
                }

                foreach (var expression in character.Expressions)
                {
                    if (expression.Name != "Dead") expression.EyesColor = EyesColor;
                }
            }

            if (character.EyesRenderer != null)
            {
                character.EyesRenderer.color = EyesColor;
            }

            character.BodyRenderers.ForEach(i => i.color = BodyColor);
            character.HeadRenderer.color = BodyColor;
            character.EarsRenderers.ForEach(i => i.color = BodyColor);

            var body = character.SpriteCollection.Body.Single(i => i.Id == Body);

            character.Body = body.Sprites.ToList();

            if (body.Tags.Contains("NoMouth"))
            {
                character.Expressions.ForEach(i => i.Mouth = null);
            }

            if (initialize) character.Initialize();
        }

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

        public static CharacterAppearance FromJson(string json)
        {
            return JsonUtility.FromJson<CharacterAppearance>(json);
        }
    }
}