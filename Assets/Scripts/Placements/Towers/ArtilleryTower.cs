using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace TowerDefence.Placements.Towers {
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

		private float horAngle;

		private readonly Timer fireTimer = new Timer();


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
			if(Target != null && CheckAimingReady() && fireTimer.EverySeconds(GetFireRate())) {
				anim.SetTrigger("Fire");
				Fire();
			}
		}

		private void Fire(){
			Debug.Log(firePoint.position);
			Debug.Log(Target.transform.position);

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
			horAngle = Mathf.Atan2(chassisTarget.x, chassisTarget.z) * Mathf.Rad2Deg -90;
			chassis.transform.localRotation = Quaternion.Slerp(
				chassis.transform.localRotation,
				Quaternion.Euler(0, 0, horAngle),
				Time.deltaTime / GetTurningTime()
			);
		}
	}
}
