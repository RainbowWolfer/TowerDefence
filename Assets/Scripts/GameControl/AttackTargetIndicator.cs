using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefence;
using TowerDefence.Enemies;
using UnityEngine;

namespace TowerDefence.GameControl {
	[ExecuteInEditMode]
	public class AttackTargetIndicator: MonoBehaviour {
		[SerializeField]
		private Transform indicator;

		[Space]
		public Enemy enemy;
		public bool show;

		private void Update() {
			if(show && enemy != null) {
				indicator.gameObject.SetActive(true);
				//should get enemy height
				indicator.position = enemy.transform.position + new Vector3(0,
					0.7f + Mathf.Sin(Time.time * 2) * 0.1f,
				0);
			} else {
				indicator.gameObject.SetActive(false);
			}
		}
	}
}
