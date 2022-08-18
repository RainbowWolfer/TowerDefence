using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefence.GameControl.Waves;
using UnityEngine;

namespace TowerDefence.UserInterface.LevelIncomingPanel {
	public class EnemyCountsManager: MonoBehaviour {
		[SerializeField]
		private GameObject itemPrefab;
		[SerializeField]
		private List<EnemyCountItem> items = new List<EnemyCountItem>();

		private void Start() {
			Clear();
		}

		public void Set(Dictionary<EnemyType, int> counts) {
			foreach(KeyValuePair<EnemyType, int> item in counts) {
				if(item.Value <= 0) {
					continue;
				}
				AddItem(item.Key, item.Value);
			}
		}

		private void AddItem(EnemyType type, int count) {
			if(items.Count >= 9) {
				return;
			}
			EnemyCountItem item = Instantiate(itemPrefab, transform).GetComponent<EnemyCountItem>();
			item.Set(type, count);
			items.Add(item);
		}

		public void Clear() {
			items.Clear();
			for(int i = 0; i < transform.childCount; i++) {
				Destroy(transform.GetChild(i).gameObject);
			}
		}

	}
}
