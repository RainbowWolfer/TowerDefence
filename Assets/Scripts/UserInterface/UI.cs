using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TowerDefence.UserInterface.LevelIncomingPanel;
using TowerDefence.UserInterface.LevelPanels;

namespace TowerDefence.UserInterface {
	public class UI: MonoBehaviour {
		public static UI Instance { get; private set; }

		[SerializeField]
		private Canvas canvas;

		[field: SerializeField]
		public UIRayCaster RayCaster { get; private set; }

		public GameObject prefab_healthBar;

		[Space]
		public FpsCounter fpsCounter;
		public FlowIconManager flowIconManager;
		public IconManager iconManager;
		public PlacementPanelManager placementPanelManager;
		public FinancePanel financePanel;
		public PausePanel pausePanel;
		public IncomingPanel incomingPanel;
		public LevelPanel levelPanel;

		private void Awake() {
			Instance = this;
			//canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		}

		private void Start() {

		}

		private void Update() {
			if(Input.GetKeyDown(KeyCode.Escape)) {
				pausePanel.Show = !pausePanel.Show;
			}

		}

		public static Vector2 GetUIPosition(RectTransform rt) {
			static bool IsSecondParent(RectTransform t) => t.parent.GetComponent<UI>() != null;
			var pos = new Vector2(rt.localPosition.x, rt.localPosition.y);
			while(!IsSecondParent(rt)) {
				rt = rt.parent as RectTransform;
				pos += new Vector2(rt.localPosition.x, rt.localPosition.y);
			}
			return pos + new Vector2(Screen.width, Screen.height) / 2;
		}
	}
}
