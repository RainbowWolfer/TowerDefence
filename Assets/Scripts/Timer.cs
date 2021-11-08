using UnityEngine;

namespace TowerDefence {
	public sealed class Timer {
		private float nextTime = 0;
		public bool EverySeconds(float rate) {
			if(nextTime < Time.time) {
				nextTime = Time.time + rate;
				return true;
			}
			return false;
		}
		private int a = 0;
		public bool OnlyRunOnce(bool b) {
			if(b) {
				if(a == 0) {
					a++;
					return true;
				}
			}
			return false;
		}
		private void SetA() {
			a = 0;
		}
		private float startTime = 0;
		public bool WaitForSeconds(float time) {
			if(OnlyRunOnce(true)) {
				startTime = Time.time;
			}
			if(Time.time > startTime + time) {
				return true;
			}
			return false;
		}
		public override string ToString() {
			return base.ToString() + " : " + nextTime;
		}
	}
}