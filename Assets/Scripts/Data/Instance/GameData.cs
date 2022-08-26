using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefence.Scripts.Data;
using UnityEngine;

namespace TowerDefence.Data.Instance {
	public class GameData: MonoBehaviour {
		public static GameData Instance { get; private set; }


		[field: SerializeField]
		public TowerPrefabs Towers { get; private set; }

		[field: SerializeField]
		public EnemyPrefabs Enemies { get; private set; }

		[field: SerializeField]
		public CardsBenefits Cards { get; private set; }


		private void Awake() {
			Instance = this;
		}
	}
}
