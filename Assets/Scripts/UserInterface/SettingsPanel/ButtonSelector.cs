using System;
using UnityEngine;

namespace TowerDefence.UserInterface {
	public class ButtonSelector: MonoBehaviour {
		[SerializeField]
		private SettingsPanel parent;
		public RectTransform rectTransform => transform as RectTransform;

		[SerializeField]
		private RectTransform ll;
		[SerializeField]
		private RectTransform rl;
		[SerializeField]
		private RectTransform lc;
		[SerializeField]
		private RectTransform rc;

		[SerializeField]
		private MyButton button1;
		[SerializeField]
		private MyButton button2;
		[SerializeField]
		private MyButton button3;

		[SerializeField]
		private float gap = 25;

		private float y = -150;
		private float buttonWidth = 250;

		private const float expandWidth = 300;
		private const float originalWidth = 250;

		public int currentSelection;

		public void SetButtonsAction(Action a, Action b, Action c) {
			button1.OnUp = () => {
				currentSelection = currentSelection == 1 ? 0 : 1;
				a?.Invoke();
				parent.CalculatePanelOpen();
			};
			button2.OnUp = () => {
				currentSelection = currentSelection == 2 ? 0 : 2;
				b?.Invoke();
				parent.CalculatePanelOpen();
			};
			button3.OnUp = () => {
				currentSelection = currentSelection == 3 ? 0 : 3;
				c?.Invoke();
				parent.CalculatePanelOpen();
			};
		}

		public void CancelSelection() {
			currentSelection = 0;
		}

		private float cv1;
		private float cv2;
		private float cv3;
		private float cv4;
		private float cv5;
		private void Update() {
			rectTransform.anchoredPosition = new Vector2(0, Mathf.SmoothDamp(rectTransform.anchoredPosition.y, y, ref cv1, 0.1f));

			float parentWidth = parent.width;
			float v = (parentWidth - buttonWidth) / 2 - gap;

			lc.anchoredPosition = new Vector2(Mathf.SmoothDamp(lc.anchoredPosition.x, v, ref cv2, 0.1f), 0);
			rc.anchoredPosition = new Vector2(Mathf.SmoothDamp(rc.anchoredPosition.x, -v, ref cv3, 0.1f), 0);

			ll.sizeDelta = new Vector2(Mathf.SmoothDamp(ll.sizeDelta.x, v, ref cv4, 0.1f), 10);
			rl.sizeDelta = new Vector2(Mathf.SmoothDamp(rl.sizeDelta.x, v, ref cv5, 0.1f), 10);
			switch(currentSelection) {
				case 1:
					y = -150;
					button1.size.x = expandWidth;
					button2.size.x = originalWidth;
					button3.size.x = originalWidth;
					gap = parent.setttingsOffset + 50;
					break;
				case 2:
					y = -290;
					button1.size.x = originalWidth;
					button2.size.x = expandWidth;
					button3.size.x = originalWidth;
					gap = parent.setttingsOffset + 50;
					break;
				case 3:
					y = -430;
					button1.size.x = originalWidth;
					button2.size.x = originalWidth;
					button3.size.x = expandWidth;
					gap = parent.setttingsOffset + 50;
					break;
				default:
					if(button1.IsMouseOn) {
						y = -150;
						button1.size.x = expandWidth;
						button2.size.x = originalWidth;
						button3.size.x = originalWidth;
						gap = 50;
					} else if(button2.IsMouseOn) {
						y = -290;
						button1.size.x = originalWidth;
						button2.size.x = expandWidth;
						button3.size.x = originalWidth;
						gap = 50;
					} else if(button3.IsMouseOn) {
						y = -430;
						button1.size.x = originalWidth;
						button2.size.x = originalWidth;
						button3.size.x = expandWidth;
						gap = 50;
					} else {
						button1.size.x = originalWidth;
						button2.size.x = originalWidth;
						button3.size.x = originalWidth;
						gap = 25;
					}
					break;
			}
		}
		//private void OnValidate() {
		//	float parentWidth = pausePanel.Rt.sizeDelta.x;
		//	float v = (parentWidth - buttonWidth) / 2 - gap;
		//	lc.anchoredPosition = new Vector2(v, 0);
		//	rc.anchoredPosition = new Vector2(-v, 0);

		//	ll.sizeDelta = new Vector2(v, 10);
		//	rl.sizeDelta = new Vector2(v, 10);
		//}

	}
}
