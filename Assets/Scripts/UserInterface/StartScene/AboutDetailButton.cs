using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using TowerDefence.Scripts.Data;
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

		public Range<Color> backgroundColorRange = new Range<Color>(new Color(0.65f, 0.65f, 0.65f), new Color(0.264f, 0.264f, 0.264f));
		public Range<Color> foregroundColorRange = new Range<Color>(Color.black, Color.white);

		private Color foregroundColor;

		private void Start() {
			foregroundColor = text?.color ?? icon?.color ?? Color.white;
		}

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
				IsMouseOn ? backgroundColorRange.from : backgroundColorRange.to,
				Time.deltaTime * 15
			);

			foregroundColor = Color.Lerp(
				foregroundColor,
				IsMouseOn ? foregroundColorRange.from : foregroundColorRange.to,
				Time.deltaTime * 15
			);

			if(text != null) {
				text.color = foregroundColor;
			}
			if(icon != null) {
				icon.color = foregroundColor;
			}
		}

		public void SetText(object text) {
			if(this.text == null) {
				return;
			}
			this.text.text = text?.ToString() ?? "Null";
		}
	}
}
