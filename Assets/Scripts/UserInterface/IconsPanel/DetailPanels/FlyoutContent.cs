using UnityEngine;

namespace TowerDefence.UserInterface.IconsPanel {
	public class FlyoutContent: MonoBehaviour {
		public RectTransform Rt => transform as RectTransform;

		[SerializeField]
		private RectTransform pointer;
		[SerializeField]
		private RectTransform content;

		public float offsetX;

		private void Update() {
			content.anchoredPosition = new Vector2(offsetX, content.anchoredPosition.y);
		}
	}
}
