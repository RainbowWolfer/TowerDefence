using TowerDefence.Scripts.Data;
using UnityEngine;

namespace TowerDefence.Functions {
	public class LoopTimer {
		private float nextTime = 0;
		public float Interval { get; set; }
		public Range<float> RandomRange { get; set; }

		public LoopTimer(float interval, Range<float> range) {
			Interval = interval;
			RandomRange = range;
		}
		public LoopTimer(float interval, float from, float to) {
			Interval = interval;
			RandomRange = new Range<float>(from, to);
		}

		public void RandomizeInterval() {
			Interval = Random.Range(RandomRange.from, RandomRange.to);
		}

		public void ResetToNextInterval(bool randomize = true) {
			if(randomize) {
				RandomizeInterval();
			}
			nextTime = Time.time + Interval;
		}

		public bool EveryTime(bool randomizeWhenDone = false) {
			if(nextTime < Time.time) {
				nextTime = Time.time + Interval;
				if(randomizeWhenDone) {
					RandomizeInterval();
				}
				return true;
			}
			return false;
		}
	}
}
