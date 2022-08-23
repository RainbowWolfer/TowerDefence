using TowerDefence.Functions;
using UnityEngine;

namespace TowerDefence.Placements.Towers {
	public class CannonTower: Tower, ITurret {
		[Space]
		[SerializeField]
		private Animator anim;
		[SerializeField]
		private Transform chassis;
		[SerializeField]
		private Transform turret;


		private float cv1;
		private readonly Timer fireTimer = new Timer();

		public float HorAngle { get; set; }
		public float VerAngle { get; set; }
		public Vector3 RandomFreePoint { get; set; }

		public float FreeTimeTurningSpeed => 0.5f;

		private readonly LoopTimer randomRotateTimer = new LoopTimer(5, 2, 8);

		protected override void Awake() {
			base.Awake();
		}

		protected override void Start() {
			base.Start();
			randomRotateTimer.ResetToNextInterval();
		}

		protected override void Update() {
			base.Update();
			if(Target != null) {
				CalculateAngles(Target.transform.position);
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

			bool ready = Target != null && CheckAimingReady();
			anim.SetBool("Firing", ready);
			if(ready && fireTimer.EverySeconds(GetFireRate())) {
				Fire();
			}
		}

		private void Fire() {
			float damage = GetDamage();
			bool dead = Target.TakeDamage(damage);
			Exp += damage;
			if(dead) {
				Kills++;
			}
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
			turret.transform.localEulerAngles = new Vector3(0,
				Mathf.SmoothDampAngle(
					turret.transform.localEulerAngles.y,
					VerAngle,
					ref cv1,
					GetTurningTime()
				)
			, 0);
		}



		public bool CheckAimingReady() {
			float hor = Mathf.Abs(chassis.transform.localEulerAngles.y - HorAngle);
			if(hor > 180) {
				hor -= 360;
			}
			float ver = Mathf.Abs(turret.transform.localEulerAngles.y - VerAngle);
			if(ver > 180) {
				ver -= 360;
			}
			const float deviation = 5;
			//Debug.Log($"hor:{hor} ;\tver:{ver}");
			return Mathf.Abs(hor) < deviation && Mathf.Abs(ver) < deviation;
		}


		private void OnDrawGizmosSelected() {
			Gizmos.DrawSphere(transform.position + RandomFreePoint, 0.1f);
		}
	}
}
