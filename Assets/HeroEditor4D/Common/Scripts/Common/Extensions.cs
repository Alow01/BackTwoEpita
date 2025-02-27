using System;
using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor4D.Common.Scripts.Data;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.HeroEditor4D.Common.Scripts.Common
{
    public static class Extensions
    {
        public static void SetActive(this Component target, bool active)
        {
            target.gameObject.SetActive(active);
        }

        public static bool IsEmpty(this string target)
        {
            return string.IsNullOrEmpty(target);
        }

        public static void Clear(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                Object.Destroy(child.gameObject);
            }
        }

        public static T ToEnum<T>(this string value) where T : Enum
        {
            if (string.IsNullOrEmpty(value)) return (T) Enum.GetValues(typeof(T)).GetValue(0);

            return (T) Enum.Parse(typeof(T), value);
        }

        public static T Random<T>(this T[] source)
        {
            return source[UnityEngine.Random.Range(0, source.Length)];
        }

        public static T Random<T>(this List<T> source)
        {
            return source[UnityEngine.Random.Range(0, source.Count)];
        }

        public static T Random<T>(this List<T> source, int seed)
        {
            UnityEngine.Random.InitState(seed);

            return source[UnityEngine.Random.Range(0, source.Count)];
        }

        public static Sprite FindSprite(this List<ItemSprite> list, string id)
        {
            return list.Single(i => i.Id == id).Sprite;
        }

        public static List<Sprite> FindSprites(this List<ItemSprite> list, string id)
        {
            return list.Single(i => i.Id == id).Sprites.ToList();
        }
    }
}