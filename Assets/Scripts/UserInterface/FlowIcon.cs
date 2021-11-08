using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TowerDefence.UserInterface {
	public abstract class FlowIcon: MonoBehaviour {
		protected RectTransform rectTransform;

		public abstract Transform Target { get; }
		public Vector3 offest;

		protected virtual void Awake() {
			rectTransform = transform as RectTransform;
		}

		protected virtual void Start() {
			UpdatePosition();
		}

		protected virtual void Update() {
			if(Target != null) {
				UpdatePosition();
			}
		}

		private void UpdatePosition() {
			//rectTransform.localPosition = UI.GetScreenPosition(Target.position + offest);
			Vector3 viewPos = Camera.main.WorldToViewportPoint(Target.position + offest);
			GetComponent<RectTransform>().anchorMin = new Vector2(viewPos.x, viewPos.y);
			GetComponent<RectTransform>().anchorMax = new Vector2(viewPos.x, viewPos.y);
		}
	}
}
