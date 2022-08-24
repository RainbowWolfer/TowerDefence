using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence.UserInterface {
	public class SettingsPanel: MonoBehaviour {
		public RectTransform Rt => transform as RectTransform;

		[SerializeField]
		private RectTransform buttonsParent;
		[SerializeField]
		private CanvasGroup settingsCanvas;
		[SerializeField]
		private RectTransform settingsRt;
		[SerializeField]
		private VerticalLayoutGroup settingsGroup;
		[SerializeField]
		private RectTransform settingsGroupRt;

		[field: Space]
		[field: SerializeField]
		public ButtonSelector ButtonSelector { get; private set; }
		[SerializeField]
		private GameObject settingsObj;
		[SerializeField]
		private GameObject aboutObj;


		private bool isSettingsOpened;
		public bool IsSettingsOpened {
			get => isSettingsOpened;
			set {
				isSettingsOpened = value;
				if(!IsSettingsOpened) {
					ButtonSelector.CancelSelection();
				}
			}
		}

		//1,2,3
		public int[] openPanelIndexes;


		[Space]
		public float setttingsOffset = 375;
		public float width = 1200;

		[SerializeField]
		private float scrollSpeed = 45;
		[SerializeField]
		private float currentScroll = 0;

		private float cv1;
		private float cv2;
		private float cv3;
		private float cv4;

		private void Update() {
			buttonsParent.anchoredPosition = new Vector2(
				Mathf.SmoothDamp(
					buttonsParent.anchoredPosition.x,
					IsSettingsOpened ? -setttingsOffset : 0,
					ref cv1,
					0.1f
				),
				buttonsParent.anchoredPosition.y
			);

			settingsCanvas.alpha = Mathf.SmoothDamp(
				settingsCanvas.alpha,
				IsSettingsOpened ? 1 : 0,
				ref cv2,
				0.1f
			);

			settingsRt.sizeDelta = new Vector2(
				Mathf.SmoothDamp(settingsRt.sizeDelta.x,
					 IsSettingsOpened ? 725 : 0,
					 ref cv4,
					 0.1f
				),
				450
			);

			if(IsShowingSettings()) {

				settingsGroupRt.anchoredPosition = new Vector2(
					settingsGroupRt.anchoredPosition.x,
					Mathf.SmoothDamp(
						settingsGroupRt.anchoredPosition.y,
						currentScroll,
						ref cv3,
						0.1f
					)
				);

				float totalHeight = settingsGroup.padding.vertical + settingsGroupRt.childCount * 130;//spacing = 0

				currentScroll += -Input.mouseScrollDelta.y * scrollSpeed;
				currentScroll = Mathf.Clamp(currentScroll, 0, totalHeight - settingsRt.sizeDelta.y);
			}
		}

		public void ShowSettings() {
			settingsObj.SetActive(true);
			aboutObj?.SetActive(false);
			currentScroll = 0;
		}

		public void ShowAbout() {
			settingsObj.SetActive(false);
			aboutObj?.SetActive(true);
		}

		public bool IsShowingSettings() => settingsObj.activeSelf;
		public bool IsShowingAbout() => aboutObj?.activeSelf ?? false;

		public void CalculatePanelOpen() {
			IsSettingsOpened = openPanelIndexes.Contains(ButtonSelector.currentSelection);
		}
	}
}
