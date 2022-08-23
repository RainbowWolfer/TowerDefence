using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.VFX;

namespace TowerDefence.Placements.Towers {
	public class LightningTower: Tower {
		[Space]
		[SerializeField]
		private Color lightningColor;

		//[SerializeField]
		//private float attackRadius = 2;
		//[SerializeField]
		//private float fireRate = 3;
		//[SerializeField]
		//private float damage = 500;

		[SerializeField]
		private Transform parentX;
		[SerializeField]
		private Transform parentZ;

		[SerializeField]
		private VisualEffect effect;

		private readonly Timer fireTimer = new Timer();
		protected override void Awake() {
			base.Awake();
		}

		protected override void Update() {
			base.Update();
			if(Target != null) {
				SetRotation(Target.transform);
			}
			if(Target != null && fireTimer.EverySeconds(GetFireRate())) {
				Fire(Target.transform);
			}
		}

		private void Fire(Transform e) {
			targetUpdater.pause = true;
			SetSize(e);
			StartCoroutine(DamageDelay(0.15f));
			effect.Play();
		}

		private IEnumerator DamageDelay(float delay) {
			yield return new WaitForSeconds(delay);
			DoDamage();
			yield return new WaitForSeconds(0.5f);//after effect disappear
			targetUpdater.pause = false;
		}

		private void DoDamage() {
			if(Target != null) {
				float damage = GetDamage();
				bool dead = Target.TakeDamage(damage);
				if(dead) {
					Kills++;
				}
				Exp += damage;
			}
		}

		private void SetSize(Transform t) {
			float distance = Vector3.Distance(effect.transform.position, t.position);
			//Debug.Log(distance / 5);
			effect.SetFloat("Height", distance / 5);
		}

		private void SetRotation(Transform t) {
			Vector3 direction = Target.transform.position - (transform.position + new Vector3(0, 1.5f, 0));
			float horAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + 90;

			float radius = new Vector2(direction.x, direction.z).magnitude;
			float verAngle = Mathf.Atan2(direction.y, radius) * Mathf.Rad2Deg;

			parentX.localEulerAngles = new Vector3(0, 0, -90 + horAngle);
			parentZ.localEulerAngles = new Vector3(-verAngle, 0, 0);

			//Debug.Log(horAngle + "  _  " + verAngle);
		}

		private void OnDrawGizmos() {
			if(Target == null) {
				return;
			}
			//Vector3 direction = target.transform.position - (transform.position + new Vector3(0, 1.5f, 0));
			//float horAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + 90;

			//float radius = new Vector2(direction.x, direction.z).magnitude;
			//float verAngle = Mathf.Atan2(direction.y, radius) * Mathf.Rad2Deg;

			//Gizmos.DrawRay(transform.position + new Vector3(0, 1.5f, 0), direction);

			//float distance = Vector3.Distance(effect.transform.position, target.transform.position);
			//Debug.Log("(" + horAngle + "  ,  " + verAngle + " )distance:" + distance);
		}

	}
}
