﻿using UnityEngine;
using Random = UnityEngine.Random;

namespace TowerDefence.Placements.Environments {
	public class EnvironmentCube: Placement {
		public Transform rotationParent;
		public Vector3 axis;

		public bool enableRandomRotationOnStart = true;

		protected override void Start() {
			base.Start();
			if(enableRandomRotationOnStart) {
				rotationParent.Rotate(axis, Random.Range(0, 360));
			}
		}

		public void Shovel() {
			Level.Cash -= info.shovelPrice;
			Game.Instance.level.ClearNode(coord.x, coord.y);
			Game.Instance.control.DeselectTower(true);
		}
	}
}
