using System;
using TowerDefence.Effects;
using TowerDefence.Functions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TowerDefence.Placements.Towers {
	//[ExecuteInEditMode]
	public class ArtilleryTower: Tower, ITurret {
		[SerializeField]
		private Animator anim;

		[Space]
		[SerializeField]
		private Transform chassis;
		[SerializeField]
		private Transform turret;
		[SerializeField]
		private Transform firePoint;
		[SerializeField]
		private AnimationCurve artilleryCurve;

		[Space]
		[SerializeField]
		private GameObject projectilePrefab;
		private readonly Timer fireTimer = new Timer();
		private readonly LoopTimer randomRotateTimer = new LoopTimer(5, 2, 8);

		public float HorAngle { get; set; }
		public float VerAngle { get; set; }
		public Vector3 RandomFreePoint { get; set; }
		public float FreeTimeTurningSpeed => 0.5f;

		//public Transform obj;

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

			float distance = Target == null ? 0 : Vector2.Distance(
				new Vector2(transform.position.x, transform.position.z),
				new Vector2(Target.transform.position.x, Target.transform.position.y)
			);
			if(Target != null
				&& CheckAimingReady()
				&& fireTimer.EverySeconds(GetFireRate())
				&& distance >= GetMinimunAttackRadius() //check is within minimum range
			) {
				anim.SetTrigger("Fire");
				Fire();
			}
		}

		private void Fire() {
			var projectile = Instantiate(projectilePrefab).GetComponent<ArtilleryProjectile>();
			projectile.transform.SetPositionAndRotation(
				firePoint.position,
				firePoint.rotation
			);
			projectile.start = firePoint.position;
			projectile.end = Target.transform.position;
			projectile.curve = artilleryCurve;
			projectile.damage = GetDamage();
			projectile.radius = GetBlastRadius();
			//projectile.curve.keys[0].value = projectile.start.y / projectile.maxHeight;
			//Debug.Break();
		}

		public override float GetTurningTime() {
			return Target == null ? FreeTimeTurningSpeed : base.GetTurningTime();
		}

		public float GetBlastRadius() {
			return (Star switch {
				Star.None => 1,
				Star.Star1 => 1.3f,
				Star.Star2 => 1.6f,
				Star.Star3 => 1.9f,
				_ => throw new Exception(),
			}) * (IsUpgraded ? 1.3f : 1);
		}

		public float GetMinimunAttackRadius() {
			return 1f;
		}

		public bool CheckAimingReady() {
			float hor = Mathf.Abs(chassis.transform.localEulerAngles.z - HorAngle);
			if(hor > 180) {
				hor -= 360;
			}
			const float deviation = 8;
			return Mathf.Abs(hor) < deviation;
		}

		public void CalculateAngles(Vector3 target) {
			Vector3 chassisTarget = target - transform.position;
			HorAngle = Mathf.Atan2(chassisTarget.x, chassisTarget.z) * Mathf.Rad2Deg - 90;


			float targetDistance = Vector2.Distance(
				new Vector2(target.x, target.z),
				new Vector2(transform.position.x, transform.position.z)
			);
			float maxRadius = GetAttackRadius();
			float minRadius = 1f;
			float radiusPercentage = Mathf.Clamp(targetDistance / (maxRadius - minRadius), 0, 1f);
			VerAngle = Mathf.Lerp(10f, 45f, radiusPercentage);
		}

		public void UpdateModel() {
			chassis.transform.localRotation = Quaternion.Slerp(
				chassis.transform.localRotation,
				Quaternion.Euler(0, 0, HorAngle),
				Time.deltaTime / GetTurningTime()
			);
			turret.localEulerAngles = new Vector3(0,
				Mathf.MoveTowards(turret.localEulerAngles.y, VerAngle, Time.deltaTime * 25)
			, 0);
		}
	}
}
