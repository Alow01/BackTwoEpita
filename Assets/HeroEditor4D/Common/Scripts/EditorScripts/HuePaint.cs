using UnityEngine;

namespace Assets.HeroEditor4D.Common.Scripts.EditorScripts
{
    public class HuePaint : MonoBehaviour
    {
        [Range(0.0f, 1.0f)] 
        public float Hue;

        private MaterialPropertyBlock _materialPropertyBlock;
        private SpriteRenderer _spriteRenderer;

        private readonly int _shaderColor = Shader.PropertyToID("_Color");
        private readonly int _shaderHue = Shader.PropertyToID("_Hue");

        public void OnValidate()
        {
            if (Hue > 0)
            {
                ShiftHue();
            }
        }

        public void Start()
        {
            if (Hue > 0)
            {
                ShiftHue();
            }
        }

        public void ShiftHue()
        {
            _materialPropertyBlock ??= new MaterialPropertyBlock();
            _spriteRenderer ??= GetComponent<SpriteRenderer>();

            var spriteColor = _spriteRenderer.color;

            _materialPropertyBlock.SetColor(_shaderColor, spriteColor);
            _materialPropertyBlock.SetFloat(_shaderHue, Hue);
            _spriteRenderer.SetPropertyBlock(_materialPropertyBlock);
        }
    }
}