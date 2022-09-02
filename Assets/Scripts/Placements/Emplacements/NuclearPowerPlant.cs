using UnityEngine;

namespace TowerDefence.Placements.Emplacements {
	public class NuclearPowerPlant: Emplacement {
		public override EmplacementAbility Ability {
			get => null;
			set { return; }
		}

		protected override void Start() {
			base.Start();
		}

		protected override void Update() {
			base.Update();
		}

		public override void Upgrade() {
			base.Upgrade();
		}

		public override void Fire(Vector2 coord) {

		}



	}
}
