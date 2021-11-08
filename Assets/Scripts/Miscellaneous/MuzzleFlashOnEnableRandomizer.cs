using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TowerDefence.Miscellaneous {
	public class MuzzleFlashOnEnableRandomizer: MonoBehaviour {
		public bool enable;
		private void OnEnable() {
			if(enable) {
				transform.localEulerAngles = new Vector3(Random.Range(0, 360), 0, 0);
			}
		}
	}
}
