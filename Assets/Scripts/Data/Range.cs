using System;
using Random = UnityEngine.Random;

namespace TowerDefence.Scripts.Data {
	[Serializable]
	public struct Range {
		public float from;
		public float to;

		public float GetRandom() {
			return Random.Range(from, to);
		}
	}
}
