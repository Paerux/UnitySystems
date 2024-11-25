using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Paerux.Utils
{
    public static class Extensions
    {
        #region Game Object Extensions
        public static void HideInHierarchy(this GameObject gameObject)
        {
            gameObject.hideFlags = HideFlags.HideInHierarchy;
        }
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            return component ? component : gameObject.AddComponent<T>();
        }
        public static void DestroyChildren(this GameObject gameObject)
        {
            gameObject.transform.DestroyChildren();
        }
        public static void EnableChildren(this GameObject gameObject)
        {
            gameObject.transform.EnableChildren();
        }
        public static void DisableChildren(this GameObject gameObject)
        {
            gameObject.transform.DisableChildren();
        }
        public static void ResetTransform(this GameObject gameObject)
        {
            gameObject.transform.Reset();
        }
        #endregion
        #region Transform Extensions
        public static void ForEveryChild(this Transform parent, Action<Transform> action)
        {
            for (var i = parent.childCount - 1; i >= 0; i--)
            {
                action(parent.GetChild(i));
            }
        }
        public static void EnableChildren(this Transform parent)
        {
            parent.ForEveryChild(child => child.gameObject.SetActive(true));
        }
        public static void DisableChildren(this Transform parent)
        {
            parent.ForEveryChild(child => child.gameObject.SetActive(false));
        }
        public static void DestroyChildren(this Transform parent)
        {
            parent.ForEveryChild(child => Object.Destroy(child.gameObject));
        }
        public static void Reset(this Transform transform)
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }
        #endregion
        #region Vector2 Extensions
        public static Vector2 Add(this Vector2 vector, float x = 0, float y = 0)
        {
            return new Vector2(vector.x + x, vector.y + y);
        }
        public static Vector2 With(this Vector2 vector, float? x = null, float? y = null)
        {
            return new Vector2(x ?? vector.x, y ?? vector.y);
        }
        public static bool InRangeOf(this Vector2 vector, Vector2 target, float range)
        {
            return (vector - target).sqrMagnitude <= range * range;
        }
        public static Vector3 ToVector3(this Vector2 vector, float z = 0)
        {
            return new Vector3(vector.x, vector.y, z);
        }
        #endregion
        #region Vector3 Extensions
        public static Vector3 Add(this Vector3 vector, float x = 0, float y = 0, float z = 0)
        {
            return new Vector3(vector.x + x, vector.y + y, vector.z + z);
        }
        public static Vector3 With(this Vector3 vector, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
        }
        public static bool InRangeOf(this Vector3 vector, Vector3 target, float range)
        {
            return (vector - target).sqrMagnitude <= range * range;
        }
        #endregion
        #region String Extensions
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static bool IsBlank(this string str)
        {
            return str.IsNullOrWhiteSpace() || str.IsNullOrEmpty();
        }

        public static string OrEmpty(this string str)
        {
            return str ?? string.Empty;
        }

        public static string Slice(this string str, int start, int end)
        {
            if (str.IsBlank())
            {
                throw new ArgumentException("String is null or empty");
            }

            if (start < 0 || start > str.Length - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(start), "Start index is out of range");
            }
            end = end < 0 ? str.Length + end : end;
            if (end < 0 || end < start || end > str.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(end), "End index is out of range");
            }

            return str[start..end];
        }

        #endregion
        #region List Extensions
        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            var rng = new System.Random();
            int count = list.Count;
            while (count > 1)
            {
                count--;
                int k = rng.Next(count + 1);
                T value = list[k];
                list[k] = list[count];
                list[count] = value;
            }
            return list;
        }
        #endregion
        #region Color Extensions
        public static Color SetAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }
        public static string ToHex(this Color color)
        {
            return $"#{ColorUtility.ToHtmlStringRGBA(color)}";
        }
        public static Color Invert(this Color color)
        {
            return new Color(1 - color.r, 1 - color.g, 1 - color.b, color.a);
        }
        #endregion
    }
}