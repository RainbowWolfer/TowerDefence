﻿using System;
using TMPro;
using TowerDefence.Data;
using TowerDefence.UserInterface.IconsPanel;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence.UserInterface {
	public abstract class SelectionBaseIcon: MonoBehaviour {
		private IconManager manager;

		public RectTransform Rt => transform as RectTransform;

		[SerializeField]
		private GameObject background;
		[SerializeField]
		private TextMeshProUGUI nameText;
		[SerializeField]
		private TextMeshProUGUI priceText;

		[Space]
		[SerializeField]
		private RectTransform descriptionPanel;
		[SerializeField]
		private CanvasGroup descriptionCanvas;

		[Space]
		[SerializeField]
		private Image backgroundImage;
		[SerializeField]
		private Image iconImage;
		private bool isSelected;
		public bool IsSelected {
			get => isSelected;
			set {
				isSelected = value;
				backgroundImage.color = value ? Color.red : Color.white;
			}
		}
		public bool CashAvailable => Level.Cash >= Info.price;

		public bool IsMouseOn { get; private set; }

		[field: Space]
		[field: SerializeField]
		public FlyoutContent Flyout { get; set; }

		private Vector2 position;
		private float offsetY;

		private bool disappearing = false;

		private float cv1;
		private float cv2;

		private float tooltipTime;
		private float tooltipTimeThreshold = 0.5f;

		public Action<TowerInfo> OnInfoUpdated { get; protected set; }
		private TowerInfo info;
		public TowerInfo Info {
			get => info;
			private set {
				info = value;
				OnInfoUpdated?.Invoke(value);
			}
		}

		public bool ShowToolTip => tooltipTime > tooltipTimeThreshold;

		private const float DESCRIPTION_TARGET_HEIGHT = 395;

		protected virtual void Awake() {

		}

		protected virtual void Start() {
			descriptionPanel.sizeDelta = new Vector2(descriptionPanel.sizeDelta
				.x, 0);
			descriptionCanvas.alpha = 0;
		}

		public void Initialize(float posX, TowerInfo info, IconManager manager) {
			this.manager = manager;
			Info = info;
			UpdateDisplay();
			Rt.anchoredPosition = new Vector2(posX, -100);
			position = new Vector2(posX, 100);
			offsetY = 0;
		}

		protected virtual void Update() {
			IsMouseOn = UIRayCaster.HasElement(background);
			if(IsMouseOn) {
				tooltipTime += Time.deltaTime;
			} else {
				tooltipTime = 0;
			}

			Rt.anchoredPosition = new Vector2(position.x,
				Mathf.SmoothDamp(Rt.anchoredPosition.y, position.y + offsetY, ref cv1, 0.1f)
			);

			offsetY = IsMouseOn ? 20 : 0;

			float desiredHeight = IsMouseOn && ShowToolTip ? DESCRIPTION_TARGET_HEIGHT : 0;
			descriptionPanel.sizeDelta = new Vector2(descriptionPanel.sizeDelta.x,
				Mathf.SmoothDamp(descriptionPanel.sizeDelta.y, desiredHeight, ref cv2, 0.05f)
			);
			descriptionCanvas.alpha = Mathf.Lerp(0, 1, descriptionPanel.sizeDelta.y / DESCRIPTION_TARGET_HEIGHT);

			if(disappearing && (transform as RectTransform).anchoredPosition.y <= -99) {
				manager.Remove(this);
			}

			priceText.color = CashAvailable ? Color.white : Color.red;

			if(IsMouseOn && Input.GetMouseButtonUp(0)) {
				manager.Select(this);
			}
		}

		private void UpdateDisplay() {
			if(Info == null) {
				return;
			}
			nameText.text = string.IsNullOrEmpty(Info.TowerName) ? "Undefined" : Info.TowerName;
			priceText.text = $"${Info.price}";
			iconImage.sprite = Info.icon;
		}

		public void Disappear() {
			position.y = -100;
			disappearing = true;
		}
	}
}