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
			if(IsMouseOnButton && Input.GetMouseButtonUp(0)) {
				IsOn = !IsOn;
				handled = true;
			}

			IsMouseOn = UIRayCaster.HasElements(backgrounds);
			IsMouseOnButton = UIRayCaster.HasElements(buttonBackgrounds);

			Rt.anchoredPosition = new Vector2(Rt.anchoredPosition.x,
				Mathf.Lerp(
					Rt.anchoredPosition.y,
					IsOn ? 0 : Game.Instance.waves.IsOnGoing ? 150 : 120,
					Time.deltaTime * 10
				)
			);

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

			titleText.rectTransform.anchoredPosition = new Vector2(0,
				Mathf.Lerp(titleText.rectTransform.anchoredPosition.y, showTitle ? 0 : 150, Time.deltaTime * 10)
			);

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

		public void PopupLevel(int currentLevel, StageLevel level, float waitSeconds) {
			UpdateTitle($"LEVEL {currentLevel}");
			SetTimer(waitSeconds);
			IsOn = true;
			StartCoroutine(PopUpLevelCoroutine(waitSeconds));
		}

		private IEnumerator PopUpLevelCoroutine(float waitSeconds) {
			handled = false;
			showTitle = true;
			showCount = false;
			yield return new WaitForSeconds(waitSeconds * 0.3f);
			showTitle = false;
			showCount = true;
			yield return new WaitForSeconds(waitSeconds - waitSeconds * 0.3f);
			if(!handled) {
				IsOn = false;
			}
		}
	}
}
