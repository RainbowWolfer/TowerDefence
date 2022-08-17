using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro.Examples;
using TowerDefence.Data;
using TowerDefence.Enemies;
using TowerDefence.Functions;
using TowerDefence.GameControl.Waves;
using TowerDefence.UserInterface;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TowerDefence {
	public class Game: MonoBehaviour {
		public static Game Instance { get; private set; }

		public Level level;
		public LevelControl control;

		public WavesManager waves;

		[field: SerializeField]
		public TowerPrefabs Towers { get; private set; }

		[field: SerializeField]
		public EnemyPrefabs Enemies { get; private set; }

		[SerializeField]
		private Transform enemiesParent;

		//[SerializeField]
		//private GameObject testEnemy;

		public readonly List<Enemy> enemies = new List<Enemy>();

		private void Awake() {
			Instance = this;
		}

		private void Start() {
			GameInitialize();
		}

		private void Update() {
			if(Input.GetKeyDown(KeyCode.K)) {
				GameInitialize();
			} else if(Input.GetKey(KeyCode.J)) {
				level.VisualizePath();
			}
			//if(Input.GetMouseButtonDown(0)) {
			//	Vector3 pos = GetMousePosition();
			//	int x = Mathf.RoundToInt(pos.x);
			//	int y = Mathf.RoundToInt(pos.z);
			//	if(level.Check(x, y)) {
			//		level.EditNode(x, y, (short)Random.Range(1, 3));
			//	}
			//}
		}

		public void GameInitialize() {
			level.Initialize(MapInfo.GenerateRandomMap(15, 20));
			ClearEnemies();

			waves.levels = StageLevel.GetDefaultLevels();
			waves.StartGame();
			//if(creatingEnemiesCoroutine != null) {
			//	StopCoroutine(creatingEnemiesCoroutine);
			//}
			//creatingEnemiesCoroutine = StartCoroutine(CreateEnemiesWaves());
		}

		public static Vector2 MultiplyVectors(Vector3 v, Vector3 w) {
			v.x *= w.x;
			v.y *= w.y;
			v.z *= w.z;
			return v;
		}

		public void EnemiesTakeAreaDamageV2(Vector2 position, float radius, float damage) {
			List<Enemy> enemiesInRange = new List<Enemy>();
			foreach(Enemy item in enemies) {
				var pos = new Vector2(item.transform.position.x, item.transform.position.z);
				if(Vector2.Distance(pos, position) < radius) {
					enemiesInRange.Add(item);
				}
			}

			foreach(Enemy e in enemiesInRange) {
				e.TakeDamage(damage);
			}
		}

		public void EnemiesTakeAreaDamageV3(Vector3 position, float radius, float damage) {
			List<Enemy> enemiesInRange = new List<Enemy>();
			foreach(Enemy item in enemies) {
				if(Vector3.Distance(item.transform.position, position) < radius) {
					enemiesInRange.Add(item);
				}
			}

			foreach(Enemy e in enemiesInRange) {
				e.TakeDamage(damage);
			}
		}

		public void SpawnEnemy(EnemyType type) {
			Vector2Int start = level.StartCoord;
			EnemyInfo info = Enemies.RequestByType(type);
			Enemy enemy = Instantiate(info.prefab, enemiesParent).GetComponent<Enemy>();
			enemy.info = info;
			enemy.transform.position = new Vector3(start.x, 0f, start.y);
			enemy.path = level.targetPath;
			//enemy.speed = 0.5f;
			//enemy.maxHealth = 300 + 1 * 500;//wave
			//enemy.Health = enemy.maxHealth;
			enemies.Add(enemy);

			UI.Instance.flowIconManager.AddHealthBar(enemy);
		}

		private void ClearEnemies() {
			foreach(Enemy item in enemies) {
				Destroy(item.gameObject);
			}
			enemies.Clear();
			UI.Instance.flowIconManager.ClearHealthBars();
		}

		public static Vector3 GetMousePosition() {
			Ray ray = CameraController.Instance.mainCamera.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out RaycastHit hit, 100, ~LayerMask.GetMask("UI"))) {
				return hit.point;
			}
			return new Vector3(int.MinValue, int.MinValue);
		}

	}
}
