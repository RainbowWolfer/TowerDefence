using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
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

		private void Update() {
			IsMouseOn = UIRayCaster.HasElement(background);

			border.color = Color.Lerp(
				border.color,
				IsMouseOn ? new Color(0.8f, 0.8f, 0.8f) : Color.white,
				Time.deltaTime * 5
			);


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
		}
	}
}
