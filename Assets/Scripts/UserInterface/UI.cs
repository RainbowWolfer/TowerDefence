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

namespace TowerDefence.UserInterface {
	public class UI: MonoBehaviour {
		public static UI Instance;

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

		//public static bool HasMouseOnUI(){
		//	return IsContact;
		//}



		public static Vector3 GetScreenPosition(Vector3 worldPosition) {
			Vector3 desirePos = CameraController.Instance.mainCamera.WorldToScreenPoint(worldPosition) - new Vector3(Screen.width / 2, Screen.height / 2, 0);
			//bool isInScreen = Mathf.Abs(desirePos.x) < Screen.width / 2 && Mathf.Abs(desirePos.y) < Screen.height / 2 && desirePos.z > 0;
			return desirePos;
		}

		public static bool IsContact(RectTransform rt) {
			Vector2 mp = Input.mousePosition;
			Vector2 size = rt.sizeDelta;
			if(rt.anchorMin != rt.anchorMax) {
				if(rt.anchorMin.x == 0 && rt.anchorMax.x == 1) {
					size.x = Screen.width + rt.sizeDelta.x;
				}
				if(rt.anchorMin.y == 0 && rt.anchorMax.y == 1) {
					size.y = Screen.height + rt.sizeDelta.y;
				}
			}
			//Vector2 pos = rt.position;// the ui canvas must be in screen space - world
			Vector2 pos = GetUIPosition(rt);
			Vector2 offset = Game.MultiplyVectors((rt.pivot - new Vector2(0.5f, 0.5f)), size);
			pos -= offset;
			Vector2 d = pos - mp;

			bool bx = Mathf.Abs(d.x) < Mathf.Abs(size.x / 2);
			bool by = Mathf.Abs(d.y) < Mathf.Abs(size.y / 2);
			return bx && by;
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
