using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefence.Towers;

namespace TowerDefence.UserInterface {
	public class AbilityPanel: PlacementPanel {
		public override Placement CurrentPlacement => current;
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
	}
}
