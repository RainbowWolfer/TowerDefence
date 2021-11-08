using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using TowerDefence.Data;

namespace TowerDefence.UserInterface {
	public class Icon: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
		private IconManager manager;
		public TowerInfo Info { get; private set; }

		[SerializeField]
		private TextMeshProUGUI nameText;
		[SerializeField]
		private TextMeshProUGUI priceText;

		[SerializeField]
		private RectTransform descriptionPanel;
		[SerializeField]
		private TextMeshProUGUI descriptionText;

		[SerializeField]
		private Image bgImg;
		[SerializeField]
		private Image iconImg;

		[SerializeField]
		private bool isMouseOn;

		private bool isSelected;
		public bool IsSelected {
			get => isSelected;
			set {
				isSelected = value;
				bgImg.color = value ? Color.red : Color.white;
			}
		}

		private Vector2 position;
		private float offsetY;

		private bool disappearing = false;

		private float cv1;
		private float cv2;

		public void Initialize(float posX, TowerInfo info, IconManager manager) {
			this.Info = info;
			this.manager = manager;
			UpdateDisplay();
			(transform as RectTransform).anchoredPosition = new Vector2(posX, -100);
			position = new Vector2(posX, 100);
			offsetY = 0;
		}

		private void Update() {
			var rt = transform as RectTransform;
			rt.anchoredPosition = new Vector2(position.x,
				Mathf.SmoothDamp(rt.anchoredPosition.y, position.y + offsetY, ref cv1, 0.1f)
			);

			offsetY = isMouseOn ? 20 : 0;

			float desiredY = isMouseOn ? 0 : -descriptionPanel.sizeDelta.y;
			descriptionPanel.anchoredPosition = new Vector2(0,
				Mathf.SmoothDamp(descriptionPanel.anchoredPosition.y, desiredY, ref cv2, 0.1f)
			);

			if(disappearing && (transform as RectTransform).anchoredPosition.y <= -99) {
				manager.Remove(this);
			}
		}

		private void UpdateDisplay() {
			if(Info == null) {
				return;
			}
			nameText.text = string.IsNullOrEmpty(Info.TowerName) ? "Undefined" : Info.TowerName;
			priceText.text = $"${Info.price}";
			iconImg.sprite = Info.icon;
		}

		public void Disappear() {
			position.y = -100;
			disappearing = true;
		}

		public void OnPointerEnter(PointerEventData eventData) {
			isMouseOn = true;
		}

		public void OnPointerExit(PointerEventData eventData) {
			isMouseOn = false;
		}

		public void OnPointerClick(PointerEventData eventData) {
			manager.Select(this);
		}
	}
}
