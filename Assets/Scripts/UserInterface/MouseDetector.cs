using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

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
