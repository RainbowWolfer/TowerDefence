using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence.UserInterface.StartScene {
	public class AboutDetailButton: MonoBehaviour {
		[SerializeField]
		private Image background;
		[SerializeField]
		private Image icon;
		[SerializeField]
		private TextMeshProUGUI text;

		public Action OnClick { get; set; }

		public bool IsMouseOn { get; set; }

		private void Update() {
			IsMouseOn = UIRayCaster.HasElement(background.gameObject);

			if(IsMouseOn && OnClick != null) {
				//Cursor.SetCursor();
			} else {

			}

			if(IsMouseOn && Input.GetMouseButtonUp(0)) {
				OnClick?.Invoke();
			}

			background.color = Color.Lerp(background.color,
				IsMouseOn ? new Color(0.65f, 0.65f, 0.65f) : new Color(0.264f, 0.264f, 0.264f),
				Time.deltaTime * 15
			);

			text.color = Color.Lerp(text.color, IsMouseOn ? Color.black : Color.white, Time.deltaTime * 15);
			if(icon != null) {
				icon.color = text.color;
			}
		}
	}
}
