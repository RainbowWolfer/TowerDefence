using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefence.Towers;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;

namespace TowerDefence.Towers {
	public class SnipeTower: Tower {
		[Space]
		[SerializeField]
		private Animator anim;

		[SerializeField]
		private Transform chassis;
		[SerializeField]
		private Transform turret;
		[SerializeField]
		private Transform bulletCasingTransform;
		[SerializeField]
		private Transform raycastCenter;

		[Space]
		[SerializeField]
		private GameObject bulletCasingPrefab;


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
			if(Target != null) {
				AimAt(Target.transform);
			}
			//AimAt(obj.transform);
			bool ready = Target != null && CheckAimingReady();
			if(ready && fireTimer.EverySeconds(GetFireRate())) {
				anim.SetTrigger("Fire");
				Fire();
			}
		}

		private void Fire() {
			List<Enemy> enemyList = new List<Enemy>() { Target };
			//raycast
			Vector3 direction = Target.transform.position - raycastCenter.position;
			RaycastHit[] hits = Physics.RaycastAll(raycastCenter.position, direction, 9, LayerMask.GetMask("Enemy"));
			foreach(RaycastHit item in hits) {
				if(item.collider.TryGetComponent<Enemy>(out var e) && !enemyList.Contains(e)) {
					enemyList.Add(e);
				}
			}
			for(int i = 0; i < enemyList.Count; i++) {
				float damage = GetDamage() * (i * -0.1f + 1.1f);
				enemyList[i].TakeDamage(damage);
				Exp += damage;
			}
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
			verAngle = -Mathf.Atan2(turretTarget.y, radius) * Mathf.Rad2Deg;
			turret.transform.localEulerAngles = new Vector3(0,
				Mathf.SmoothDampAngle(turret.transform.localEulerAngles.y, verAngle, ref cv1, GetTurningTime()),
			0);
		}

		public float GetTurningTime() {
			return 0.04f * (Star switch {
				0 => 1f,
				1 => 1f,
				2 => 0.95f,
				3 => 0.9f,
				_ => throw new Exception("Error"),
			});
		}
		public float GetFireRate() {
			return 1f * Star switch {
				0 => 1f,
				1 => 1.1f,
				2 => 1.2f,
				3 => 1.3f,
				_ => throw new Exception("Error"),
			};
		}

		public override float GetAttackRadius() {
			return 6f;
		}

		public override float GetDamage() {
			return 200f;
		}

		//used in animation 
		public void EjectBullet() {
			var rigidbody = Instantiate(bulletCasingPrefab).GetComponent<Rigidbody>();
			rigidbody.transform.SetPositionAndRotation(
				bulletCasingTransform.transform.position,
				bulletCasingTransform.transform.rotation
			);

			rigidbody.AddRelativeTorque(
				Random.Range(-70, 70),
				Random.Range(-70, 70),
				Random.Range(-70, 70) * Time.deltaTime,
				ForceMode.Impulse
			);

			rigidbody.AddRelativeForce(
				Random.Range(0, 0),
				Random.Range(2.5f, 5f),
				Random.Range(0, 0),
				ForceMode.Impulse
			);

			Destroy(rigidbody.gameObject, 5f);
		}
	}
}
