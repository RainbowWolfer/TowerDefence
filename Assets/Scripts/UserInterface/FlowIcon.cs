using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TowerDefence.UserInterface {
	public abstract class FlowIcon: MonoBehaviour {
		protected RectTransform Rt => transform as RectTransform;

		public abstract Transform Target { get; }
		public Vector3 offest;

		protected virtual void Awake() {

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
			Vector3 viewPos = Camera.main.WorldToViewportPoint(Target.position + offest);
			Rt.anchorMin = new Vector2(viewPos.x, viewPos.y);
			Rt.anchorMax = new Vector2(viewPos.x, viewPos.y);
		}
	}
}
