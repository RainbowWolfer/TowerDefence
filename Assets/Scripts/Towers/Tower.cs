using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefence.GameControl;
using UnityEngine;

namespace TowerDefence.Towers {
	public abstract class Tower: Placement {
		protected Game game;
		protected TargetUpdater targetUpdater;
		public Enemy Target { get; protected set; }

		private float exp;

		public float Exp {
			get => exp;
			protected set {
				exp = value;
				if(value >= info.star3Exp && Star != 3) {
					Star = 3;
				} else if(value >= info.star2Exp && Star != 2) {
					Star = 2;
				} else if(value >= info.star1Exp && Star != 1) {
					Star = 1;
				}
			}
		}
		public int Star { get; protected set; }
		public int Kills { get; protected set; }

		public bool IsUpgraded { get; protected set; }

		public abstract float GetDamage();
		public abstract float GetAttackRadius();
		protected virtual void Awake() {
			game = Game.Instance;
			targetUpdater = new TargetUpdater(game, this);
		}

		private void Start() {
			Exp = 0;
			Star = 0;
			Kills = 0;
			IsUpgraded = false;
			InitializeAttributes();
		}

		protected virtual void Update() {
			targetUpdater.SelfUpdate();
			Target = targetUpdater.Target;
		}

		protected abstract void InitializeAttributes();

		public virtual void Upgrade() {
			IsUpgraded = true;
		}

		public void Sell() {
			Game.Instance.level.Cash += IsUpgraded ? info.upgradedSellPrice : info.sellPrice;
			Game.Instance.level.ClearNode(coord.x, coord.y);
			Game.Instance.control.DeselectTower(true);
		}

		public virtual void Ability() {//select (-> click detination) -> do things
			if(!info.abilityEnabled) {
				return;
			}
		}

	}
}
