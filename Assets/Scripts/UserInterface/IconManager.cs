using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefence.Data;
using TowerDefence.Towers;
using UnityEngine;

namespace TowerDefence.UserInterface {
	public class IconManager: MonoBehaviour {
		public delegate void OnSelectionChangedEventHandler(Icon newSelection);
		public event OnSelectionChangedEventHandler OnSelectionChanged;

		[SerializeField]
		private MyButton button_towers;
		[SerializeField]
		private MyButton button_enplacements;
		[SerializeField]
		private MyButton button_tools;

		private MyButton currentButton;

		[SerializeField]
		private GameObject prefab_icon;
		[SerializeField]
		private List<Icon> icons = new List<Icon>();

		private void Awake() {
			Clear();
			button_towers.OnUp += () => {
				button_towers.IsSelected = true;
				button_enplacements.IsSelected = false;
				button_tools.IsSelected = false;
				currentButton = button_towers;
				DrawTowers();
			};
			button_enplacements.OnUp += () => {
				button_towers.IsSelected = false;
				button_enplacements.IsSelected = true;
				button_tools.IsSelected = false;
				currentButton = button_enplacements;
				DrawEnplacements();
			};
			button_tools.OnUp += () => {
				button_towers.IsSelected = false;
				button_enplacements.IsSelected = false;
				button_tools.IsSelected = true;
				currentButton = button_tools;
				DrawTools();
			};
		}

		private void Start() {
			button_towers.IsSelected = true;
			currentButton = button_towers;
			DrawTowers();
		}

		private void DrawTowers() {
			Draw(new List<TowerInfo>() {
				Game.Towers.RequestByID(1),
				Game.Towers.RequestByID(2),
				Game.Towers.RequestByID(3),
				Game.Towers.RequestByID(4),
				Game.Towers.RequestByID(3),
				Game.Towers.RequestByID(2),
			});
		}

		private void DrawEnplacements() {
			Draw(new List<TowerInfo>() {
				Game.Towers.RequestByID(7),
				Game.Towers.RequestByID(7),
				Game.Towers.RequestByID(7),
				Game.Towers.RequestByID(1),
				Game.Towers.RequestByID(2),
				Game.Towers.RequestByID(3),
			});
		}

		private void DrawTools() {
			Draw(new List<TowerInfo>() {
				Game.Towers.RequestByID(2),
				Game.Towers.RequestByID(2),
				Game.Towers.RequestByID(1),
				Game.Towers.RequestByID(2),
				Game.Towers.RequestByID(1),
				Game.Towers.RequestByID(1),
			});
		}

		public void Draw(List<TowerInfo> towers) {
			DisapperToClear();
			for(int i = 0; i < towers.Count; i++) {
				Icon icon = Instantiate(prefab_icon, transform).GetComponent<Icon>();
				icon.Initialize(110 + i * 190, towers[i], this);
				icons.Add(icon);
			}
		}

		private void DisapperToClear() {
			foreach(Icon item in icons) {
				item.Disappear();
			}
		}

		private void Clear() {
			for(int i = 0; i < transform.childCount; i++) {
				Destroy(transform.GetChild(i).gameObject);
			}
			icons.Clear();
		}

		public void Remove(Icon icon) {
			icons.Remove(icon);
			Destroy(icon.gameObject);
		}

		public TowerInfo GetSelected() => icons.Find(i => i.IsSelected)?.Info;

		public Icon Selected => icons.Find(i => i.IsSelected);
		public void Select(Icon target) {
			if(icons.Find(i => i == target).IsSelected) {
				DeselectAll();//deselect only current. works anyway
				OnSelectionChanged?.Invoke(null);
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
