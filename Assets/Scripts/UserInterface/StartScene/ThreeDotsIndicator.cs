using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TowerDefence.UserInterface.StartScene {
	[ExecuteInEditMode]
	public class ThreeDotsIndicator: MonoBehaviour {
		[field: SerializeField]
		public int Count { get; set; }

		[SerializeField]
		private GameObject dot1;
		[SerializeField]
		private GameObject dot2;
		[SerializeField]
		private GameObject dot3;


		private void Update() {
			Count = Mathf.Clamp(Count, 0, 3);
			dot1.SetActive(Count > 0);
			dot2.SetActive(Count > 1);
			dot3.SetActive(Count > 2);
		}
	}
}
