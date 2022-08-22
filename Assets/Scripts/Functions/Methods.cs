using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
	}
}
