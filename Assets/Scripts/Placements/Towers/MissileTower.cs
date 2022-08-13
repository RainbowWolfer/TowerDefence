using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TowerDefence.Placements.Towers {
	public class MissileTower: Tower {
		[Space]
		[SerializeField]
		private Animator anim;

		[Space]
		[SerializeField]
		private Transform chassis;
		[SerializeField]
		private Transform turret;
		[SerializeField]
		private Transform radar;

		[Space]
		[SerializeField]
		private float radarSpinSpeed = 25;


		[Space]
		[SerializeField]
		private GameObject missilePrefab;


		private float horAngle;
		private float verAngle;

		private float cv1;
		private readonly Timer fireTimer = new Timer();

		protected override void Awake() {
			base.Awake();
		}

		protected override void Start() {
			base.Start();
		}

		protected override void Update() {
			base.Update();
			RadarSpin();
			if(Target != null) {
				AimAt(Target.transform);
			}
			bool ready = Target != null && CheckAimingReady();
			//anim.SetBool("Firing", ready);
			if(ready && fireTimer.EverySeconds(GetFireRate())) {
				Fire();
			}
		}

		private void Fire() {
			float damage = GetDamage();
			Target.TakeDamage(damage);
			Exp += damage;
		}

		public void FireMissile(int index) {
			if(index < 0 || index > 3) {
				return;
			}


		}

		private void RadarSpin() {
			radar.Rotate(radarSpinSpeed * Time.deltaTime * Vector3.up, Space.World);
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

		private void AimAt(Transform target) {
			Vector3 chassisTarget = target.position - transform.position;
			horAngle = Mathf.Atan2(chassisTarget.x, chassisTarget.z) * Mathf.Rad2Deg + 90;
			chassis.transform.localRotation = Quaternion.Slerp(
				chassis.transform.localRotation,
				Quaternion.Euler(-90, 0, horAngle),
				Time.deltaTime / GetTurningTime()
			);

			Vector3 turretTarget = target.position - (transform.position + new Vector3(0, 0.8f, 0));
			float radius = new Vector2(turretTarget.x, turretTarget.z).magnitude;
			verAngle = Mathf.Atan2(turretTarget.y, radius) * Mathf.Rad2Deg;
			turret.transform.localEulerAngles = new Vector3(
				Mathf.SmoothDampAngle(turret.transform.localEulerAngles.x, verAngle, ref cv1, GetTurningTime()),
			0, 0);
		}
	}
}
