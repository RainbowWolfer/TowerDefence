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
		public RectTransform rectTransform => transform as RectTransform;
		public bool Show { get; set; }
		[SerializeField]
		private ButtonSelector selector;

		public bool IsSettingsOpened { get; set; }

		[SerializeField]
		public RectTransform settingsPanel;
		[SerializeField]
		private RectTransform buttonsParent;

		[SerializeField]
		private Image bg;

		public bool HasMouseOn => UI.IsContact(bg.transform as RectTransform);
		public readonly float setttingsOffset = 375;

		private float cv1;
		private float cv2;
		private void Update() {
			rectTransform.anchoredPosition = new Vector2(0, Mathf.SmoothDamp(rectTransform.anchoredPosition.y, Show ? 0 : rectTransform.sizeDelta.y, ref cv1, 0.1f));

			buttonsParent.anchoredPosition = new Vector2(Mathf.SmoothDamp(buttonsParent.anchoredPosition.x, IsSettingsOpened ? -setttingsOffset : 0, ref cv2, 0.1f), buttonsParent.anchoredPosition.y);
		}
	}
}
