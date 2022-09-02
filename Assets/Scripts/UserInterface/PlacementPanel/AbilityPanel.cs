using System;
using TowerDefence.Placements;

namespace TowerDefence.UserInterface {
	public class AbilityPanel: PlacementPanel {
		//public override Placement CurrentPlacement => current;
		public Emplacement current;

		public override float Width => 500;

		public override void Initialize(Placement placement, PlacementPanelManager manager) {
			base.Initialize(placement, manager);
			if(placement is Emplacement e) {
				this.current = e;
			} else {
				throw new Exception($"{nameof(placement)} type cast error");
			}
		}

		protected override void Update() {
			base.Update();

		}
	}
}
