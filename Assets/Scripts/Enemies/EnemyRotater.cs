﻿using UnityEngine;

namespace TowerDefence.Enemies {
	public class EnemyRotater: MonoBehaviour {
		[SerializeField]
		private Transform model;

		[SerializeField]
		private float turnSpeed = 5;

		private Vector3 target;
		private bool firstAssign = true;

		public void UpdateTarget(Vector3 pos) {
			target = pos;
			if(firstAssign) {
				firstAssign = false;
				model.LookAt(target);
			}
		}

		private void Update() {
			Vector3 direction = target - model.position;
			direction.y = 0;
			if(direction.magnitude > Vector3.kEpsilon) {//Look rotation viewing vector is zero
				Quaternion toRotation = Quaternion.LookRotation(direction);
				model.rotation = Quaternion.Slerp(model.rotation, toRotation, turnSpeed * Time.deltaTime);
			}
		}
	}
}
