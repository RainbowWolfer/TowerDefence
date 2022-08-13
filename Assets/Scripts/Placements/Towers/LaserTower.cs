using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TowerDefence.Placements.Towers {
	/// <summary>
	/// damage -> start damage / end damage
	/// GetDamage() -> strat damage
	/// GetDamageLength() -> start + length = end damage
	/// </summary>
	public class LaserTower: Tower {
		[SerializeField]
		private Transform turrert;
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

		private bool aimReady;

		private float horAngle;
		private float verAngle;

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
		}

		protected override void Update() {
			base.Update();
			if(Target != null) {
				AimAt(Target.transform);
				aimReady = CheckAimingReady();
				if(aimReady) {
					Fire(Target);
				} else {
					laser_l.enabled = false;
					laser_r.enabled = false;
				}
			} else {
				laser_l.enabled = false;
				laser_r.enabled = false;
			}
			_laserColor = Color.Lerp(_laserColor, laserColor, Time.deltaTime * 5);
			laser_l.colorGradient = GetFixedColor(_laserColor);
			laser_r.colorGradient = GetFixedColor(_laserColor);

			headLight.material.SetColor("_EmissionColor", _laserColor * Mathf.Lerp(0, 100, Percentage));
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

		private void AimAt(Transform target) {
			Vector3 direction = target.position - (transform.position + new Vector3(0, 1.37f, 0));
			horAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + 90;

			float radius = new Vector2(direction.x, direction.z).magnitude;
			verAngle = Mathf.Atan2(direction.y, radius) * Mathf.Rad2Deg;

			verAngle = Mathf.Clamp(verAngle, -35, 60);
			turrert.localEulerAngles = new Vector3(0, 0, Mathf.SmoothDampAngle(turrert.localEulerAngles.z, horAngle, ref cv1, 0.1f));
			gun.localEulerAngles = new Vector3(0, Mathf.SmoothDampAngle(gun.localEulerAngles.y, verAngle, ref cv2, 0.1f), 0);
		}

		private bool CheckAimingReady() {
			float hor = Mathf.Abs(turrert.localEulerAngles.z - horAngle);
			if(hor > 180) {
				hor -= 360;
			}
			float ver = Mathf.Abs(gun.localEulerAngles.y - verAngle);
			if(ver > 180) {
				ver -= 360;
			}
			const float deviation = 5;
			//Debug.Log($"hor:{hor} ;\tver:{ver}");
			return Mathf.Abs(hor) < deviation && Mathf.Abs(ver) < deviation;
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
