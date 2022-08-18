using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefence.Enemies;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence.UserInterface {
	public class HealthBar: FlowIcon {
		public Enemy target;

		public override Transform Target => target.transform;
		[SerializeField]
		private Image fill;

		protected override void Awake() {
			base.Awake();
		}

		protected override void Update() {
			base.Update();
			UpdateState();
			offest = new Vector3(0, target.healthBarHeight, 0);
			if(target.healthBarSize != Vector2.zero) {
				Rt.sizeDelta = target.healthBarSize;
			}
		}

		private void UpdateState() {
			fill.fillAmount = target.HealthPercentage;
		}
	}
}
