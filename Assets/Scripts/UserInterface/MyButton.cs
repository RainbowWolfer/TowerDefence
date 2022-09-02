using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TowerDefence.UserInterface {
	public class MyButton: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {
		[HideInInspector]
		public RectTransform rectTransform;
		public Action OnEnter;
		public Action OnExit;
		public Action OnDown;
		public Action OnUp;

		[SerializeField]
		private Image background;
		[SerializeField]
		private Shadow shadow;
		[SerializeField]
		private TextMeshProUGUI content;

		public bool IsMouseOn { get; private set; }

		public bool enableSizeTransition;
		public Vector2 size;

		public bool enableSelection;

		private bool isSelected;
		public bool IsSelected {
			get => isSelected;
			set {
				isSelected = value;
				UpdateColor();
			}
		}

		public Color color_mouseOn = new Color(0.549f, 0.549f, 0.549f, 0.5f);//989898
		public Color color_mouseOff = new Color(0.415f, 0.415f, 0.415f, 0.5f);//6A6A6A
		public Color color_mouseDown = new Color(0.415f, 0.415f, 0.415f, 0.5f);
		public Color color_mouseOnSelected = new Color(0.803f, 0.803f, 0.803f, 0.5f);//CDCDCD
		public Color color_mouseOffSelected = new Color(0.643f, 0.643f, 0.643f, 0.5f);//A4A4A4

		[SerializeField]
		private float colorChangingSpeed = 5;
		private Color currentColor;
		private void Awake() {
			rectTransform = transform as RectTransform;
			currentColor = enableSelection && IsSelected ? color_mouseOffSelected : color_mouseOff;

			OnEnter += () => {
				currentColor = enableSelection && IsSelected ? color_mouseOnSelected : color_mouseOn;
			};
			OnExit += () => {
				currentColor = enableSelection && IsSelected ? color_mouseOffSelected : color_mouseOff;
			};

			OnDown += () => {
				if(enableSelection) {
					currentColor = color_mouseDown;
				}
				if(shadow != null) {
					shadow.enabled = false;
				}
			};
			OnUp += () => {
				if(enableSelection && !IsSelected) {
					IsSelected = true;
					currentColor = color_mouseOnSelected;
				} else if(enableSelection && IsSelected) {
					IsSelected = false;
					currentColor = color_mouseOn;
				}
				if(shadow != null) {
					shadow.enabled = transform;
				}
			};
		}

		private void UpdateColor() {
			if(IsMouseOn) {
				if(enableSelection && isSelected) {
					currentColor = color_mouseOnSelected;
				} else {
					currentColor = color_mouseOn;
				}
			} else {
				if(enableSelection && isSelected) {
					currentColor = color_mouseOffSelected;
				} else {
					currentColor = color_mouseOff;
				}
			}
		}

		public void OnPointerEnter(PointerEventData eventData) {
			IsMouseOn = true;
			OnEnter?.Invoke();
		}

		public void OnPointerExit(PointerEventData eventData) {
			IsMouseOn = false;
			OnExit?.Invoke();
		}
		public void OnPointerDown(PointerEventData eventData) {
			OnDown?.Invoke();
		}

		public void OnPointerUp(PointerEventData eventData) {
			OnUp?.Invoke();
		}

		private Vector2 cv1;
		private void Update() {
			background.color = Color.Lerp(
				background.color,
				currentColor,
				Time.deltaTime * colorChangingSpeed
			);

			if(enableSizeTransition) {
				rectTransform.sizeDelta = Vector2.SmoothDamp(
					rectTransform.sizeDelta,
					size,
					ref cv1,
					0.1f
				);
			}
		}

	}
}
