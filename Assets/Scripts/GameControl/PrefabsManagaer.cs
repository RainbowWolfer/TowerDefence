using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TowerDefence.GameControl {
	public class PrefabsManagaer: MonoBehaviour {
		public static PrefabsManagaer Instance { get; private set; }
		private void Awake() {
			Instance = this;
		}

		[field: SerializeField]
		public GameObject CoinsBurst { get; private set; }


	}
}
