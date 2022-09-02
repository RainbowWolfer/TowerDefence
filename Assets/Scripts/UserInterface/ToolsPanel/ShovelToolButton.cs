using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence.UserInterface {
	public class ShovelToolButton: MonoBehaviour {
		[SerializeField]
		private Image background;
		[SerializeField]
		private TextMeshProUGUI nameText;
		[SerializeField]
		private TextMeshProUGUI priceText;
		[SerializeField]
		private TextMeshProUGUI sizeText;
		[SerializeField]
		private Image lockIcon;
		[SerializeField]
		private Image filler;

		[field: Space]
		[field: SerializeField]
		public bool IsMouseOn { get; private set; }
		[field: SerializeField]
		[field: Range(0, 1f)]
		public float Cooldown { get; set; }
		[field: SerializeField]
		public bool IsLocked { get; set; }

		private float cv1;
		private void Awake() {
			nameText.rectTransform.anchoredPosition = Vector2.zero;
		}

		private void Update() {
			IsMouseOn = UIRayCaster.HasElement(background.gameObject);

			nameText.rectTransform.anchoredPosition = new Vector2(
				Mathf.SmoothDamp(
					nameText.rectTransform.anchoredPosition.x,
					IsMouseOn ? -225 : 0,
					ref cv1, 0.1f
				),
				nameText.rectTransform.anchoredPosition.y
			);

			lockIcon.gameObject.SetActive(IsLocked);
			priceText.gameObject.SetActive(!IsLocked);
			sizeText.gameObject.SetActive(!IsLocked);

			filler.fillAmount = Cooldown;

			Color color;
			if(IsMouseOn) {
				color = new Color(0.439f, 0.587f, 0.886f);
			} else {
				color = new Color(0.439f, 0.484f, 0.886f);
			}
			filler.color = Color.Lerp(filler.color, color, Time.deltaTime * 5);
		}
	}
}
