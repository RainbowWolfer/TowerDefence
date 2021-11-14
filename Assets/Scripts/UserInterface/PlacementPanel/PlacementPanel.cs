using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefence.GameControl;
using TowerDefence.Towers;
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
		public abstract Placement CurrentPlacement { get; }

		public abstract float Width { get; }

		[SerializeField]
		private CanvasGroup canvas;

		[SerializeField]
		private PointEventHandler topHandler;
		[SerializeField]
		private PointEventHandler botHandler;

		public bool IsMouseOnPanel => UI.IsContact(topHandler.transform as RectTransform) || UI.IsContact(botHandler.transform as RectTransform);

		public virtual void Initialize(Placement placement, PlacementPanelManager manager) {
			Show = true;
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
			rectTransform.anchoredPosition = new Vector2(Mathf.SmoothDamp(rectTransform.anchoredPosition.x, Show ? 0 : -Width * 1.01f, ref p_cv1, 0.1f), 0);
			rectTransform.sizeDelta = new Vector2(Mathf.SmoothDamp(rectTransform.sizeDelta.x, Width, ref p_cv2, 0.1f), rectTransform.sizeDelta.y);

			rectTransform.localEulerAngles = new Vector3(0,
				Mathf.SmoothDampAngle(rectTransform.localEulerAngles.y, Show ? 0 : -90, ref p_cv3, 0.1f)
			, 0);
			canvas.alpha = Mathf.Lerp(0, 1, rectTransform.localEulerAngles.y / 90 + 1);
			if(!Show && rectTransform.anchoredPosition.x <= -Width) {
				if(CurrentPlacement != null) {
					manager.Remove(CurrentPlacement);
				} else if(this is AbilityPanel ap && manager.CurrentAbilityPanel == ap) {
					manager.Remove(ap);
				} else {
					throw new Exception($"Unhandled Panel Type: {this}");
				}
			}
		}
	}
}
