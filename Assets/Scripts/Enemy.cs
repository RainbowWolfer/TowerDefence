using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefence.UserInterface;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TowerDefence {
	public class Enemy: MonoBehaviour {
		public Game game;
		public float maxHealth;
		public float health;

		public float speed = 5;
		public Path path;
		public int index = 0;
		public Vector3 offset;

		private void Start() {
			StartCoroutine(MoveCoroutine());
		}

		private void Update() {

		}

		public void TakeDamage(float damage) {
			health -= damage;
			if(health <= 0) {
				Die();
			}
		}

		private void Die() {
			game.enemies.Remove(this);
			UI.Instance.flowIconManager.RemoveHealthBar(this);
			Destroy(gameObject);
		}

		private IEnumerator MoveCoroutine() {
			while(index != path.Length) {
				transform.position = Vector3.MoveTowards(transform.position, GetNextPosition(), speed * Time.deltaTime);
				if(Vector3.Distance(transform.position, GetNextPosition()) < 0.1f) {
					index++;
					GenerateRandomOffset();
				}
				yield return null;
			}
			game.enemies.Remove(this);
			UI.Instance.flowIconManager.RemoveHealthBar(this);
			Destroy(this.gameObject);
		}

		private void GenerateRandomOffset() {
			if(Random.Range(0, 100) > 50) {
				return;
			}
			offset = new Vector3(Random.Range(-0.4f, 0.4f), 0, Random.Range(-0.4f, 0.4f));
		}

		private Vector3 GetNextPosition() {
			var v = path.path[index].Coord;
			return new Vector3(v.x, transform.position.y, v.y) + offset;
		}
	}
}
