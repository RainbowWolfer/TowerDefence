using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TowerDefence.Towers {
	public abstract class Emplacement: Placement {
		public bool IsUpgraded { get; protected set; }

		public void Upgrade() {

		}

		public void Sell() {
			Game.Instance.level.Cash += IsUpgraded ? info.upgradedSellPrice : info.sellPrice;
			Game.Instance.level.ClearNode(coord.x, coord.y);
			Game.Instance.control.DeselectTower(true);
		}

		public virtual void Abibity() {

		}
	}
}
