using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using TowerDefence.Functions;
using TowerDefence.GameControl.Waves;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence.UserInterface.LevelIncomingPanel {
	public class IncomingPanel: MonoBehaviour {
		[field: SerializeField]
		public bool IsMouseOn { get; private set; }

		[SerializeField]
		private GameObject[] backgrounds;
		private RectTransform Rt => transform as RectTransform;

		[field: SerializeField]
		public bool IsOn { get; private set; }
		[field: SerializeField]
		public bool ForceHide { get; set; }

		[SerializeField]
		private TextMeshProUGUI titleText;
		[SerializeField]
		private RectTransform countPanel;
		[SerializeField]
		private Image timerFiller;
		[SerializeField]
		private TextMeshProUGUI timerText;

		[Space]
		[SerializeField]
		private bool showTitle;
		[SerializeField]
		private bool showCount;


		[Space]
		[Header("Button")]
		[SerializeField]
		private GameObject[] buttonBackgrounds;
		[SerializeField]
		private RectTransform buttonArrow;

		[field: SerializeField]
		public bool IsMouseOnButton { get; private set; }

		private bool handled;

		private float timer_start;
		private float timer_duration;


		private void Update() {
			//click on expand button
			if(IsMouseOnButton && Input.GetMouseButtonUp(0)) {
				IsOn = !IsOn;
				handled = true;
			}

			IsMouseOn = UIRayCaster.HasElements(backgrounds);
			IsMouseOnButton = UIRayCaster.HasElements(buttonBackgrounds);

			//self transition
			Rt.anchoredPosition = new Vector2(Rt.anchoredPosition.x,
				Mathf.Lerp(
					Rt.anchoredPosition.y,
					IsOn ? 0 : !WavesManager.Instance.LevelGoing || WavesManager.Instance.IsSpawningEnemies ? 150 : 120,
					Time.deltaTime * 10
				)
			);

			//expand button
			buttonArrow.localRotation = Quaternion.Lerp(buttonArrow.localRotation,
				Quaternion.Euler(IsOn ? 180 : 0, 0, 0)
			, Time.deltaTime * 10);

			foreach(GameObject item in buttonBackgrounds) {
				if(item.TryGetComponent(out Image image)) {
					image.color = Color.Lerp(image.color,
						IsMouseOnButton ? new Color(0.5f, 0.5f, 0.5f, 0.95f) : new Color(0, 0, 0, 0.85f)
					, Time.deltaTime * 15);
				}
			}

			//showTitle
			titleText.rectTransform.anchoredPosition = new Vector2(0,
				Mathf.Lerp(titleText.rectTransform.anchoredPosition.y, showTitle ? 0 : 150, Time.deltaTime * 10)
			);


			//showCount
			countPanel.anchoredPosition = new Vector2(0,
				Mathf.Lerp(countPanel.anchoredPosition.y, showCount ? 0 : 150, Time.deltaTime * 10)
			);

			CalculateTimer();
		}

		private void CalculateTimer() {
			float offset = Time.time - timer_start;

			timerText.text = ((int)Mathf.Clamp(timer_duration - offset, 0, 9999)).FormatMinAndSec();
			timerFiller.fillAmount = 1 - offset / timer_duration;
		}

		public void SetTimer(float seconds) {
			timer_start = Time.time;
			timer_duration = seconds;

			timerText.text = ((int)seconds).FormatMinAndSec();
			timerFiller.fillAmount = 1;


		}

		public void UpdateTitle(string text) {
			titleText.text = $">> {text} <<";
		}

		public void UpdateCount() {

		}

		public async Task PopupStart() {
			UpdateTitle($"GET READY");
			IsOn = true;
			showTitle = true;
			showCount = false;
			SetTimer(4);
			await Task.Delay(4000);
			IsOn = false;
			await Task.Delay(700);
		}

		public async void PopupFinish() {
			UpdateTitle($"ALL CLEARED !");
			showTitle = true;
			showCount = false;
			IsOn = true;
			await Task.Delay(2000);
			IsOn = false;
		}

		public async void PopupLevel(int currentLevel, StageLevel level, float waitSeconds) {
			UpdateTitle($"LEVEL {currentLevel}");
			SetTimer(waitSeconds);
			IsOn = true;

			float w1 = waitSeconds * 0.3f;
			float w2 = waitSeconds - w1;

			handled = false;
			showTitle = true;
			showCount = false;
			await Task.Delay((int)(w1 * 1000));
			showTitle = false;
			showCount = true;
			await Task.Delay((int)(w2 * 1000));
			if(!handled) {
				IsOn = false;
			}
		}

	}
}
