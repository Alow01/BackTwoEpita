using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Assets.HeroEditor4D.Common.Scripts.Common;
using Assets.HeroEditor4D.Common.Scripts.Enums;
using UnityEngine;

namespace Assets.HeroEditor4D.Common.Scripts.EditorScripts
{
    [RequireComponent(typeof(Camera))]
    public class SpriteSheetCapture : MonoBehaviour
    {
        private Character4D _character;

        public void Capture(Vector2 direction, List<CaptureOption> options, int frameSize, int frameCount, bool shadow)
        {
            StartCoroutine(CaptureFrames(direction, options, frameSize, frameCount, shadow));
        }

        private IEnumerator CaptureFrames(Vector2 direction, List<CaptureOption> options, int frameSize, int frameCount, bool shadow)
        {
            _character = FindObjectOfType<Character4D>();
            _character.SetDirection(direction);
            _character.Shadows.ForEach(i => i.SetActive(false));
            _character.Shadows[0].SetActive(shadow);

            var stateHandler = _character.Animator.GetBehaviours<StateHandler>().SingleOrDefault(i => i.Name == "Death");

            if (stateHandler)
            {
                stateHandler.StateExit.RemoveAllListeners();
            }

            var clips = new Dictionary<string, List<Texture2D>>();

            foreach (var option in options)
            {
                _character.Animator.speed = 0;

                for (var j = 0; j < frameCount; j++)
                {
                    var normalizedTime = (float) j / (frameCount - 1);
                    
                    yield return ShowFrame(option.StateL, option.StateU, option.StateC, normalizedTime);

                    var clip = _character.Animator.GetCurrentAnimatorClipInfo(option.StateU == null ? 2 : 1)[0].clip;
                    var expressionEvent = clip.events.Where(i => i.functionName == "SetExpression" && Mathf.Abs(i.time / clip.length - normalizedTime) <= 1f / (frameCount - 1))
                        .OrderBy(i => Mathf.Abs(i.time / clip.length - normalizedTime)).FirstOrDefault();

                    if (expressionEvent != null)
                    {
                        _character.SetExpression(expressionEvent.stringParameter);
                    }

                    var frame = CaptureFrame(frameSize, frameSize);
                    var animationName = option.StateU ?? option.StateL ?? option.StateC;

                    if (clips.ContainsKey(animationName))
                    {
                        clips[animationName].Add(frame);
                    }
                    else
                    {
                        clips.Add(animationName, new List<Texture2D> { frame });
                    }

                    yield return new WaitForSeconds(0.2f);
                }
            }

            _character.AnimationManager.SetState(CharacterState.Idle);
            _character.Animator.speed = 1;

            if (stateHandler)
            {
                stateHandler.StateExit.AddListener(() => _character.SetExpression("Default"));
            }

            var texture = CreateSheet(clips, frameSize, frameSize);

            yield return StandaloneFilePicker.SaveFile("Save as sprite sheet", "", "Character", ".png", texture.EncodeToPNG(), (success, path) => { Debug.Log(success ? $"Saved as {path}" : "Error saving."); });
        }

        private IEnumerator ShowFrame(string stateL, string stateU, string stateC, float normalizedTime)
        {
            switch (stateU)
            {
                case "Idle" when _character.WeaponType == WeaponType.Firearm1H:
                    stateU = "IdleFirearm1H";
                    break;
                case "Idle" when _character.WeaponType == WeaponType.Firearm2H:
                    stateU = "IdleFirearm2H";
                    break;
                case "Ready" when _character.WeaponType == WeaponType.Firearm1H:
                    stateU = "ReadyFirearm1H";
                    break;
                case "Ready" when _character.WeaponType == WeaponType.Firearm2H:
                    stateU = "ReadyFirearm2H";
                    break;
                case "Ready":
                    stateU = "Ready1H";
                    break;
            }

            if (stateC != null)
            {
                _character.Animator.Play(stateC, 2, normalizedTime);
            }
            else
            {
                _character.Animator.Play(stateL, 0, normalizedTime);
                _character.Animator.Play(stateU, 1, normalizedTime);
            }
            
            yield return null;

            while (_character.Animator.GetCurrentAnimatorClipInfo(stateC == null ? 0 : 2).Length == 0)
            {
                yield return null;
            }

            if (_character.Animator.IsInTransition(1))
            {
                Debug.Log("IsInTransition");
            }
        }

        private Texture2D CaptureFrame(int width, int height)
        {
            var cam = GetComponent<Camera>();
            var renderTexture = new RenderTexture(width, height, 24);
            var texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);

            cam.targetTexture = renderTexture;
            cam.Render();
            RenderTexture.active = renderTexture;
            texture2D.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            cam.targetTexture = null;
            RenderTexture.active = null;
            Destroy(renderTexture);

            return texture2D;
        }

        private Texture2D CreateSheet(Dictionary<string, List<Texture2D>> clips, int width, int height)
        {
            var texture = new Texture2D(clips.First().Value.Count * width, clips.Keys.Count * height);

            foreach (var clip in clips)
            {
                for (var i = 0; i < clip.Value.Count; i++)
                {
                    texture.SetPixels(i * width, clips.Keys.Reverse().ToList().IndexOf(clip.Key) * height, width, height, clip.Value[i].GetPixels());
                }
            }

            texture.Apply();

            return texture;
        }
    }

    public class CaptureOption
    {
        public string StateL;
        public string StateU;
        public string StateC;

        public CaptureOption(string stateL)
        {
            StateL = stateL;
            StateU = stateL;
        }

        public CaptureOption(string stateL, string stateU, string stateC = null)
        {
            StateL = stateL;
            StateU = stateU;
            StateC = stateC;
        }
    }
}