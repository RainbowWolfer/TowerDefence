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
			Rt.anchoredPosition = new Vector2(0, Mathf.SmoothDamp(Rt.anchoredPosition.y, Show ? 0 : Rt.sizeDelta.y, ref cv1, 0.1f));

			buttonsParent.anchoredPosition = new Vector2(Mathf.SmoothDamp(buttonsParent.anchoredPosition.x, IsSettingsOpened ? -setttingsOffset : 0, ref cv2, 0.1f), buttonsParent.anchoredPosition.y);
		}
	}
}
