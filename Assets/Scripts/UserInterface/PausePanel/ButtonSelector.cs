using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence.UserInterface {
	public class ButtonSelector: MonoBehaviour {
		[SerializeField]
		private PausePanel pausePanel;
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
		private MyButton resumeButton;
		[SerializeField]
		private MyButton settingsButton;
		[SerializeField]
		private MyButton exitButton;

		[SerializeField]
		private float gap = 25;

		private float y = -150;
		private float buttonWidth = 250;

		private const float expandWidth = 300;
		private const float originalWidth = 250;

		private void Awake() {
			resumeButton.OnUp += () => {
				pausePanel.Show = false;
			};
			settingsButton.OnUp += () => {
				pausePanel.IsSettingsOpened = !pausePanel.IsSettingsOpened;
			};
			exitButton.OnUp += () => {
				//exit to main menu
			};
		}

		private float cv1;
		private float cv2;
		private float cv3;
		private float cv4;
		private float cv5;
		private void Update() {
			rectTransform.anchoredPosition = new Vector2(0, Mathf.SmoothDamp(rectTransform.anchoredPosition.y, y, ref cv1, 0.1f));

			float parentWidth = pausePanel.Rt.sizeDelta.x;
			float v = (parentWidth - buttonWidth) / 2 - gap;

			lc.anchoredPosition = new Vector2(Mathf.SmoothDamp(lc.anchoredPosition.x, v, ref cv2, 0.1f), 0);
			rc.anchoredPosition = new Vector2(Mathf.SmoothDamp(rc.anchoredPosition.x, -v, ref cv3, 0.1f), 0);

			ll.sizeDelta = new Vector2(Mathf.SmoothDamp(ll.sizeDelta.x, v, ref cv4, 0.1f), 10);
			rl.sizeDelta = new Vector2(Mathf.SmoothDamp(rl.sizeDelta.x, v, ref cv5, 0.1f), 10);
			if(pausePanel.IsSettingsOpened) {
				y = -290;
				resumeButton.size.x = originalWidth;
				settingsButton.size.x = expandWidth;
				exitButton.size.x = originalWidth;
				gap = pausePanel.setttingsOffset + 50;
			} else {
				if(resumeButton.IsMouseOn) {
					y = -150;
					resumeButton.size.x = expandWidth;
					settingsButton.size.x = originalWidth;
					exitButton.size.x = originalWidth;
					gap = 50;
				} else if(settingsButton.IsMouseOn) {
					y = -290;
					resumeButton.size.x = originalWidth;
					settingsButton.size.x = expandWidth;
					exitButton.size.x = originalWidth;
					gap = 50;
				} else if(exitButton.IsMouseOn) {
					y = -430;
					resumeButton.size.x = originalWidth;
					settingsButton.size.x = originalWidth;
					exitButton.size.x = expandWidth;
					gap = 50;
				} else {
					resumeButton.size.x = originalWidth;
					settingsButton.size.x = originalWidth;
					exitButton.size.x = originalWidth;
					gap = 25;
				}
			}
		}
		private void OnValidate() {
			float parentWidth = pausePanel.Rt.sizeDelta.x;
			float v = (parentWidth - buttonWidth) / 2 - gap;
			lc.anchoredPosition = new Vector2(v, 0);
			rc.anchoredPosition = new Vector2(-v, 0);

			ll.sizeDelta = new Vector2(v, 10);
			rl.sizeDelta = new Vector2(v, 10);
		}
	}
}
