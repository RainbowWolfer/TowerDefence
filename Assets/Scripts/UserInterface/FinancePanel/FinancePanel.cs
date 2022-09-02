using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence.UserInterface {
	public class FinancePanel: MonoBehaviour {
		[SerializeField]
		private TextMeshProUGUI cashText;
		[SerializeField]
		private Image cashImage;

		[Space]
		private float displayCash;
		private int targetCash;

		private float cv1;

		public float speedThreshold = 500;
		public float maxScale = 1.3f;

		public void UpdateCash(int cash) {
			targetCash = cash;
		}

		private void Update() {
			//Debug.Log(cv1);
			displayCash = Mathf.SmoothDamp(displayCash, targetCash, ref cv1, 0.2f);
			cashText.text = $"{Mathf.RoundToInt(displayCash)}";

			cashImage.transform.localScale = Vector3.one * Mathf.Clamp((maxScale - 1) / speedThreshold * cv1 + 1, 1, maxScale);
			cashImage.color = new Color(1,
				Mathf.Lerp(0.85f, 1f, (cashImage.transform.localScale.x - 1) / (maxScale - 1))
			, 0, 1);
			cashText.color = cashImage.color;
		}
	}
}
