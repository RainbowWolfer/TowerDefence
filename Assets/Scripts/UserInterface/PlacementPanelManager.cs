using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefence.Towers;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence.UserInterface {
	public class PlacementPanelManager: MonoBehaviour {
		[SerializeField]
		private GameObject detailPanelPrefab;
		[SerializeField]
		private GameObject descriptionPanelPrefab;

		private readonly Dictionary<Placement, PlacementPanel> pool = new Dictionary<Placement, PlacementPanel>();

		public Placement Current { get; private set; }
		public PlacementPanel CurrentPanel => pool[Current];

		public bool IsMouseContact { get; private set; }

		private void Awake() {
			Clear();
		}

		private void Update() {
			//Debug.Log(HasMouseOnPanels());
			//DebugPool();
		}

		private void DebugPool() {
			string result = "";
			foreach(KeyValuePair<Placement, PlacementPanel> item in pool) {
				result += $"({item.Key}) - ({item.Value})";
			}
			Debug.Log(result);
		}

		public bool HasMouseOnPanels() => pool.Values.Any(p => p.IsMouseOnPanel);

		public void Request(Placement p) {
			if(p == null || Current == p) {
				return;
			}

			foreach(PlacementPanel item in pool.Values) {
				item.Show = false;
			}

			Current = p;
			if(pool.ContainsKey(p)) {
				pool[p].Show = true;
			} else {
				GameObject prefab;
				if(p is Tower) {
					prefab = detailPanelPrefab;
				} else if(p is EnvironmentCube) {
					prefab = descriptionPanelPrefab;
				} else if(p is Emplacement) {
					prefab = detailPanelPrefab;
				} else {
					throw new Exception("new type of placement not implemented");
				}
				var panel = Instantiate(prefab, transform).GetComponent<PlacementPanel>();
				pool.Add(p, panel);
				panel.Initialize(p, this);
			}
		}

		public void ClearRequest() {//maybe i should call it deselected?
			Current = null;
			foreach(PlacementPanel item in pool.Values) {
				item.Show = false;
			}
		}

		private void Clear() {
			for(int i = 0; i < transform.childCount; i++) {
				Destroy(transform.GetChild(i).gameObject);
			}
			pool.Clear();
		}

		public void Remove(Placement t) {
			Destroy(pool[t].gameObject);
			pool.Remove(t);
		}

	}
}
