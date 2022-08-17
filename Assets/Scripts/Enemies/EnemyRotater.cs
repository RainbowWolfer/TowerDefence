using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TowerDefence.Enemies {
	public class EnemyRotater: MonoBehaviour {
		[SerializeField]
		private Transform model;

		[SerializeField]
		private float turnSpeed = 5;

		private Vector3 target;

		public void UpdateTarget(Vector3 pos) {
			target = pos;
		}

		private void Update() {
			Vector3 direction = target - model.position;
			direction.y = 0;
			Quaternion toRotation = Quaternion.LookRotation(direction);
			model.rotation = Quaternion.Slerp(model.rotation, toRotation, turnSpeed * Time.deltaTime);
		}
	}
}
