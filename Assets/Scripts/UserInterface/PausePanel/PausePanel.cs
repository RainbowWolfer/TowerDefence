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
		[SerializeField]
		private Image menuButtonBackground;

		[field: Space]
		[field: SerializeField]
		public MouseDetector MouseDetector { get; private set; }

		[SerializeField]
		private ButtonSelector selector;

		[SerializeField]
		private RectTransform settingsPanel;
		[SerializeField]
		private RectTransform buttonsParent;

		public RectTransform Rt => transform as RectTransform;
		public bool Show { get; set; }
		public bool IsSettingsOpened { get; set; }

		public readonly float setttingsOffset = 375;

		private float cv1;
		private float cv2;
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
			menuButtonBackground.color = Color.Lerp(menuButtonBackground.color, menuBackgroundColor, Time.deltaTime * 15);

			Rt.anchoredPosition = new Vector2(0, Mathf.SmoothDamp(Rt.anchoredPosition.y, Show ? 0 : Rt.sizeDelta.y, ref cv1, 0.1f));

			buttonsParent.anchoredPosition = new Vector2(Mathf.SmoothDamp(buttonsParent.anchoredPosition.x, IsSettingsOpened ? -setttingsOffset : 0, ref cv2, 0.1f), buttonsParent.anchoredPosition.y);
		}
	}
}
