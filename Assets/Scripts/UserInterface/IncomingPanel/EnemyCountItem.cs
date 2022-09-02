using TMPro;
using TowerDefence.Data;
using TowerDefence.GameControl.Waves;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence.UserInterface.LevelIncomingPanel {
	public class EnemyCountItem: MonoBehaviour {
		[SerializeField]
		private GameObject background;
		[SerializeField]
		private Image border;
		[SerializeField]
		private Image content;
		[SerializeField]
		private TextMeshProUGUI countText;
		[SerializeField]
		private RectTransform detail;
		[SerializeField]
		private CanvasGroup detailCanvas;

		[Space]
		[SerializeField]
		private TextMeshProUGUI nameText;
		[SerializeField]
		private Image image;
		[SerializeField]
		private TextMeshProUGUI descriptionText;
		[SerializeField]
		private TextMeshProUGUI healthText;
		[SerializeField]
		private TextMeshProUGUI speedText;
		[SerializeField]
		private TextMeshProUGUI coinsText;

		[Space]
		[SerializeField]
		private Sprite cube;
		[SerializeField]
		private Sprite robot;
		[SerializeField]
		private Sprite hummer;
		[SerializeField]
		private Sprite apc;
		[SerializeField]
		private Sprite tank;

		[field: Space]
		[field: SerializeField]
		public bool IsMouseOn { get; set; }

		private void Start() {
			detail.sizeDelta = new Vector2(detail.sizeDelta.x, 0);
		}

		private void Update() {
			IsMouseOn = UIRayCaster.HasElement(background);

			border.color = Color.Lerp(
				border.color,
				IsMouseOn ? new Color(0.8f, 0.8f, 0.8f) : Color.white,
				Time.deltaTime * 5
			);

			detail.sizeDelta = new Vector2(detail.sizeDelta.x, Mathf.Lerp(
				detail.sizeDelta.y,
				IsMouseOn ? 400 : 0,
				Time.deltaTime * 15
			));
			detailCanvas.alpha = Mathf.Lerp(0, 1, detail.sizeDelta.y / 400);
		}

		public void Set(EnemyType type, int count) {
			content.sprite = type switch {
				EnemyType.Cube => cube,
				EnemyType.Robot => robot,
				EnemyType.Hummer => hummer,
				EnemyType.APC => apc,
				EnemyType.Tank => tank,
				_ => null,
			};
			countText.text = $"{count}";

			EnemyInfo info = Game.Instance.Enemies.RequestByType(type);
			nameText.text = info.name;
			image.sprite = content.sprite;
			descriptionText.text = info.description;
			healthText.text = $"{info.health.from} - {info.health.to}";
			speedText.text = $"{info.speed.from} - {info.speed.to}";
			coinsText.text = $"{info.coins.from} - {info.coins.to}";
		}
	}
}
