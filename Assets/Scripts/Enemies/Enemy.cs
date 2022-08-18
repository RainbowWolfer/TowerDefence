using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

		//public float speed = 5;

		public Path path;
		public int index = 0;
		public Vector3 offset;
		public Vector2 randomOffsetLimit = new Vector2(0.4f, 0.4f);

		public BaseBuff buff;

		[field: SerializeField]
		public float Health { get; private set; }

		[field: SerializeField]
		public float MaxHealth { get; private set; }

		public float HealthPercentage => Health / MaxHealth;

		public Vector3? startPosition;

		private void Awake() {
			MaxHealth = info.health * 1;
			Health = MaxHealth;
		}

		private void Start() {
			if(startPosition == null) {
				StartCoroutine(MoveCoroutine());
			} else {
				StartCoroutine(StartMoveCoroutine());
			}
		}

		private void Update() {
			buff?.Update(this);
		}

		public void TakeDamage(float damage) {
			Health -= damage;
			if(Health <= 0) {
				Die();
			}
		}

		private void Die() {
			if(this is ISpawnOnDeath death) {
				death.SpawnEnemies();
			}
			Game.Instance.enemies.Remove(this);
			UI.Instance.flowIconManager.RemoveHealthBar(this);
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

		private IEnumerator MoveCoroutine() {
			while(index != path.Length) {
				Vector3 next = GetNextPosition();
				Rotater?.UpdateTarget(next);
				transform.position = Vector3.MoveTowards(transform.position,
					next, Time.deltaTime * info.speed
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

		private Vector3 GetNextPosition() {
			Vector2Int v = path.path[index].Coord;
			return new Vector3(v.x, transform.position.y, v.y) + offset;
		}
	}
}
