using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefence.Effects.Visuals;
using TowerDefence.Functions;
using TowerDefence.GameControl;
using UnityEngine;

namespace TowerDefence.Placements {
	public abstract class FieldPlacement: Placement {
		public Action<Star> OnLevelUp { get; set; }
		private float exp;
		public float Exp {
			get => exp;
			set {
				exp = value;
				if(value >= info.star3Exp) {
					if(Star == Star.Star3) {
						return;
					}
					OnLevelUp?.Invoke(Star.Star3);
					Star = Star.Star3;
				} else if(value >= info.star2Exp) {
					if(Star == Star.Star2) {
						return;
					}
					OnLevelUp?.Invoke(Star.Star2);
					Star = Star.Star2;
				} else if(value >= info.star1Exp) {
					if(Star == Star.Star1) {
						return;
					}
					OnLevelUp?.Invoke(Star.Star1);
					Star = Star.Star1;
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
		public int Kills { get; set; }


		public Star Star { get; protected set; }


		public bool IsUpgraded { get; protected set; }

		public virtual void Upgrade() {
			if(Level.Cash < info.upgradePrice) {
				return;
			}
			IsUpgraded = true;
			Level.Cash -= info.upgradePrice;
			var effect = Instantiate(PrefabsManagaer.Instance.TowerUpgradeEffect, transform).GetComponent<UpgradeEffect>();
			effect.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
		}

		public void Sell() {
			Level.Cash += IsUpgraded ? info.UpgradedSellPrice : info.SellPrice;
			Game.Instance.level.ClearNode(coord.x, coord.y);
			Game.Instance.control.DeselectTower(true);
		}

		protected override void Start() {
			base.Start();
			if(info != null && !Methods.CheckLinerIncrease(true, info.star1Exp, info.star2Exp, info.star3Exp)) {
				Debug.LogError($"The Exp of {info.name} is not correct");
			}
		}

		protected override void Update() {
			base.Update();
		}
	}

	public enum Star {
		None, Star1, Star2, Star3
	}

}
