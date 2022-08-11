using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TowerDefence.Towers {
	public abstract class Emplacement: FieldPlacement {
		public abstract EmplacementAbility Ability { get; set; }

		protected override void Update() {
			base.Update();
			Ability?.UpdateCooldownTimer(IsUpgraded);
		}

		public bool IsCooldownReady() {
			if(Ability == null) {
				return false;
			}
			if(IsUpgraded) {
				return Ability.CooldownTimer >= Ability.UpgradedCooldown;
			} else {
				return Ability.CooldownTimer >= Ability.Cooldown;
			}
		}

		public abstract void Fire(Vector2 coord);
	}

	public enum EmplacementAbilityType {
		Individual, CirclrArea
	}

	public enum AbilityStatusType {
		Idle, Ready, Firing
	}

	public class EmplacementAbility {
		public float AbilityRadius { get; set; }
		public float Cooldown { get; set; }
		public float UpgradedCooldown { get; set; }
		public float CooldownTimer { get; set; } = 0;
		public bool IsFiring { get; set; }

		public AbilityStatusType StatusType { get; private set; } = AbilityStatusType.Idle;
		public float CooldownPercentage { get; private set; }

		public void ResetCooldown() {
			CooldownTimer = 0;
			StatusType = AbilityStatusType.Idle;
		}

		public void UpdateCooldownTimer(bool IsUpgraded) {
			if(!IsFiring) {
				CooldownTimer += Time.deltaTime;
			}

			float max = IsUpgraded ? UpgradedCooldown : Cooldown;
			float current = CooldownTimer;
			CooldownPercentage = current >= max ? 1f : current / max;

			if(IsFiring) {
				StatusType = AbilityStatusType.Firing;
			} else {
				StatusType = current >= max ? AbilityStatusType.Ready : AbilityStatusType.Idle;
			}

			if(Input.GetKeyDown(KeyCode.C)) {
				CooldownTimer += max;
			}
		}
	}
}
