using System;
using UnityEngine;

namespace TowerDefence.Data {
	[CreateAssetMenu(fileName = "Specials Data", menuName = "Data/Specials Data")]
	[Serializable]
	public class SpecialsData: ScriptableObject {
		//name & icon
		public Sprite icon;
		public Tier tier;

		public enum Tier {
			Normal, Rare
		}
	}
}
