using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence.UserInterface {
	public class BonusChecker: MonoBehaviour {
		[SerializeField]
		private RectTransform parent;
		[SerializeField]
		private CanvasGroup canvas;
		[SerializeField]
		private Image checker;
		[SerializeField]
		private TextMeshProUGUI text;

		[Space]
		[SerializeField]
		private Sprite uncheckImage;
		[SerializeField]
		private Sprite checkImage;

		private void Start() {
			parent.anchoredPosition = new Vector2(350, 0);
			canvas.alpha = 0;
		}

		private void Update() {
			parent.anchoredPosition = new Vector2(
				Mathf.Lerp(parent.anchoredPosition.x, 0, Time.deltaTime * 5)
			, 0);
			canvas.alpha = Mathf.Lerp(canvas.alpha, 1, Time.deltaTime * 5);
		}

		public void SetContent(string content) {
			text.text = content;
		}

		public void Check(bool b) {
			checker.sprite = b ? checkImage : uncheckImage;
		}

	}
}
