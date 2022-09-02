using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence.Functions {
	public static class Methods {
		public static string ToDuo(this int num) {
			if(0 <= num && num < 10) {
				return $"0{num}";
			} else {
				return $"{num}";
			}
		}

		public static bool CheckLinerIncrease(bool checkUnsigned, params float[] list) {
			if(checkUnsigned && list.Any(i => i < 0)) {
				return false;
			}
			return list.SequenceEqual(list.OrderBy(i => i).ToArray());
		}


		public static void PrintDictionary<T, W>(this Dictionary<T, W> dic) {
			string content = "";
			foreach(KeyValuePair<T, W> item in dic) {
				content += $"{item.Key} - {item.Value}\n";
			}
			Debug.LogWarning(content);
		}

		public static void ColorLerp(this Material material, string name, Color targetColor, float speed) {
			material.SetColor(name, Color.Lerp(
				material.GetColor(name), targetColor, speed
			));
		}

		public static string FormatMinAndSec(this int seconds) {
			int minute = 0;
			while(seconds - 60 >= 0) {
				minute++;
				seconds -= 60;
				if(minute > 99) {
					return "99:99";
				}
			}
			return $"{minute.ToDuo()}:{seconds.ToDuo()}";
		}

		public static Quaternion RandomRotation() {
			return Quaternion.Euler(
				Random.Range(0, 360),
				Random.Range(0, 360),
				Random.Range(0, 360)
			);
		}

		public static bool TryGetComponentInParent<T>(this Component component, out T result) where T : Component {
			result = component.GetComponentInParent<T>();
			return result != null;
		}

		public static string Format(this string raw, params object[] args) {
			if(string.IsNullOrWhiteSpace(raw)) {
				return raw;
			}
			string result = raw;
			for(int i = 0; i < args.Length; i++) {
				result = result.Replace("{{" + i + "}}", args[i].ToString());
			}
			return result;
		}


		public static void SetNativeSizeByHeight(this Image image, float height, float maxWidth = float.MaxValue) {
			image.SetNativeSize();
			float aspect = image.rectTransform.sizeDelta.y / image.rectTransform.sizeDelta.x;
			float width = height / aspect;
			if(width > maxWidth) {
				height = aspect * maxWidth;
				width = maxWidth;
			}
			image.rectTransform.sizeDelta = new Vector2(width, height);
		}
	}
}
