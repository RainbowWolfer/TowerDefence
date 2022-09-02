using UnityEngine;

namespace TowerDefence.GameControl {
	public class PrefabsManagaer: MonoBehaviour {
		public static PrefabsManagaer Instance { get; private set; }
		private void Awake() {
			Instance = this;
		}

		[field: SerializeField]
		public GameObject CoinsBurst { get; private set; }

		[field: SerializeField]
		public GameObject TowerLevelUpEffect { get; private set; }

		[field: SerializeField]
		public GameObject TowerUpgradeEffect { get; private set; }


	}
}
