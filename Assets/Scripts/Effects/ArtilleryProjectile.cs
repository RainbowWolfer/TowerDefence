using System.Collections.Generic;
using TowerDefence.Enemies;
using TowerDefence.Enemies.Buffs;
using TowerDefence.Placements;
using UnityEngine;

namespace TowerDefence.Effects {
	//[ExecuteInEditMode]
	public class ArtilleryProjectile: MonoBehaviour {
		public Tower owner;
		public Vector3 start;
		public Vector3 end;
		public AnimationCurve curve;
		[Space]
		[SerializeField]
		private Transform model;

		[Space]
		[Range(0, 1f)]
		public float percentage;
		public float speed;
		public float maxHeight = 1f;

		public bool timePass = false;

		[Space]
		public float damage;
		public float radius;
		[Space]
		public float slowEnemyDuration = 0;
		[Range(0, 1f)]
		public float slowEnemyPercetange = 0;

		private void Start() {
			timePass = true;
			transform.position = start;
			percentage = 0;


			//make the projectile in the correct starting position
			float targetPercentage = start.y / maxHeight;
			const float offset = 0.05f;
			for(float i = 0; i < 0.5f; i += offset) {
				var yPer = curve.Evaluate(i);
				if(Mathf.Abs(yPer - targetPercentage) < offset) {
					percentage = i;
					break;
				}
			}

			//curve.keys[0] = new Keyframe(0, start.y / maxHeight);
			//Debug.Log(curve.keys[0].value);
			//Debug.Log(start.y / maxHeight);
			//Debug.Break();
		}

		private void Update() {
			if(timePass) {//make it easy for editor debug
				percentage += Time.deltaTime * speed / 10;
			}
			percentage = Mathf.Clamp(percentage, 0, 1f);

			(Vector3 pos_lead, float height_lead) = GetPositionAndHeight(percentage + 0.01f);
			model.LookAt(new Vector3(pos_lead.x, height_lead, pos_lead.z));

			(Vector3 pos, float height) = GetPositionAndHeight(percentage);
			transform.position = new Vector3(pos.x, height, pos.z);
			//Debug.Break();

			if(percentage >= 1) {
				//do explosion
				(float totalDamage, int kills) = Game.Instance.EnemiesTakeAreaDamageV3(transform.position, radius, damage);
				if(owner != null) {
					owner.Exp += totalDamage;
					owner.Kills += kills;
				}
				//do slow down
				List<Enemy> enemies = Game.Instance.GetEnemiesInRangeV3(transform.position, radius);
				foreach(Enemy e in enemies) {
					e.buffs.Add(new SlowdownBuff(0.7f, 0.4f));
				}

				Destroy(gameObject);
			}
		}

		private (Vector3 pos, float height) GetPositionAndHeight(float per) {
			per = Mathf.Clamp(per, 0, 1f);
			Vector3 pos = Vector3.Lerp(start, end, per);
			float height = curve.Evaluate(per) * this.maxHeight;
			return (pos, height);
		}
	}
}
