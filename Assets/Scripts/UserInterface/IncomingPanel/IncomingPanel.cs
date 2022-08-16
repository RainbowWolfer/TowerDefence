using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TowerDefence.UserInterface.IncomingPanel {
	public class IncomingPanel: MonoBehaviour {
		[field: SerializeField]
		public bool IsMouseOn { get; private set; }

		[SerializeField]
		private GameObject[] backgrounds;


		private void Update() {
			IsMouseOn = UIRayCaster.HasElements(backgrounds);

		}
	}
}
