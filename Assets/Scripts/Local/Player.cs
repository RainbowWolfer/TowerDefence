using System;
using UnityEngine;

namespace TowerDefence.Local {
	[Serializable]
	public class Player {
		public static Player Current { get; set; }

		public string username;

		public int diamond = 0;
		public int rank = 1;
		public Cards cards;

		public Player(string name) {
			this.username = name;
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
