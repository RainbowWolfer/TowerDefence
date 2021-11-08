using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TowerDefence.UserInterface {
	public class SettingsPanel: MonoBehaviour {
		public RectTransform rectTransform => transform as RectTransform;
		[SerializeField]
		private PausePanel pausePanel;
		[SerializeField]
		private CanvasGroup canvasGroup;
		[SerializeField]
		private RectTransform group;
		[SerializeField]
		private VerticalLayoutGroup verticalLayoutGroup;
		public bool Show => pausePanel.IsSettingsOpened;

		public float MouseScroll => Input.mouseScrollDelta.y;
		private float scrollSpeed = 25;
		private float currentScroll = 0;

		private float cv1;
		private float cv2;
		private float cv3;
		private float cv4;
		private void Update() {
			rectTransform.sizeDelta = new Vector2(Mathf.SmoothDamp(rectTransform.sizeDelta.x, Show ? 725 : 0, ref cv1, 0.1f), 450);
			rectTransform.anchoredPosition = new Vector2(Mathf.SmoothDamp(rectTransform.anchoredPosition.x, Show ? 400 : 800, ref cv2, 0.1f), rectTransform.anchoredPosition.y);
			canvasGroup.alpha = Mathf.SmoothDamp(canvasGroup.alpha, Show ? 1 : 0, ref cv3, 0.1f);

			group.anchoredPosition = new Vector2(group.anchoredPosition.x, Mathf.SmoothDamp(group.anchoredPosition.y, currentScroll, ref cv4, 0.1f));

			float totalHeight = verticalLayoutGroup.padding.vertical + group.childCount * 130;//spacing = 0
			currentScroll += -MouseScroll * scrollSpeed;
			currentScroll = Mathf.Clamp(currentScroll, 0, totalHeight - rectTransform.sizeDelta
			.y);

		}
	}
}
