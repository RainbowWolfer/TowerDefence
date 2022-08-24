using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TowerDefence.UserInterface {
	public class PausePanel: MonoBehaviour {
		public RectTransform Rt => transform as RectTransform;

		[SerializeField]
		private Image menuButtonBackground;
		[SerializeField]
		private SettingsPanel settingsPanel;

		[field: Space]
		[field: SerializeField]
		public MouseDetector MouseDetector { get; private set; }

		private bool show;
		public bool Show {
			get => show;
			set {
				show = value;
				if(!show) {
					settingsPanel.IsSettingsOpened = false;
				}
			}
		}

		public bool IsSettingsOpened { get; set; }

		public float setttingsOffset = 375;

		private float cv1;

		private void Awake() {
			settingsPanel.openPanelIndexes = new int[] { 2 };
			settingsPanel.ButtonSelector.SetButtonsAction(
				() => {
					Show = false;
					settingsPanel.ButtonSelector.CancelSelection();
				},
				() => {
					settingsPanel.ShowSettings();
					settingsPanel.IsSettingsOpened = !settingsPanel.IsSettingsOpened;
				},
				() => {
					//exit to main menu
				}
			);
		}

		private void Update() {
			Color menuBackgroundColor;
			if(UIRayCaster.HasElement(menuButtonBackground.gameObject)) {
				menuBackgroundColor = new Color(0.6f, 0.6f, 0.6f, 0.6f);
				if(Input.GetMouseButtonUp(0)) {
					Show = !Show;
				}
			} else {
				menuBackgroundColor = new Color(0, 0, 0, 0.63f);
			}

			menuButtonBackground.color = Color.Lerp(
				menuButtonBackground.color,
				menuBackgroundColor,
				Time.deltaTime * 15
			);

			Rt.anchoredPosition = new Vector2(
				0,
				Mathf.SmoothDamp(
					Rt.anchoredPosition.y,
					Show ? 0 : Rt.sizeDelta.y,
					ref cv1,
					0.1f
				)
			);
		}
	}
}
