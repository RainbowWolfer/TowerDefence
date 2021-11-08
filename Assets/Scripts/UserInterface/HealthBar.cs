using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
			offest = new Vector3(0, 0.2f, 0);
		}

		protected override void Update() {
			base.Update();
			UpdateState();
		}

		private void UpdateState() {
			fill.fillAmount = target.health / target.maxHealth;
		}
	}
}
