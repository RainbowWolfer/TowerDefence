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

	[Serializable]
	public struct Range<T> {
		public T from;
		public T to;

		public Range(T from, T to) {
			this.from = from;
			this.to = to;
		}
	}
}
