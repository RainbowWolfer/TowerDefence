using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefence.Data;
using TowerDefence.Placements;
using UnityEngine;

namespace TowerDefence.UserInterface {
	public class IconManager: MonoBehaviour {
		public delegate void OnSelectionChangedEventHandler(SelectionBaseIcon newSelection);
		public event OnSelectionChangedEventHandler OnSelectionChanged;

		[SerializeField]
		private MyButton button_towers;
		[SerializeField]
		private MyButton button_enplacements;

		private MyButton currentButton;

		[SerializeField]
		private GameObject prefab_icon;
		[SerializeField]
		private List<SelectionBaseIcon> icons = new List<SelectionBaseIcon>();

		private void Awake() {
			Clear();
			button_towers.OnUp += () => {
				if(currentButton == button_towers) {
					return;
				}
				button_towers.IsSelected = true;
				button_enplacements.IsSelected = false;
				currentButton = button_towers;
				DrawTowers();
			};
			button_enplacements.OnUp += () => {
				if(currentButton == button_enplacements) {
					return;
				}
				button_towers.IsSelected = false;
				button_enplacements.IsSelected = true;
				currentButton = button_enplacements;
				DrawEnplacements();
			};
		}

		private void Start() {
			button_towers.IsSelected = true;
			currentButton = button_towers;
			DrawTowers();
		}

		private void DrawTowers() {
			Draw(new List<TowerInfo>() {
				Game.Instance.Towers.RequestByID(1),
				Game.Instance.Towers.RequestByID(2),
				Game.Instance.Towers.RequestByID(3),
				Game.Instance.Towers.RequestByID(4),
				Game.Instance.Towers.RequestByID(5),
				Game.Instance.Towers.RequestByID(6),
			});
		}

		private void DrawEnplacements() {
			Draw(new List<TowerInfo>() {
				Game.Instance.Towers.RequestByID(7),
				Game.Instance.Towers.RequestByID(8),
				Game.Instance.Towers.RequestByID(9),
				Game.Instance.Towers.RequestByID(1),
				Game.Instance.Towers.RequestByID(2),
				Game.Instance.Towers.RequestByID(3),
			});
		}

		public void Draw(List<TowerInfo> towers) {
			DisapperToClear();
			for(int i = 0; i < towers.Count; i++) {
				var icon = Instantiate(prefab_icon, transform).GetComponent<SelectionBaseIcon>();
				icon.Initialize(110 + i * 190, towers[i], this);
				icon.Flyout.offsetX = i == 0 ? 170 : 0;
				icons.Add(icon);
			}
		}

		private void DisapperToClear() {
			foreach(SelectionBaseIcon item in icons) {
				item.Disappear();
			}
		}

		private void Clear() {
			for(int i = 0; i < transform.childCount; i++) {
				Destroy(transform.GetChild(i).gameObject);
			}
			icons.Clear();
		}

		public void Remove(SelectionBaseIcon icon) {
			icons.Remove(icon);
			Destroy(icon.gameObject);
		}

		public TowerInfo GetSelected() => icons.Find(i => i.IsSelected)?.Info;

		public SelectionBaseIcon Selected => icons.Find(i => i.IsSelected);

		public void Select(SelectionBaseIcon target) {
			if(icons.Find(i => i == target).IsSelected) {
				DeselectAll();//deselect only current. works anyway
				OnSelectionChanged?.Invoke(null);
				return;
			}
			Level.CheckForLowPowers();
			if(!target.CashAvailable) {
				//pop up notification
				NotificationPanel.InsufficientFund();
				return;
			}
			icons.ForEach(i => i.IsSelected = i == target);
			OnSelectionChanged?.Invoke(target);
		}

		public void DeselectAll() {
			icons.ForEach(i => i.IsSelected = false);
			OnSelectionChanged?.Invoke(null);
		}
	}
}
