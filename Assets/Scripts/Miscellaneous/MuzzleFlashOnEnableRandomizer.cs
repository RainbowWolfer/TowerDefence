using UnityEngine;
using Random = UnityEngine.Random;

namespace TowerDefence.Miscellaneous {
	public class MuzzleFlashOnEnableRandomizer: MonoBehaviour {
		public bool enable;
		private void OnEnable() {
			if(enable) {
				transform.localEulerAngles = new Vector3(
					Random.Range(0, 360),
					transform.localEulerAngles.y,
					transform.localEulerAngles.z
				);
			}
		}
	}
}
