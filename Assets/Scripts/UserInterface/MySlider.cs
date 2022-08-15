using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TowerDefence.UserInterface {
	public class MySlider: MonoBehaviour {
		[SerializeField]
		private TextMeshProUGUI titleText;
		[SerializeField]
		private TextMeshProUGUI contentText;
		[SerializeField]
		private Image fillerImg;
		[SerializeField]
		private Image handleImg;
		[SerializeField]
		private Outline handleOutline;
		[SerializeField]
		private PointEventHandler handle;

		[SerializeField]
		private MySliderContent content;

		[field: SerializeField]
		public bool IsMouseOn { get; private set; }

		[SerializeField]
		private bool drag;

		public string title;
		public int currrentValue;

		private void Awake() {
			handle.MouseEnter += s => IsMouseOn = true;
			handle.MouseExit += s => IsMouseOn = false;
			handle.MouseDown += s => {
				drag = true;
			};
			handle.MouseUp += s => {
				drag = false;
			};
		}

		private void Start() {
			UpdateLayout();
		}

		private void Update() {
			// (725 - 100) / 2 = 317.5
			if(drag) {
				Vector2 pos = UI.GetUIPosition(transform as RectTransform);
				Vector2 direction = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - pos;
				float x = Mathf.Clamp(direction.x, -312.5f, 312.5f);
				float percentage = (x + 312.5f) / 625f;

				string text;
				switch(content.type) {
					case MySliderContent.ContentType.Number:
						int current = (int)((content.max - content.min) * percentage);
						text = current.ToString();
						break;
					case MySliderContent.ContentType.Percentage:
						//Debug.Log(percentage);
						text = $"{(int)(percentage * 100)}%";
						break;
					case MySliderContent.ContentType.Content:
						if(!content.ContentAvailable) {
							throw new Exception("content not found");
						}
						string[] c = content.content;
						var ps = new List<float>();
						for(int i = 0; i < c.Length; i++) {
							ps.Add((float)i / (c.Length - 1));
						}
						float fixedPercentage = ps.First();
						int index = 0;
						for(int i = 1; i < ps.Count; i++) {
							if(Mathf.Abs(percentage - ps[i]) < Mathf.Abs(percentage - fixedPercentage)) {
								fixedPercentage = ps[i];
								index = i;
							}
						}
						percentage = fixedPercentage;
						x = Mathf.Lerp(-312.5f, 312.5f, percentage);
						text = $"{c[index]}";
						break;
					default:
						throw new Exception("type not found");
				}

				contentText.text = text;
				handleImg.rectTransform.anchoredPosition = new Vector2(x, handleImg.rectTransform.anchoredPosition.y);

				fillerImg.fillAmount = percentage;
			}
			handleOutline.enabled = IsMouseOn || drag;
		}

		private void UpdateLayout() {
			this.name = $"MySlider_{title}";
			titleText.text = title;
		}

		public void Initialize(int i) {
			switch(content.type) {
				case MySliderContent.ContentType.Number:

					break;
				case MySliderContent.ContentType.Percentage:

					break;
				case MySliderContent.ContentType.Content:

					break;
				default:
					throw new Exception("type not found");
			}
		}

		private void OnValidate() {
			UpdateLayout();
		}

		[Serializable]
		private class MySliderContent {
			public ContentType type;

			public int min = 0;
			public int max = 100;
			[Range(1, 50)]
			public int gap = 1;

			public string[] content;
			public bool ContentAvailable => content.Length > 0;

			public enum ContentType {
				Number, Percentage, Content
			}
		}

	}
}
