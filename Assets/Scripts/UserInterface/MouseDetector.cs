using System;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefence.UserInterface {
	[Serializable]
	public class MouseDetector {
		[field: SerializeField]
		public List<GameObject> Backgrounds { get; private set; }

		public bool GetHasMouseOn() {
			return UIRayCaster.HasElements(Backgrounds);
		}
	}
}
