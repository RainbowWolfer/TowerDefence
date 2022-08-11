using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TowerDefence.Functions {
	public static class Methods {

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
	}
}
