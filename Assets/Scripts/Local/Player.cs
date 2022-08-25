using System;
using UnityEngine;

namespace TowerDefence.Local {
	[Serializable]
	public class Player {
		public static Player Current { get; set; }

		public string name;

		public Player(string name) {
			this.name = name;
		}

		public static string ToJson() {
			return JsonUtility.ToJson(Current);
		}

		public static Player FromJson(string json) {
			if(string.IsNullOrWhiteSpace(json)) {
				return null;
			} else {
				return JsonUtility.FromJson<Player>(json);
			}
		}
	}
}
