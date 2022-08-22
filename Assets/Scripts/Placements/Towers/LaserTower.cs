using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefence.Enemies;
using TowerDefence.Functions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TowerDefence.Placements.Towers {
	/// <summary>
	/// damage -> start damage / end damage
	/// GetDamage() -> strat damage
	/// GetDamageLength() -> start + length = end damage
	/// </summary>
	public class LaserTower: Tower, ITurret {
		[SerializeField]
		private Transform turret;
		[SerializeField]
		private Transform gun;

		[SerializeField]
		private LineRenderer laser_l;
		[SerializeField]
		private LineRenderer laser_r;

		[SerializeField]
		private Transform laser_l_start;
		[SerializeField]
		private Transform laser_r_start;

		[ColorUsage(true, false)]
		public Color laserColor;
		private Color _laserColor;

		[SerializeField]
		private MeshRenderer headLight;

		private float fireTime;
		private float Percentage => fireTime / GetChargingSpeed();

		public float HorAngle { get; set; }
		public float VerAngle { get; set; }
		private readonly LoopTimer randomRotateTimer = new LoopTimer(5, 2, 8);

		public Vector3 RandomFreePoint { get; set; }
		public float FreeTimeTurningSpeed => 0.5f;

		private float cv1;
		private float cv2;
		protected override void Awake() {
			base.Awake();
			targetUpdater.OnTargetChanged += e => {
				fireTime = 0;
			};
			laser_l.enabled = false;
			laser_r.enabled = false;
		}

		protected override void Start() {
			base.Start();
			_laserColor = new Color(0, 0.2f, 0.3f, 0.05f);
			laserColor = _laserColor;
			laser_l.colorGradient = GetFixedColor(_laserColor);
			laser_r.colorGradient = GetFixedColor(_laserColor);
			randomRotateTimer.ResetToNextInterval();
		}

		protected override void Update() {
			base.Update();
			if(Target != null) {
				CalculateAngles(Target.transform.position);
				if(CheckAimingReady()) {
					Fire(Target);
					randomRotateTimer.ResetToNextInterval();
				} else {
					laser_l.enabled = false;
					laser_r.enabled = false;
				}
			} else {
				laser_l.enabled = false;
				laser_r.enabled = false;
				if(randomRotateTimer.EveryTime(true)) {
					RandomFreePoint = new Vector3(
						Random.Range(-2, 2f),
						Random.Range(-0.2f, 0.2f),
						Random.Range(-2, 2f)
					);
					CalculateAngles(gun.position + RandomFreePoint);
				}
			}
			UpdateModel();
			_laserColor = Color.Lerp(_laserColor, laserColor, Time.deltaTime * 5);
			laser_l.colorGradient = GetFixedColor(_laserColor);
			laser_r.colorGradient = GetFixedColor(_laserColor);

			headLight.material.SetColor("_EmissionColor", _laserColor * Mathf.Lerp(0, 100, Percentage));
		}

		public override float GetTurningTime() {
			return Target == null ? FreeTimeTurningSpeed : base.GetTurningTime();
		}

		private void OnValidate() {
			laser_l.colorGradient = GetFixedColor(laserColor);
			laser_r.colorGradient = GetFixedColor(laserColor);
		}

		private Gradient GetFixedColor(Color color) {
			return new Gradient() {
				mode = GradientMode.Fixed,
				alphaKeys = new GradientAlphaKey[] {
					new GradientAlphaKey(color.a, 0),
					new GradientAlphaKey(color.a, 1),
				},
				colorKeys = new GradientColorKey[] {
					new GradientColorKey(color, 0),
					new GradientColorKey(color, 1),
				},
			};
		}

		private void Fire(Enemy enemy) {
			fireTime += Time.deltaTime;
			UpdateLaser(enemy.transform);
			DoDamage(enemy);
		}

		private void DoDamage(Enemy enemy) {
			float damage = Mathf.Lerp(
				GetDamage(),
				GetDamage() + GetDamageLength(),
				Percentage
			);
			laserColor = Color.Lerp(
				new Color(0, 0.2f, 0.3f, 0.05f),
				new Color(0, 0.65f, 1f, 1f),
				Percentage
			);
			enemy.TakeDamage(damage);
		}

		private void UpdateLaser(Transform target) {
			laser_l.enabled = true;
			laser_r.enabled = true;
			laser_l.SetPositions(new Vector3[] {
				target.position,
				laser_l_start.position
			});
			laser_r.SetPositions(new Vector3[] {
				target.position,
				laser_r_start.position
			});
		}

		public void CalculateAngles(Vector3 target) {
			Vector3 direction = target - (transform.position + new Vector3(0, 1.37f, 0));
			HorAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + 90;

			float radius = new Vector2(direction.x, direction.z).magnitude;
			VerAngle = Mathf.Atan2(direction.y, radius) * Mathf.Rad2Deg;

			VerAngle = Mathf.Clamp(VerAngle, -35, 60);

		}

		public bool CheckAimingReady() {
			float hor = Mathf.Abs(turret.localEulerAngles.z - HorAngle);
			if(hor > 180) {
				hor -= 360;
			}
			float ver = Mathf.Abs(gun.localEulerAngles.y - VerAngle);
			if(ver > 180) {
				ver -= 360;
			}
			const float deviation = 5;
			//Debug.Log($"hor:{hor} ;\tver:{ver}");
			return Mathf.Abs(hor) < deviation && Mathf.Abs(ver) < deviation;
		}

		public void UpdateModel() {
			turret.localEulerAngles = new Vector3(0, 0, Mathf.SmoothDampAngle(turret.localEulerAngles.z, HorAngle, ref cv1, 0.1f));
			gun.localEulerAngles = new Vector3(0, Mathf.SmoothDampAngle(gun.localEulerAngles.y, VerAngle, ref cv2, 0.1f), 0);
		}

		public override void Upgrade() {
			base.Upgrade();
		}

		public float GetDamageLength() {
			return (Star switch {
				Star.None => 2,
				Star.Star1 => 4,
				Star.Star2 => 6,
				Star.Star3 => 8,
				_ => throw new Exception(),
			}) * (IsUpgraded ? 1.2f : 1);
		}

		public float GetChargingSpeed() {
			return (Star switch {
				Star.None => 4,
				Star.Star1 => 5,
				Star.Star2 => 6,
				Star.Star3 => 7,
				_ => throw new Exception(),
			}) * (IsUpgraded ? 1.4f : 1);
		}
	}
}
