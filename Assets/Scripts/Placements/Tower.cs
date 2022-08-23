using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefence.Effects.Visuals;
using TowerDefence.Enemies;
using TowerDefence.GameControl;
using TowerDefence.Placements.Towers;
using UnityEngine;

namespace TowerDefence.Placements {
	public abstract class Tower: FieldPlacement {
		protected TargetUpdater targetUpdater;

		public Enemy Target { get; protected set; }


		public virtual float GetDamage() {
			return (Star switch {
				Star.None => info.damage.none,
				Star.Star1 => info.damage.one,
				Star.Star2 => info.damage.two,
				Star.Star3 => info.damage.three,
				_ => throw new Exception(),
			}) * (IsUpgraded ? info.damageUpgradeMultuplier : 1);
		}

		public virtual float GetAttackRadius() {
			return (Star switch {
				Star.None => info.attackRadius.none,
				Star.Star1 => info.attackRadius.one,
				Star.Star2 => info.attackRadius.two,
				Star.Star3 => info.attackRadius.three,
				_ => throw new Exception(),
			}) * (IsUpgraded ? info.radiusUpgradeMultuplier : 1);
		}

		public virtual float GetFireRate() {
			return (Star switch {
				Star.None => info.fireRate.none,
				Star.Star1 => info.fireRate.one,
				Star.Star2 => info.fireRate.two,
				Star.Star3 => info.fireRate.three,
				_ => throw new Exception(),
			}) * (IsUpgraded ? info.fireRateUpgradeMultuplier : 1);
		}

		public virtual float GetTurningTime() {
			if(this is ITurret turret && Target == null) {
				return turret.FreeTimeTurningSpeed;
			} else {
				return Star switch {
					Star.None => info.turnnigTime.none,
					Star.Star1 => info.turnnigTime.one,
					Star.Star2 => info.turnnigTime.two,
					Star.Star3 => info.turnnigTime.three,
					_ => throw new Exception(),
				};
			}
		}

		protected virtual void Awake() {
			targetUpdater = new TargetUpdater(this);
		}

		protected override void Start() {
			base.Start();
			ResetStats();
			OnLevelUp += s => {
				if(s > Star) {
					var effect = Instantiate(PrefabsManagaer.Instance.TowerLevelUpEffect, transform).GetComponent<LevelUpEffect>();
					effect.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
					effect.star = s;
				}
			};
		}

		protected override void Update() {
			base.Update();
			targetUpdater.SelfUpdate();
			Target = targetUpdater.Target;
		}

		public void ResetStats() {
			Exp = 0;
			Star = 0;
			Kills = 0;
			IsUpgraded = false;
		}
	}
}
