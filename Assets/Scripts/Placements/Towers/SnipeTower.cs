using System.Collections.Generic;
using TowerDefence.Enemies;
using TowerDefence.Functions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TowerDefence.Placements.Towers {
	public class SnipeTower: Tower, ITurret {
		[Space]
		[SerializeField]
		private Animator anim;

		[Space]
		[SerializeField]
		private Transform chassis;
		[SerializeField]
		private Transform turret;
		[SerializeField]
		private Transform bulletCasingTransform;
		[SerializeField]
		private Transform raycastCenter;
		[SerializeField]
		private Transform firePoint;

		[Space]
		[SerializeField]
		private GameObject bulletCasingPrefab;
		[SerializeField]
		private GameObject snipeLineEffectPrefab;

		public float HorAngle { get; set; }
		public float VerAngle { get; set; }
		public Vector3 RandomFreePoint { get; set; }
		public float FreeTimeTurningSpeed => 0.5f;

		private readonly LoopTimer randomRotateTimer = new LoopTimer(5, 2, 8);


		private float cv1;
		private readonly Timer fireTimer = new Timer();


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
			if(ready && fireTimer.EverySeconds(GetFireRate())) {
				anim.SetTrigger("Fire");
				GenerateEffect();
				Fire();
			}
		}

		private void GenerateEffect() {
			var line = Instantiate(snipeLineEffectPrefab).GetComponent<LineRenderer>();
			line.SetPositions(new Vector3[2]{
				firePoint.position,
				Target.transform.position,
			});
		}

		private void Fire() {
			List<Enemy> enemyList = new List<Enemy>() { Target };
			//raycast
			Vector3 direction = Target.transform.position - raycastCenter.position;
			RaycastHit[] hits = Physics.RaycastAll(raycastCenter.position, direction, 9, LayerMask.GetMask("Enemy"));
			foreach(RaycastHit item in hits) {
				if(item.collider.TryGetComponentInParent<Enemy>(out var e) && !enemyList.Contains(e)) {
					enemyList.Add(e);
				}
			}
			for(int i = 0; i < enemyList.Count; i++) {
				float damage = GetDamage() * (i * -0.1f + 1.1f);//get damage decay (index)
				bool dead = enemyList[i].TakeDamage(damage);
				if(dead) {
					Kills++;
				}
				Exp += damage;
			}
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

		public void CalculateAngles(Vector3 target) {
			Vector3 chassisTarget = target - transform.position;
			HorAngle = Mathf.Atan2(chassisTarget.x, chassisTarget.z) * Mathf.Rad2Deg + 90;

			Vector3 turretTarget = target - (transform.position + new Vector3(0, 0.8f, 0));
			float radius = new Vector2(turretTarget.x, turretTarget.z).magnitude;
			VerAngle = -Mathf.Atan2(turretTarget.y, radius) * Mathf.Rad2Deg;
		}

		public void UpdateModel() {
			chassis.transform.localRotation = Quaternion.Slerp(
				chassis.transform.localRotation,
				Quaternion.Euler(-90, 0, HorAngle),
				Time.deltaTime / GetTurningTime()
			);
			turret.transform.localEulerAngles = new Vector3(0,
				Mathf.SmoothDampAngle(turret.transform.localEulerAngles.y, VerAngle, ref cv1, GetTurningTime()),
			0);
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
