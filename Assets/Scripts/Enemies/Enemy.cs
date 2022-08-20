using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TowerDefence.Data;
using TowerDefence.Enemies.Buffs;
using TowerDefence.Enemies.Interfaces;
using TowerDefence.GameControl.Waves;
using TowerDefence.UserInterface;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TowerDefence.Enemies {
	public class Enemy: MonoBehaviour {
		public EnemyInfo info;

		[field: SerializeField]
		public EnemyRotater Rotater { get; private set; }
		[Space]

		//public float speed = 5;

		public Path path;
		public int index = 0;
		public Vector3 offset;
		public Vector2 randomOffsetLimit = new Vector2(0.4f, 0.4f);

		private float speedData;
		private float healthData;


		[Space]
		[Header("Multipliers")]
		public float speedMultiplier = 1;

		[Space]
		public float healthBarHeight = 0.2f;
		public Vector2 healthBarSize = new Vector2(35, 5);

		public List<BaseBuff> buffs = new List<BaseBuff>();

		[field: Space]
		[field: SerializeField]
		public float Health { get; private set; }

		[field: SerializeField]
		public float MaxHealth { get; private set; }

		public float HealthPercentage => Health / MaxHealth;

		public Vector3? startPosition;

		protected virtual void Awake() {
			speedData = info.speed.GetRandom();
			healthData = info.health.GetRandom();

			int level = WavesManager.Instance.currentLevelInt;
			MaxHealth = healthData * (1 + WavesManager.Instance.healthMultiplier * (level - 1));
			Health = MaxHealth;
		}

		protected virtual void Start() {
			if(startPosition == null) {
				index = 1;
				StartCoroutine(MoveCoroutine());
			} else {
				StartCoroutine(StartMoveCoroutine());
			}
		}

		protected virtual void Update() {
			BuffsUpdate();
		}

		private void BuffsUpdate() {
			List<BaseBuff> timedOutBuffs = new List<BaseBuff>();
			buffs.ForEach(b => {
				b.Update(this);
				if(b.HasLostEffect) {
					timedOutBuffs.Add(b);
					b.OnLost(this);
				}
			});
			timedOutBuffs.ForEach(b => {
				buffs.Remove(b);
			});
		}

		public bool TakeDamage(float damage) {
			Health -= damage;
			if(Health <= 0) {
				Die();
				return true;
			} else {
				return false;
			}
		}

		private void Die() {
			if(this is ISpawnOnDeath death) {
				death.SpawnEnemies();
			}
			Game.Instance.enemies.Remove(this);
			UI.Instance.flowIconManager.RemoveHealthBar(this);
			Level.Cash += (int)info.coins.GetRandom();
			Destroy(gameObject);
		}

		private IEnumerator StartMoveCoroutine() {
			//move to start position
			float time = Time.time;
			while(Vector2.Distance(new Vector2(
				transform.position.x,
				transform.position.z
			), new Vector2(
				startPosition.Value.x,
				startPosition.Value.z
			)) >= 0.01f && Time.time - time <= 0.5f) {
				transform.position = Vector3.Lerp(transform.position, startPosition.Value, Time.deltaTime * 5);
				yield return null;
			}
			if(Time.time - time <= 0.5f) {
				yield return new WaitForSeconds(0.5f);
			}
			yield return MoveCoroutine();
		}

		private Vector3 GetNextPosition() {
			Vector2Int v = path.path[index].Coord;
			return new Vector3(v.x, transform.position.y, v.y) + offset;
		}

		private IEnumerator MoveCoroutine() {
			while(index != path.Length) {
				Vector3 next = GetNextPosition();
				Rotater?.UpdateTarget(next);
				transform.position = Vector3.MoveTowards(transform.position,
					next, Time.deltaTime * speedData / 100 * speedMultiplier
				);
				if(Vector3.Distance(transform.position, next) < 0.1f) {
					index++;
					GenerateRandomOffset();
				}
				yield return null;
			}
			//escaped
			WavesManager.Instance.CurrentEscapes++;
			WavesManager.UpdateEscapes();
			Game.Instance.enemies.Remove(this);
			UI.Instance.flowIconManager.RemoveHealthBar(this);
			Destroy(gameObject);
		}

		public void GenerateRandomOffset() {
			if(Random.Range(0, 100) <= 50) {
				offset = new Vector3(
					Random.Range(-randomOffsetLimit.x, randomOffsetLimit.x),
					0,
					Random.Range(-randomOffsetLimit.y, randomOffsetLimit.y)
				);
			}
		}

	}
}
