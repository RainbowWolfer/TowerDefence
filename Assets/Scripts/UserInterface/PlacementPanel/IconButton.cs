using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using TowerDefence.Scripts.Data;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence.UserInterface {
	public class IconButton: MonoBehaviour {
		public string title;

		[SerializeField]
		private GameObject background;
		[SerializeField]
		private TextMeshProUGUI titleText;

		[field: SerializeField]
		public Image Icon { get; set; }
		[field: SerializeField]
		public Outline Outline { get; set; }
		[field: SerializeField]
		public TextMeshProUGUI Text { get; set; }


		public bool IsMouseOn { get; private set; }

		public Action<IconButton> OnClick { get; set; } = null;

		public Action<Image, Outline, TextMeshProUGUI> ExternalUpdate { get; set; }

		public Range<float> textAlpha = new Range<float>(0, 1);
		public Range<float> textY = new Range<float>(20, 0);
		public Range<Vector2> iconSize;
		private readonly float speed = 15;

		private void Start() {
			titleText.text = title;
		}

		private void Update() {
			IsMouseOn = UIRayCaster.HasElement(background);

			Outline.enabled = IsMouseOn;

			Color newColor = titleText.color;
			newColor.a = IsMouseOn ? textAlpha.to : textAlpha.from;
			titleText.color = Color.Lerp(titleText.color, newColor, Time.deltaTime * speed);
			titleText.rectTransform.anchoredPosition = Vector2.Lerp(
				titleText.rectTransform.anchoredPosition,
				new Vector2(0, IsMouseOn ? textY.to : textY.from),
				Time.deltaTime * speed
			);
			Icon.rectTransform.sizeDelta = Vector2.Lerp(
				Icon.rectTransform.sizeDelta,
				IsMouseOn ? iconSize.to : iconSize.from,
				Time.deltaTime * speed
			);


			if(IsMouseOn && Input.GetMouseButtonUp(0)) {
				OnClick?.Invoke(this);
			}

			ExternalUpdate?.Invoke(Icon, Outline, Text);
		}

	}
}
