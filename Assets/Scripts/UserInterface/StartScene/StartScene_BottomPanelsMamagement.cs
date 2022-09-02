using UnityEngine;

namespace TowerDefence.UserInterface.StartScene {
	public class StartScene_BottomPanelsMamagement: MonoBehaviour {
		public StartScene_BottomPanel Panel => StartSceneUI.Instance.BottomPanel;

		[Space]
		[SerializeField]
		private RectTransform ui;
		[SerializeField]
		private RectTransform parent;

		[Space]
		[SerializeField]
		private RectTransform settings;
		[SerializeField]
		private CanvasGroup settingsCanvas;
		[SerializeField]
		private RectTransform user;
		[SerializeField]
		private CanvasGroup userCanvas;

		private float x = 0;
		private float a_settings = 0;
		private float a_user = 0;

		private float cv1;
		private float cv2;
		private float cv3;

		private void Start() {

		}

		private void Update() {
			float uiWidth = ui.sizeDelta.x;
			settings.anchoredPosition = new Vector2();
			user.anchoredPosition = new Vector2(uiWidth, 0);

			switch(Panel.panelType) {
				case StartScene_BottomPanel.PanelType.Settings:
					x = 0;
					a_settings = 1;
					a_user = 0;
					break;
				case StartScene_BottomPanel.PanelType.User:
					x = -uiWidth;
					a_settings = 0;
					a_user = 1;
					break;
				case StartScene_BottomPanel.PanelType.None:
				default:
					break;
			}

			parent.anchoredPosition = new Vector2(
				Mathf.SmoothDamp(parent.anchoredPosition.x, x, ref cv1, 0.1f)
			, 0);

			settingsCanvas.alpha = Mathf.SmoothDamp(settingsCanvas.alpha, a_settings, ref cv2, 0.1f);
			userCanvas.alpha = Mathf.SmoothDamp(userCanvas.alpha, a_user, ref cv3, 0.1f);

		}
	}
}
