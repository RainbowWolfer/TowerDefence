using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefence.GameControl;
using TowerDefence.Placements;
using UnityEngine;

namespace TowerDefence.UserInterface {
	public abstract class PlacementPanel: MonoBehaviour {
		protected PlacementPanelManager manager;
		protected RectTransform rectTransform;
		protected Action<bool> OnShowChanged;

		private bool show;

		public bool Show {
			get => show;
			set {
				show = value;
				OnShowChanged?.Invoke(value);
			}
		}
		//public abstract Placement CurrentPlacement { get; }

		public abstract float Width { get; }

		[field: SerializeField]
		public MouseDetector MouseDetector { get; private set; }
		[SerializeField]
		private CanvasGroup canvas;

		public Placement TargetPlacement { get; private set; }

		public virtual void Initialize(Placement placement, PlacementPanelManager manager) {
			Show = true;
			TargetPlacement = placement;
			this.manager = manager;
		}

		protected virtual void Awake() {
			rectTransform = transform as RectTransform;
		}

		protected virtual void Start() {
			rectTransform.anchoredPosition = new Vector2(-Width, 0);
			rectTransform.localEulerAngles = new Vector3(0, 60, 0);
		}

		private float p_cv1;
		private float p_cv2;
		private float p_cv3;
		protected virtual void Update() {
			rectTransform.anchoredPosition = new Vector2(Mathf.SmoothDamp(
				rectTransform.anchoredPosition.x,
				Show ? 0 : -Width * 1.01f,
				ref p_cv1,
				0.1f
			), 0);
			rectTransform.sizeDelta = new Vector2(Mathf.SmoothDamp(
				rectTransform.sizeDelta.x,
				Width,
				ref p_cv2,
				0.1f
			), rectTransform.sizeDelta.y);

			rectTransform.localEulerAngles = new Vector3(0, Mathf.SmoothDampAngle(
				rectTransform.localEulerAngles.y,
				Show ? 0 : -90,
				ref p_cv3,
				0.1f
			), 0);

			canvas.alpha = Mathf.Lerp(
				0, 1,
				rectTransform.localEulerAngles.y / 90 + 1
			);
			if(!Show && rectTransform.anchoredPosition.x <= -Width) {
				manager.Remove(this);
				//if(TargetPlacement != null) {
				//	manager.Remove(TargetPlacement);
				//} else if(this is AbilityPanel ap && manager.CurrentAbilityPanel == ap) {
				//	manager.Remove(ap);
				//} else if(TargetPlacement == null) {
				//	manager.RemoveEmpty();
				//} else {
				//	throw new Exception($"Unhandled Panel Type: {this}");
				//}
			}
		}
	}
}
