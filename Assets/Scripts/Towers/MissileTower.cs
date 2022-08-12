using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TowerDefence.Towers {
	public class MissileTower: Tower {
			

		public override float GetAttackRadius() {
			return 5;
		}

		public override float GetDamage() {
			return 1;
		}

		protected override void InitializeAttributes() {
			
		}


	}
}
