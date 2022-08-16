using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TowerDefence.UserInterface.IncomingPanel {
	public class EnemyCountsManager: MonoBehaviour {
		[SerializeField]
		private GameObject itemPrefab;
		[SerializeField]
		private List<EnemyCountItem> items = new List<EnemyCountItem>();

		private void Start() {
			Clear();
		}


		public void Clear() {
			items.Clear();
			for(int i = 0; i < transform.childCount; i++) {
				Destroy(transform.GetChild(i).gameObject);
			}
		}
	}
}
