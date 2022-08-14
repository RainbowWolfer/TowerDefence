using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using TowerDefence.Effects;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace TowerDefence.Placements.Towers {
	//[ExecuteInEditMode]
	public class ArtilleryTower: Tower {
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

		private float horAngle;
		public float verAngle;

		private readonly Timer fireTimer = new Timer();

		//public Transform obj;

		protected override void Awake() {
			base.Awake();
		}

		protected override void Start() {
			base.Start();
		}

		protected override void Update() {
			base.Update();
			if(Target != null) {
				AimAt(Target.transform);
			}

			if(Target != null
				&& CheckAimingReady()
				&& fireTimer.EverySeconds(GetFireRate())
				&& true //check is within minimum range
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
			//projectile.curve.keys[0].value = projectile.start.y / projectile.maxHeight;
			//Debug.Break();
		}

		public float GetBlastRadius(){
			return 1f;
		}

		public float GetMinimunAttackRadius(){
			return 1f;
		}

		private bool CheckAimingReady() {
			float hor = Mathf.Abs(chassis.transform.localEulerAngles.z - horAngle);
			if(hor > 180) {
				hor -= 360;
			}
			const float deviation = 8;
			return Mathf.Abs(hor) < deviation;
		}

		private void AimAt(Transform target) {
			Vector3 chassisTarget = target.position - transform.position;
			horAngle = Mathf.Atan2(chassisTarget.x, chassisTarget.z) * Mathf.Rad2Deg - 90;
			chassis.transform.localRotation = Quaternion.Slerp(
				chassis.transform.localRotation,
				Quaternion.Euler(0, 0, horAngle),
				Time.deltaTime / GetTurningTime()
			);

			float targetDistance = Vector2.Distance(
				new Vector2(target.position.x, target.position.z),
				new Vector2(transform.position.x, transform.position.z)
			);
			float maxRadius = GetAttackRadius();
			float minRadius = 1f;
			float radiusPercentage = Mathf.Clamp(targetDistance / (maxRadius - minRadius), 0, 1f);
			verAngle = Mathf.Lerp(10f, 45f, radiusPercentage);
			turret.localEulerAngles = new Vector3(0,
				Mathf.MoveTowards(turret.localEulerAngles.y, verAngle, Time.deltaTime * 25)
			, 0);
		}
	}
}
