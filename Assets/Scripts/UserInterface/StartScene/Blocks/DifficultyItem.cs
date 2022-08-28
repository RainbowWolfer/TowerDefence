using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

namespace TowerDefence.UserInterface.StartScene.Blocks {
	public class DifficultyItem: MonoBehaviour {
		[SerializeField]
		private Image image;
		[SerializeField]
		private TextMeshProUGUI title;
		[SerializeField]
		private TextMeshProUGUI count;

		[field: Space]
		[field: SerializeField]
		public bool IsMouseOn { get; private set; }
		[field: SerializeField]
		public bool IsSelected { get; private set; }

		public Color mainColor;

		public DifficultyItem[] others;

		private void Update() {
			IsMouseOn = UIRayCaster.HasElement(image.gameObject);
			if(IsMouseOn && Input.GetMouseButtonUp(0)) {
				IsSelected = !IsSelected;
				foreach(DifficultyItem item in others) {
					item.IsSelected = false;
				}
			}

			Color c = mainColor;
			c.a = IsSelected ? 1 : (IsMouseOn ? 0.3f : 0f);
			image.color = c;

			title.fontStyle = IsSelected ? FontStyles.Bold : FontStyles.Normal;
		}
	}
}
