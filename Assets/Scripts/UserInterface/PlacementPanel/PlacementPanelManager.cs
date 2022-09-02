using System;
using System.Collections.Generic;
using System.Linq;
using TowerDefence.Placements;
using TowerDefence.Placements.Environments;
using UnityEngine;
namespace TowerDefence.UserInterface {
	public class PlacementPanelManager: MonoBehaviour {
		[SerializeField]
		private GameObject detailPanelPrefab;
		[SerializeField]
		private GameObject descriptionPanelPrefab;
		[SerializeField]
		private GameObject abilityPanelPrefab;

		//private readonly Dictionary<Placement, PlacementPanel> pool = new Dictionary<Placement, PlacementPanel>();
		private readonly List<PlacementPanel> panels = new List<PlacementPanel>();

		public AbilityPanel CurrentAbilityPanel { get; private set; }

		public Placement Current { get; private set; }
		//public PlacementPanel CurrentPanel => pool[Current];

		private void Awake() {
			Clear();
		}

		private void Update() {
			//Debug.Log(pool.Count);
		}

		public bool HasMouseOnPanels() => panels.Any(p => p.MouseDetector.GetHasMouseOn());

		public void RequestAbility(Emplacement emplacement) {
			ClearRequest();
			CurrentAbilityPanel = Instantiate(abilityPanelPrefab, transform).GetComponent<AbilityPanel>();
			CurrentAbilityPanel.Initialize(emplacement, this);
			panels.Add(CurrentAbilityPanel);
			Game.Instance.control.StartAbility(emplacement);
		}

		public void Request(Placement placement) {
			if(placement == null || Current == placement) {
				return;
			}

			foreach(PlacementPanel item in panels) {
				item.Show = false;
			}

			Current = placement;
			PlacementPanel found = panels.Find(i => i.TargetPlacement == placement);
#pragma warning disable CS0184 // 'is' expression's given expression is never of the provided type
			if(found != null && !found is AbilityPanel) {
#pragma warning restore CS0184 // 'is' expression's given expression is never of the provided type
				found.Show = true;
			} else {
				GameObject prefab;
				if(placement is FieldPlacement) {
					prefab = detailPanelPrefab;
				} else if(placement is EnvironmentCube) {
					prefab = descriptionPanelPrefab;
				} else {
					throw new Exception($"new type of placement not implemented ({placement.GetType()})");
				}
				var panel = Instantiate(prefab, transform).GetComponent<PlacementPanel>();
				panel.Initialize(placement, this);
				panels.Add(panel);
			}
		}

		public void ClearRequest() {//maybe i should call it deselected?
			CurrentAbilityPanel = null;
			Current = null;
			foreach(PlacementPanel item in panels) {
				item.Show = false;
			}
		}

		private void Clear() {
			for(int i = 0; i < transform.childCount; i++) {
				Destroy(transform.GetChild(i).gameObject);
			}
			panels.Clear();
		}

		public void Remove(PlacementPanel panel) {
			panels.Remove(panel);
			Destroy(panel.gameObject);
			//Destroy(pool[t].gameObject);
			//pool.Remove(t);
		}

		//public void Remove(Placement t) {

		//	//Destroy(pool[t].gameObject);
		//	//pool.Remove(t);
		//}

		//public void Remove(AbilityPanel ap) {
		//	Destroy(ap.gameObject);
		//	this.CurrentAbilityPanel = null;
		//}

		//public void RemoveEmpty() {

		//}
	}
}
