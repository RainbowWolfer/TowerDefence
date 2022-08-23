using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefence.Placements;
using UnityEngine;

namespace TowerDefence.Effects {
	//[ExecuteInEditMode]
	public class Missile: MonoBehaviour {
		public Tower owner;

		public Vector3? target;
		public float distanceThreshold = 0.1f;

		[Space]
		public float travelSpeed = 10;
		public float damage;
		public float radius;


		private bool exploded = false;

		private void Update() {
			if(target == null) {
				return;
			}

			transform.LookAt(target.Value);
			transform.position = Vector3.MoveTowards(transform.position, target.Value, Time.deltaTime * travelSpeed);

			if(!exploded && Vector3.Distance(target.Value, transform.position) <= distanceThreshold) {
				exploded = true;
				Explode();
			}
		}

		private void Explode() {
			(float totalDamage, int kills) = Game.Instance.EnemiesTakeAreaDamageV3(transform.position, radius, damage);
			if(owner != null) {
				owner.Exp += totalDamage;
				owner.Kills += kills;
			}
			Destroy(gameObject);
		}
	}
}
