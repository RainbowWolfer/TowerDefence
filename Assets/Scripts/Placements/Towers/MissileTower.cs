using System;
using TowerDefence.Effects;
using TowerDefence.Functions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TowerDefence.Placements.Towers {
	public class MissileTower: Tower, ITurret {
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

		public float HorAngle { get; set; }
		public float VerAngle { get; set; }
		public Vector3 RandomFreePoint { get; set; }
		public float FreeTimeTurningSpeed => 0.5f;

		private readonly LoopTimer randomRotateTimer = new LoopTimer(5, 2, 8);


		private float cv1;
		private readonly Timer fireTimer = new Timer();

		public bool ready = true;
		private Vector3? targetPosition;


		protected override void Awake() {
			base.Awake();
		}

		protected override void Start() {
			base.Start();
			randomRotateTimer.ResetToNextInterval();
		}

		protected override void Update() {
			base.Update();
			RadarSpin();
			if(Target != null) {
				CalculateAngles(Target.transform.position);
				targetPosition = Target.transform.position;
				randomRotateTimer.ResetToNextInterval();
			} else {
				if(randomRotateTimer.EveryTime(true)) {
					RandomFreePoint = new Vector3(
						Random.Range(-2, 2f),
						Random.Range(-0.2f, 0.2f),
						Random.Range(-2, 2f)
					);
					CalculateAngles(turret.position + RandomFreePoint);
				}
			}

			UpdateModel();
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
			missile.owner = this;
			missile.transform.position = t.position;
			missile.target = targetPosition;
			missile.radius = GetBlastRadius();
			missile.damage = GetDamage();
		}

		private void RadarSpin() {
			radar.Rotate(radarSpinSpeed * Time.deltaTime * Vector3.up, Space.World);
		}

		public bool CheckAimingReady() {
			float hor = Mathf.Abs(chassis.transform.localEulerAngles.y - HorAngle);
			if(hor > 180) {
				hor -= 360;
			}
			float ver = Mathf.Abs(turret.transform.localEulerAngles.x - VerAngle);
			if(ver > 180) {
				ver -= 360;
			}
			const float deviation = 5;
			//Debug.Log($"hor:{hor} ;\tver:{ver}");
			return Mathf.Abs(hor) < deviation && Mathf.Abs(ver) < deviation;
		}

		public void CalculateAngles(Vector3 target) {
			Vector3 chassisTarget = target - transform.position;
			HorAngle = Mathf.Atan2(chassisTarget.x, chassisTarget.z) * Mathf.Rad2Deg + 90;

			Vector3 turretTarget = target - (transform.position + new Vector3(0, 0.8f, 0));
			float radius = new Vector2(turretTarget.x, turretTarget.z).magnitude;
			VerAngle = Mathf.Atan2(turretTarget.y, radius) * Mathf.Rad2Deg;
		}

		public void UpdateModel() {
			chassis.transform.localRotation = Quaternion.Slerp(
				chassis.transform.localRotation,
				Quaternion.Euler(-90, 0, HorAngle),
				Time.deltaTime / GetTurningTime()
			);

			turret.transform.localEulerAngles = new Vector3(
				Mathf.SmoothDampAngle(turret.transform.localEulerAngles.x, VerAngle, ref cv1, GetTurningTime()),
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
