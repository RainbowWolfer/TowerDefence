using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefence.Data;
using TowerDefence.GameControl;
using TowerDefence.Towers;
using TowerDefence.UserInterface;
using UnityEngine;

namespace TowerDefence {
	public class LevelControl: MonoBehaviour {
		private Level level;

		[SerializeField]
		private ModelSelector selector;
		[SerializeField]
		private ModelSelector selector_confirm;

		private GameObject placingObject;
		private MeshRenderer[] renderers;
		private Vector2Int placingSize;

		[SerializeField]
		private Material red;
		[SerializeField]
		private Material green;

		private bool LeftClick => Input.GetMouseButtonDown(0);
		private bool RightClick => Input.GetMouseButtonDown(1);

		private TowerInfo iconSelection;
		private Placement towerSelection;

		private void Start() {
			level = Game.Instance.level;
			selector.Mode = ModelSelector.ShowMode.None;
			UI.Instance.iconManager.OnSelectionChanged += (n) => {
				iconSelection = n?.Info;
				if(n == null && placingObject != null) {
					Destroy(placingObject);
				} else if(n != null) {
					GeneratePlacingObject(n.Info);
				}
			};
			selector_confirm.SetState(ModelSelector.ShowMode.None);
		}


		private void Update() {
			if(UI.Instance.pausePanel.Show) {
				return;
			}
			if(RightClick && placingObject != null) {
				UI.Instance.iconManager.DeselectAll();
				placingObject = null;
			} else if(RightClick && towerSelection != null) {
				DeselectTower();
			}
			Vector2Int coord = GetMouseCoord();

			if(level.CheckWithin(coord)) {
				coord = level.GetOriginCoord(coord);
				level.GetHeightAndSize(coord, out float height, out Vector2Int size);
				switch(level.CheckType(coord)) {
					case NodeType.Path:
						selector.SetState(ModelSelector.ShowMode.None);
						if(iconSelection != null) {
							placingObject.SetActive(false);
						}
						if(RightClick || LeftClick) {
							DeselectTower();
						}
						break;
					case NodeType.Placable:
						if(iconSelection == null) {
							selector.SetState(ModelSelector.ShowMode.Half, coord, height, size);
							if(RightClick || LeftClick) {
								DeselectTower();
							}
						} else {
							UpdatePlacing(coord);
							if(LeftClick) {
								level.EditNode(coord, iconSelection.id);
							}
						}
						break;
					case NodeType.Unplacable:
						if(iconSelection == null) {
							selector.SetState(ModelSelector.ShowMode.Full, coord, height, size);
							if(LeftClick) {
								Placement t = level.GetPlacement(coord);
								UI.Instance.placementPanelManager.Request(t);
								towerSelection = t;
								selector_confirm.SetState(ModelSelector.ShowMode.Full, coord, height, size);
							} else if(RightClick && towerSelection != null) {
								DeselectTower();
							}
						} else {
							UpdatePlacing(coord);
						}
						break;
					default:
						throw new Exception("?");
				}
			} else {//mouse outside of the level
				selector.SetState(ModelSelector.ShowMode.None);
				if(iconSelection != null) {
					placingObject.SetActive(false);
				}
				if((RightClick || LeftClick) && towerSelection != null) {
					DeselectTower();
				}
			}
		}

		public void DeselectTower(bool ignoreUIDection = false) {//in case of control through ui
			if(UI.Instance.placementPanelManager.HasMouseOnPanels() && !ignoreUIDection) {
				return;
			}
			UI.Instance.placementPanelManager.ClearRequest();
			towerSelection = null;
			selector_confirm.SetState(ModelSelector.ShowMode.None);
		}

		private void GeneratePlacingObject(TowerInfo info) {
			if(placingObject != null) {
				Destroy(placingObject);
			}
			GameObject obj = Instantiate(info.prefab, transform);
			placingObject = obj;
			placingSize = info.size;
			renderers = obj.GetComponentsInChildren<MeshRenderer>();
			obj.GetComponentsInChildren<MonoBehaviour>().ToList().ForEach(c => c.enabled = false);
			obj.SetActive(false);
			obj.transform.position = new Vector3(obj.transform.position.x, -0.29f, obj.transform.position.z);
			obj.transform.localScale = Vector3.one * 0.99f;
		}

		private void UpdatePlacing(Vector2Int coord) {
			bool c = level.Check(coord.x, coord.y, placingSize);
			placingObject.SetActive(true);
			placingObject.transform.position = new Vector3(coord.x, placingObject.transform.position.y, coord.y);
			foreach(var item in renderers) {
				var ms = new List<Material>();
				for(int i = 0; i < item.materials.Length; i++) {
					ms.Add(c ? green : red);
				}
				item.materials = ms.ToArray();
			}
		}

		private Vector2Int GetMouseCoord() {
			Vector3 pos = Game.GetMousePosition();
			int x = Mathf.RoundToInt(pos.x);
			int y = Mathf.RoundToInt(pos.z);
			return new Vector2Int(x, y);
		}

	}
}
