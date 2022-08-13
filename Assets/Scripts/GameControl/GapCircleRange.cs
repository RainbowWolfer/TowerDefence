using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefence.Placements;
using UnityEngine;

namespace TowerDefence.GameControl {
	[ExecuteInEditMode]
	public class GapCircleRange: MonoBehaviour {
		public float rotateSpeed = 5;
		public float swaySpeed = 2;

		public float radius = 3;
		public float height = 1;

		[Space]
		[SerializeField]
		private Transform circle;

		[field: SerializeField]
		public AttackTargetIndicator AttackTargetIndicator { get; private set; }

		private void Update() {
			circle.transform.localScale = new Vector3(
				radius,
				radius,
				height + (Mathf.Sin(Time.time * swaySpeed) + 1) / 3
			);
			circle.Rotate(0, 0, rotateSpeed * Time.deltaTime, Space.Self);
		}

		public void Show(Tower tower) {
			circle.gameObject.SetActive(tower != null);
			AttackTargetIndicator.show = tower != null;
			if(tower != null) {
				transform.position = new Vector3(tower.transform.position.x, 0, tower.transform.position.z);
				radius = tower.GetAttackRadius();
			}
		}
	}
}
