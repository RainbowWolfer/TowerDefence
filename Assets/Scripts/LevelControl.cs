using System;
using System.Collections.Generic;
using System.Linq;
using TowerDefence.Data;
using TowerDefence.GameControl;
using TowerDefence.Placements;
using TowerDefence.UserInterface;
using UnityEngine;
namespace TowerDefence {
	public class LevelControl: MonoBehaviour {
		private Level level;

		[SerializeField]
		private ModelSelector selector;
		[SerializeField]
		private ModelSelector selector_confirm;
		[SerializeField]
		private CircleRangeSelector circleRangeSelector;
		[SerializeField]
		private GapCircleRange attackRange;

		//place
		private GameObject placingObject;
		private MeshRenderer[] renderers;
		private Vector2Int placingSize;

		//ability
		private Emplacement abilityEmplacement;

		//shovel
		public bool IsInShovelTool;
		public float ShovelToolCooldown;

		[SerializeField]
		private Material red;
		[SerializeField]
		private Material green;

		private bool LeftClick => Input.GetMouseButtonDown(0);
		private bool RightClick => Input.GetMouseButtonDown(1);


		private TowerInfo iconSelection;

		public Placement TowerSelection { get; set; }

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
			TowerSelection = null;
		}

		private void UpdateIndicatorTarget() {
			if(TowerSelection is Tower tower) {
				attackRange.Show(tower);
				attackRange.AttackTargetIndicator.enemy = tower.Target;
			} else {
				attackRange.Show(null);
				attackRange.AttackTargetIndicator.enemy = null;
			}
		}

		private void Update() {
			if(UI.Instance.pausePanel.Show) {
				return;
			}
			if(UI.Instance.incomingPanel.IsMouseOn || UI.Instance.incomingPanel.IsMouseOnButton) {
				return;
			}
			UpdateIndicatorTarget();
			if(abilityEmplacement != null && abilityEmplacement.Ability != null) {//emplacement ability selecting mode
				circleRangeSelector.gameObject.SetActive(true);
				circleRangeSelector.radius = abilityEmplacement.Ability.AbilityRadius;
				if(RightClick) {
					StopAbility();
					return;
				}
				Vector2Int coord = GetMouseCoord();
				if(level.CheckWithin(coord)) {
					circleRangeSelector.position = coord;
					if(LeftClick) {
						//fire!
						abilityEmplacement.Fire(coord);

						//select tower
						float height = abilityEmplacement.info.height;
						Vector2Int size = abilityEmplacement.info.size;
						selector.SetState(ModelSelector.ShowMode.Full, abilityEmplacement.coord, height, size);
						UI.Instance.placementPanelManager.Request(abilityEmplacement);
						TowerSelection = abilityEmplacement;
						selector_confirm.SetState(ModelSelector.ShowMode.Full, abilityEmplacement.coord, height, size);

						abilityEmplacement = null;
					}
				} else {//mouse out of level
					if(LeftClick || RightClick) {
						StopAbility();
						return;
					}
				}

			} else if(IsInShovelTool) {//shovel tool mode
				circleRangeSelector.gameObject.SetActive(false);
				if(RightClick) {
					StopShovelTool();
					return;
				}
				Vector2Int coord = GetMouseCoord();
				if(level.CheckWithin(coord)) {
					//update indicator object 
					if(LeftClick) {
						
					}
				} else {//mouse out of level
					if(LeftClick || RightClick) {
						StopShovelTool();
						return;
					}
				}

			} else {//normal mode
				circleRangeSelector.gameObject.SetActive(false);
				if(RightClick && placingObject != null) {//in ui selecting
					UI.Instance.iconManager.DeselectAll();
					placingObject = null;
					return;
				} else if(RightClick && TowerSelection != null) {//in selected a field placement
					DeselectTower();
					return;
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
									Level.CheckForLowPowers();
									if(Level.Cash >= iconSelection.price) {
										level.EditNode(coord, iconSelection.id);
										Level.Cash -= iconSelection.price;
									} else {
										//pop up notification
										NotificationPanel.InsufficientFund();
									}
								}
							}
							break;
						case NodeType.Unplacable:
							if(iconSelection == null) {
								selector.SetState(ModelSelector.ShowMode.Full, coord, height, size);
								if(LeftClick) {
									Placement placement = level.GetPlacement(coord);
									UI.Instance.placementPanelManager.Request(placement);
									TowerSelection = placement;
									selector_confirm.SetState(ModelSelector.ShowMode.Full, coord, height, size);
								} else if(RightClick && TowerSelection != null) {
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
					if((RightClick || LeftClick) && TowerSelection != null) {
						DeselectTower();
					}
				}
			}
		}

		public void DeselectTower(bool ignoreUIDection = false) {//in case of control through ui
			if(!ignoreUIDection && UI.Instance.placementPanelManager.HasMouseOnPanels()) {
				return;
			}
			UI.Instance.placementPanelManager.ClearRequest();
			TowerSelection = null;
			selector_confirm.SetState(ModelSelector.ShowMode.None);
		}

		public void StartAbility(Emplacement emplacement) {
			abilityEmplacement = emplacement;
			//emplacement.LightUp();
			DeselectTower();
		}

		public void StopAbility() {
			abilityEmplacement = null;
			DeselectTower();
		}

		public void StartShovelTool(int size) {

		}

		public void StopShovelTool() {

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
