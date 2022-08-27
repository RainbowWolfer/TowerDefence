using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence.UserInterface.StartScene {
	public class HorizontalScroll: MonoBehaviour {
		//public RectTransform Rt => transform as RectTransform;
		public bool EnableScroll { get; set; } = true;

		[SerializeField]
		private HorizontalLayoutGroup group;
		[SerializeField]
		private RectTransform ui;

		public RectTransform ScrollRt => group.transform as RectTransform;

		public float scrollSpeed = 100;
		public float ScrollDelta {
			get {
				Vector2 d = Input.mouseScrollDelta;
				//Input.GetTouch(0).;
				int a = d.y > 0 ? -1 : 1;
				return d.magnitude * scrollSpeed * a;
			}
		}

		[field: SerializeField]
		public float Scroll { get; set; }

		//left - right
		public Vector2 padding;

		private float cv1;

		private void Start() {
			Scroll = -padding.x;
			ScrollRt.anchoredPosition = new Vector2(0, ScrollRt.anchoredPosition.y);
		}

		private void Update() {
			//just make the children layout keep updating
			ScrollRt.sizeDelta = new Vector2(Mathf.Sin(Time.time), 0);

			//animation
			ScrollRt.anchoredPosition = new Vector2(
				Mathf.SmoothDamp(
					ScrollRt.anchoredPosition.x,
					-Scroll,
					ref cv1, 0.1f
				),
				ScrollRt.anchoredPosition.y
			);

			//action
			if(EnableScroll) {
				float width = GetWholeWidth() - ui.sizeDelta.x + padding.x + padding.y;
				Scroll = Mathf.Clamp(Scroll + ScrollDelta, -padding.x, width + padding.y);

				if(Input.touchCount != 0) {
					Touch touch = Input.GetTouch(0);
					//calculate offset to previous position
					Debug.Log(touch.position);
				} else {
					//previous position set to null
				}
			}
		}

		public float GetWholeWidth() {
			float width = 0;
			int count = group.transform.childCount;
			for(int i = 0; i < count; i++) {
				RectTransform rt = group.transform.GetChild(i) as RectTransform;
				width += rt.sizeDelta.x;
				if(i != count - 1) {
					width += group.spacing;
				}
			}
			return width;
		}
	}
}
