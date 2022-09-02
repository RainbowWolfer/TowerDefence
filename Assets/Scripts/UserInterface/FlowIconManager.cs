using System.Collections.Generic;
using TowerDefence.Enemies;
using UnityEngine;

namespace TowerDefence.UserInterface {
	public class FlowIconManager: MonoBehaviour {
		public readonly List<HealthBar> healthbars = new List<HealthBar>();

		public HealthBar AddHealthBar(Enemy enemy) {
			HealthBar bar = Instantiate(UI.Instance.prefab_healthBar, transform).GetComponent<HealthBar>();
			bar.target = enemy;
			healthbars.Add(bar);
			return bar;
		}

		public void RemoveHealthBar(Enemy enemy) {
			for(int i = 0; i < healthbars.Count; i++) {
				if(healthbars[i].target == enemy) {
					Destroy(healthbars[i].gameObject);
					healthbars.RemoveAt(i);
					return;
				}
			}
		}

		public void ClearHealthBars() {
			foreach(HealthBar item in healthbars) {
				Destroy(item.gameObject);
			}
			healthbars.Clear();
		}
	}
}
