using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Assets.HeroEditor4D.Common.Scripts.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.HeroEditor4D.Common.Scripts.EditorScripts
{
    /// <summary>
    /// Used for creating sprite sheets for frame-by-frame animation.
    /// </summary>
    public class SpriteSequenceCapture : MonoBehaviour
    {
        public Character4D Character4D;
        public GameObject Canvas;
	    public GameObject Camera;
        public string UpperBodyAnimationFolder;
	    public string LowerBodyAnimationFolder;
        public string ComplexAnimationFolder;
        public List<string> UpperBodyClipNames;
	    public List<string> LowerBodyClipNames;
        public List<string> ComplexClipNames;
        public Dropdown DirectionDropdown;
        public Dropdown PrimaryAnimationDropdown;
	    public Dropdown SecondaryAnimationDropdown;
	    public Slider CameraSizeSlider;
		public Dropdown FrameSizeDropdown;
        public Dropdown FrameRatioDropdown;
        public Dropdown ScreenshotIntervalDropdown;
        public Dropdown ShadowDropdown;
	    public GameObject Shadow;

        public ScreenshotTransparent ScreenshotTransparent;

		#if UNITY_EDITOR

		/// <summary>
		/// Called only in Editor.
		/// </summary>
		public void OnValidate()
        {
	        UpperBodyClipNames = Directory.GetFiles(UpperBodyAnimationFolder, "*.anim", SearchOption.AllDirectories).Select(Path.GetFileNameWithoutExtension).ToList();
			LowerBodyClipNames = Directory.GetFiles(LowerBodyAnimationFolder, "*.anim", SearchOption.AllDirectories).Select(Path.GetFileNameWithoutExtension).ToList();
            ComplexClipNames = Directory.GetFiles(ComplexAnimationFolder, "*.anim", SearchOption.AllDirectories).Select(Path.GetFileNameWithoutExtension).ToList();
            UpperBodyClipNames.AddRange(ComplexClipNames);

            for (var i = 0; i < UpperBodyClipNames.Count; i++)
            {
                if (UpperBodyClipNames[i].EndsWith("U")) UpperBodyClipNames[i] = UpperBodyClipNames[i].Substring(0, UpperBodyClipNames[i].Length - 1);
            }

            for (var i = 0; i < LowerBodyClipNames.Count; i++)
            {
                if (LowerBodyClipNames[i].EndsWith("L")) LowerBodyClipNames[i] = LowerBodyClipNames[i].Substring(0, LowerBodyClipNames[i].Length - 1);
            }

            PrimaryAnimationDropdown.options = new List<Dropdown.OptionData>();
			SecondaryAnimationDropdown.options = new List<Dropdown.OptionData>();

            foreach (var clipName in UpperBodyClipNames)
            {
	            PrimaryAnimationDropdown.options.Add(new Dropdown.OptionData(clipName));
            }

	        foreach (var clipName in LowerBodyClipNames)
	        {
		        SecondaryAnimationDropdown.options.Add(new Dropdown.OptionData(clipName));
	        }

            PrimaryAnimationDropdown.value = UpperBodyClipNames.IndexOf("Slash1H");
            SecondaryAnimationDropdown.value = LowerBodyClipNames.IndexOf("Idle");
        }

        /// <summary>
        /// Called on start.
        /// </summary>
        public void Start()
        {
            if (Character4D) Character4D.SetDirection(Vector2.left);

            foreach (var dropdown in new[] { PrimaryAnimationDropdown, SecondaryAnimationDropdown, FrameSizeDropdown, FrameRatioDropdown, ScreenshotIntervalDropdown, ShadowDropdown })
            {
                dropdown.RefreshShownValue();
            }

			if (UpperBodyClipNames.Count == 0) OnValidate();
        }

        /// <summary>
        /// Called when direction dropdown changed.
        /// </summary>
        /// <param name="value"></param>
        public void OnDirectionChanged(int value)
        {
            switch (value)
            {
                case 0: Character4D.SetDirection(Vector2.left); break;
                case 1: Character4D.SetDirection(Vector2.right); break;
                case 2: Character4D.SetDirection(Vector2.down); break;
                case 3: Character4D.SetDirection(Vector2.up); break;
            }
        }

        /// <summary>
        /// Load character from prefab.
        /// </summary>
        public void Load()
        {
            var path = UnityEditor.EditorUtility.OpenFilePanel("Open character prefab", "", "prefab");

            if (path.Length > 0)
            {
                path = "Assets" + path.Replace(Application.dataPath, null);
                Load(path);
            }
        }

        /// <summary>
        /// Load character from prefab by given path.
        /// </summary>
        public void Load(string path)
        {
            var character = UnityEditor.AssetDatabase.LoadAssetAtPath<Character4D>(path);

            if (character == null) throw new Exception("Error loading character, please make sure you are loading correct prefab!");

			if (Character4D != null) Destroy(Character4D.gameObject);

	        Character4D = Instantiate(character, transform);
            Character4D.transform.localPosition = Vector3.zero;
            Shadow = Character4D.transform.Find("Shadow").gameObject;
            OnDirectionChanged(DirectionDropdown.value);

            var mat = new Material(Shader.Find("Sprites/Default"));

			foreach (var spriteRenderer in Character4D.GetComponentsInChildren<SpriteRenderer>())
	        {
		        if (spriteRenderer.name != "Eyes")
		        {
			        spriteRenderer.material = mat;
		        }
	        }

            Debug.LogWarning("All materials were replaced by [Sprites/Default] to avoid outline artefacts.");
        }

        /// <summary>
        /// Create sprite sheet.
        /// </summary>
        public void Capture()
        {
			var frameSize = new[] { 256, 512, 1024 }[FrameSizeDropdown.value];
            var frameRatio = FrameRatioDropdown.value + 4;
            var interval = new[] { 0.1f, 0.25f, 0.5f, 1f }[ScreenshotIntervalDropdown.value];

	        Camera.GetComponent<Camera>().orthographicSize = CameraSizeSlider.value;
	        Shadow.SetActive(ShadowDropdown.value == 0);

			var upperClips = new List<string> { UpperBodyClipNames[PrimaryAnimationDropdown.value] };
	        var lowerClips = new List<string> { LowerBodyClipNames[SecondaryAnimationDropdown.value] };

	        StartCoroutine(CaptureFrames(upperClips, lowerClips, frameSize, frameRatio, interval));
        }

        private void ShowFrame(string upperClip, string lowerClip, float normalizedTime)
        {
            if (ComplexClipNames.Contains(upperClip))
            {
                Character4D.Animator.Play(upperClip, 2, normalizedTime);
            }
            else
            {
                Character4D.Animator.Play(upperClip, 1, normalizedTime);
                Character4D.Animator.Play(lowerClip, 0, normalizedTime);
            }

            Character4D.Animator.speed = 0;

	        if (Character4D.Animator.IsInTransition(1))
	        {
				Debug.Log("IsInTransition");
	        }
		}

        private IEnumerator CaptureFrames(List<string> upperClips, List<string> lowerClips, int frameSize, int frameRatio, float interval)
        {
            Canvas.SetActive(false);

            var death = upperClips.Any(i => i.Contains("Death"));

            foreach (var upperClip in upperClips)
            {
	            foreach (var lowerClip in lowerClips)
	            {
                    for (var i = 0; i < frameRatio; i++)
                    {
                        ShowFrame(upperClip, lowerClip, (float) i / (frameRatio - 1));

			            yield return new WaitForSeconds(interval);

                        string direction;

                        switch (DirectionDropdown.value)
                        {
                            case 0: direction = "Left"; break;
                            case 1: direction = "Right"; break;
                            case 2: direction = "Front"; break;
                            case 3: direction = "Back"; break;
                            default: throw new NotImplementedException();
                        }

                        var path = death ? $"{Application.dataPath.Replace("/Assets", null)}/SpriteSheets/{direction}/{upperClip}/{i}.png" : $"{Application.dataPath.Replace("/Assets", null)}/SpriteSheets/{direction}/{upperClip}-{lowerClip}/{i}.png";

                        ScreenshotTransparent.Width = ScreenshotTransparent.Height = frameSize;
                        ScreenshotTransparent.Capture(path);
                    }
	            }

                if (death) break;
            }

            Canvas.SetActive(true);
        }

        #endif
    }
}