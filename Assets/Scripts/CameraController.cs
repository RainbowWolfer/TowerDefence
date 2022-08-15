using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TowerDefence {
	public class CameraController: MonoBehaviour {
		public static CameraController Instance { get; private set; }

		public Camera mainCamera;
		public Camera uiCamera;

		private void Awake() {
			Instance = this;
		}

		public void AdjustPosition(int width, int height) {
			Vector3 pos = new Vector3(width, 8, height);
			if(width > height) {
				pos.x += width / 2;
			} else if(width < height) {
				pos.y += height / 2;
			}
			transform.position = pos;
		}
	}
}
