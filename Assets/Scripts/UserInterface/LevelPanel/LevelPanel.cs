using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence.UserInterface.LevelPanels {
	public class LevelPanel: MonoBehaviour {
		[field: SerializeField]
		public Counter Escapes { get; private set; }
		[field: SerializeField]
		public Counter Waves { get; private set; }
		[field: SerializeField]
		public Counter Levels { get; private set; }

		[Space]
		[SerializeField]
		private Image progressFiller;
		[SerializeField]
		private TextMeshProUGUI progressText;
		//[SerializeField]
		//private Image

		[Space]
		private float percentage;
		private float textPer;

		private void Start() {
			ResetProgress();
			Waves.defaultPercentage = 0;
			Escapes.defaultPercentage = 1;
			Levels.defaultPercentage = 1;
		}

		private void Update() {
			progressFiller.fillAmount = Mathf.Lerp(progressFiller.fillAmount, percentage, Time.deltaTime * 1);
			textPer = Mathf.Lerp(textPer, percentage, Time.deltaTime * 1f);
			progressText.text = $"{Math.Round(textPer * 100, 0)}%";
		}

		public void ResetProgress() {
			progressFiller.fillAmount = 0;
			textPer = 0;
			progressText.text = "...";
		}


		public void UpdateProgress(float percentage) {
			this.percentage = percentage;
			//Debug.Log(percentage);
		}
	}
}
