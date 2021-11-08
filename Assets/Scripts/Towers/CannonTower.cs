using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TowerDefence.Towers {
	public class CannonTower: Tower {
		[Space]
		[SerializeField]
		private Animator anim;
		[SerializeField]
		private Transform chassis;
		[SerializeField]
		private Transform turret;

		#region Attributes
		private float base_attackRadius;
		private float base_turningTime;
		private float base_fireRate;
		private float base_damage;
		#endregion

		private float horAngle;
		private float verAngle;

		private float cv1;
		private Timer fireTimer = new Timer();

		protected override void Awake() {
			base.Awake();
		}

		protected override void Update() {
			base.Update();
			if(Target != null) {
				AimAt(Target.transform);
			}
			bool ready = Target != null && CheckAimingReady();
			anim.SetBool("Firing", ready);
			if(ready && fireTimer.EverySeconds(GetFireRate())) {
				Fire();
			}
		}

		private void Fire() {
			float damage = GetDamage();
			Target.TakeDamage(damage);
			Exp += damage;
		}

		protected override void InitializeAttributes() {
			base_attackRadius = 2;
			base_turningTime = 0.05f;
			base_fireRate = 0.05f;
			base_damage = 10;
		}

		public override void Upgrade() {
			base.Upgrade();
			base_attackRadius = 3;
			base_turningTime = 0.04f;
			base_fireRate = 0.035f;
			base_damage = 20;
		}

		public override float GetDamage() {
			return base_damage * Star switch {
				0 => 1f,
				1 => 1.3f,
				2 => 1.6f,
				3 => 2f,
				_ => throw new Exception("Error"),
			};
		}

		public override float GetAttackRadius() {
			return base_attackRadius * Star switch {
				0 => 1f,
				1 => 1.1f,
				2 => 1.3f,
				3 => 1.5f,
				_ => throw new Exception("Error"),
			};
		}

		public float GetTurningTime() {
			return base_turningTime * Star switch {
				0 => 1f,
				1 => 1f,
				2 => 0.95f,
				3 => 0.9f,
				_ => throw new Exception("Error"),
			};
		}

		public float GetFireRate() {
			return base_fireRate * Star switch {
				0 => 1f,
				1 => 1.1f,
				2 => 1.2f,
				3 => 1.3f,
				_ => throw new Exception("Error"),
			};
		}

		private void AimAt(Transform target) {
			Vector3 chassisTarget = target.position - transform.position;
			horAngle = Mathf.Atan2(chassisTarget.x, chassisTarget.z) * Mathf.Rad2Deg + 90;
			chassis.transform.localRotation = Quaternion.Slerp(chassis.transform.localRotation, Quaternion.Euler(-90, 0, horAngle), Time.deltaTime / GetTurningTime());

			Vector3 turretTarget = target.position - (transform.position + new Vector3(0, 0.8f, 0));
			float radius = new Vector2(turretTarget.x, turretTarget.z).magnitude;
			verAngle = Mathf.Atan2(turretTarget.y, radius) * Mathf.Rad2Deg;
			turret.transform.localEulerAngles = new Vector3(0, Mathf.SmoothDampAngle(turret.transform.localEulerAngles.y, verAngle, ref cv1, GetTurningTime()), 0);
		}

		private bool CheckAimingReady() {
			float hor = Mathf.Abs(chassis.transform.localEulerAngles.y - horAngle);
			if(hor > 180) {
				hor -= 360;
			}
			float ver = Mathf.Abs(turret.transform.localEulerAngles.y - verAngle);
			if(ver > 180) {
				ver -= 360;
			}
			const float deviation = 5;
			//Debug.Log($"hor:{hor} ;\tver:{ver}");
			return Mathf.Abs(hor) < deviation && Mathf.Abs(ver) < deviation;
		}

	}
}
