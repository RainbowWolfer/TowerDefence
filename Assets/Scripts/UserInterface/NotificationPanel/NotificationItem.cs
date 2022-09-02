using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence.UserInterface {
	public class NotificationItem: MonoBehaviour {
		[HideInInspector]
		public NotificationPanel parent;

		public RectTransform Rt => transform as RectTransform;

		[SerializeField]
		private CanvasGroup canvas;
		[SerializeField]
		private TextMeshProUGUI text;
		[SerializeField]
		private Image icon;
		[SerializeField]
		private Image background;

		public Vector2 targetPosotion = Vector2.zero;
		public bool fade = false;

		//private float maxY = 450;

		private float startTime;

		private void Start() {
			startTime = Time.time;
		}

		public void Set(string text, Sprite sprite, Color color) {
			this.text.text = text;
			icon.sprite = sprite;
			background.color = color;
		}

		private void Update() {
			Rt.anchoredPosition = Vector2.Lerp(Rt.anchoredPosition, targetPosotion, Time.deltaTime * 12);
			if(fade) {
				canvas.alpha = Mathf.Lerp(canvas.alpha, 0, Time.deltaTime * 10);
				if(canvas.alpha <= 0.1f) {
					parent.Remove(this);
				}
			}

			if(Time.time - startTime >= 3f) {
				fade = true;
				targetPosotion = new Vector2(0, NotificationPanel.DISAPPEAR_Y);
			}
		}
	}
}
