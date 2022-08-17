using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefence.Effects;
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
		[SerializeField]
		private Transform missile1;
		[SerializeField]
		private Transform missile2;
		[SerializeField]
		private Transform missile3;
		[SerializeField]
		private Transform missile4;

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

		public bool ready = true;
		private Vector3? targetPosition;

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
				targetPosition = Target.transform.position;
			}
			if(fireTimer.EverySeconds(GetFireRate())) {
				ready = true;
			}
			if(Target != null && CheckAimingReady() && ready) {
				ready = false;
				anim.SetTrigger("Fire");
			}
			anim.SetBool("IsReady", ready);
		}

		public void FireMissile(int index) {
			if(targetPosition == null || index < 0 || index > 3) {
				return;
			}
			Transform t = index switch {
				0 => missile1,
				1 => missile2,
				2 => missile3,
				3 => missile4,
				_ => throw new Exception(),
			};

			var missile = Instantiate(missilePrefab).GetComponent<Missile>();
			missile.transform.position = t.position;
			missile.target = targetPosition;
			missile.radius = GetBlastRadius();
			missile.damage = GetDamage();
		}

		private void RadarSpin() {
			radar.Rotate(radarSpinSpeed * Time.deltaTime * Vector3.up, Space.World);
		}

		private bool CheckAimingReady() {
			float hor = Mathf.Abs(chassis.transform.localEulerAngles.y - horAngle);
			if(hor > 180) {
				hor -= 360;
			}
			float ver = Mathf.Abs(turret.transform.localEulerAngles.x - verAngle);
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

		public float GetBlastRadius() {
			return (Star switch {
				Star.None => 1,
				Star.Star1 => 1.2f,
				Star.Star2 => 1.4f,
				Star.Star3 => 1.6f,
				_ => throw new Exception(),
			}) * (IsUpgraded ? 1.5f : 1);
		}
	}
}
