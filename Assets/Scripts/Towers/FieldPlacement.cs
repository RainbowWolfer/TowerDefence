using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefence.Functions;
using UnityEngine;

namespace TowerDefence.Towers {
	public abstract class FieldPlacement: Placement {
		private float exp;
		public float Exp {
			get => exp;
			protected set {
				exp = value;
				if(value >= info.star3Exp) {
					if(Star == 3) {
						return;
					}
					Star = 3;
				} else if(value >= info.star2Exp) {
					if(Star == 2) {
						return;
					}
					Star = 2;
				} else if(value >= info.star1Exp) {
					if(Star == 1) {
						return;
					}
					Star = 1;
				}
			}
		}

		public float ExpPercentage {
			get {
				if(Exp > info.star3Exp) {
					return 1;
				} else if(Exp > info.star2Exp) {
					return (Exp - info.star2Exp) / (info.star3Exp - info.star2Exp);
				} else if(Exp > info.star1Exp) {
					return (Exp - info.star1Exp) / (info.star2Exp - info.star1Exp);
				} else {
					return Exp / info.star1Exp;
				}
			}
		}

		public int Star { get; protected set; }
		public int Kills { get; protected set; }

		public bool IsUpgraded { get; protected set; }

		public virtual void Upgrade() {
			IsUpgraded = true;
		}

		public void Sell() {
			Game.Instance.level.Cash += IsUpgraded ? info.upgradedSellPrice : info.sellPrice;
			Game.Instance.level.ClearNode(coord.x, coord.y);
			Game.Instance.control.DeselectTower(true);
		}

		protected override void Start() {
			base.Start();
			if(!Methods.CheckLinerIncrease(true, info.star1Exp, info.star2Exp, info.star3Exp)) {
				throw new Exception($"The Exp of {info.name} is not correct");
			}
		}

		protected override void Update() {
			base.Update();
		}
	}
}
