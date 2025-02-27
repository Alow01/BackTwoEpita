using System.Collections.Generic;
using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Assets.HeroEditor4D.Common.Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HeroEditor4D.Common.Scripts.EditorScripts
{
    public class CaptureOptions : MonoBehaviour
    {
        public Toggle Left;
        public Toggle Right;
        public Toggle Front;
        public Toggle Back;

        public Toggle Idle;
        public Toggle Ready;
        public Toggle Walk;
        public Toggle Run;
        public Toggle Slash;
        public Toggle Jab;
        public Toggle Shot;
        public Toggle Cast;
        public Toggle Block;
        public Toggle Death;

        public Toggle Shadow;
        public InputField FrameSize;
        public InputField FrameCount;

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void Capture()
        {
            var direction = Vector2.right;

            if (Right.isOn) direction = Vector2.right;
            if (Left.isOn) direction = Vector2.left;
            if (Front.isOn) direction = Vector2.down;
            if (Back.isOn) direction = Vector2.up;

            var options = new List<CaptureOption>();

            if (Idle.isOn) options.Add(new CaptureOption("Idle"));
            if (Ready.isOn) options.Add(new CaptureOption("Ready"));
            if (Walk.isOn) options.Add(new CaptureOption("Walk"));
            if (Run.isOn) options.Add(new CaptureOption("Run"));
            if (Slash.isOn) options.Add(new CaptureOption("Idle", FindObjectOfType<Character4D>().WeaponType == WeaponType.Melee2H ? "Slash2H" : "Slash1H"));
            if (Jab.isOn) options.Add(new CaptureOption("Idle", "Jab"));

            if (Shot.isOn)
            {
                var character = FindObjectOfType<Character>();

                switch (character.WeaponType)
                {
                    case WeaponType.Bow:
                        options.Add(new CaptureOption("Idle", "ShotBow"));
                        break;
                    case WeaponType.Firearm1H:
                        options.Add(new CaptureOption("Ready", "Fire1H"));
                        break;
                    case WeaponType.Firearm2H:
                        options.Add(new CaptureOption("Ready", "Fire2H"));
                        break;
                    case WeaponType.Paired:
                        options.Add(new CaptureOption("Idle", "SecondaryShot"));
                        break;
                }
            }

            if (Cast.isOn) options.Add(new CaptureOption("Idle", "Cast"));
            if (Block.isOn) options.Add(new CaptureOption("ShieldBlock"));
            if (Death.isOn) options.Add(new CaptureOption(null, null, "Death"));

            FindObjectOfType<SpriteSheetCapture>().Capture(direction, options, int.Parse(FrameSize.text), int.Parse(FrameCount.text), Shadow.isOn);
            Close();
        }

        public void OnFrameSizeChanged(string value)
        {
            if (FrameSize.text == "") return;

            var valueInt = int.Parse(value);

            if (valueInt < 128) valueInt = 128;
            if (valueInt > 512) valueInt = 512;

            FrameSize.SetTextWithoutNotify(valueInt.ToString());
        }

        public void OnFrameCountChanged(string value)
        {
            if (FrameCount.text == "") return;

            var valueInt = int.Parse(value);

            if (valueInt < 4) valueInt = 4;
            if (valueInt > 16) valueInt = 16;

            FrameCount.SetTextWithoutNotify(valueInt.ToString());
        }
    }
}