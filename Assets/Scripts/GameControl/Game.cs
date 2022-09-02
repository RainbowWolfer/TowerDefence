using System.Collections.Generic;
using TowerDefence.Data;
using TowerDefence.Enemies;
using TowerDefence.Enemies.Instances;
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
			waves.MaxEscapes = 20;
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

		public List<Enemy> GetEnemiesInRangeV3(Vector3 position, float radius) {
			List<Enemy> enemiesInRange = new List<Enemy>();
			foreach(Enemy item in enemies) {
				if(Vector3.Distance(item.transform.position, position) < radius) {
					enemiesInRange.Add(item);
				}
			}
			return enemiesInRange;
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

		public (float damage, int kills) EnemiesTakeAreaDamageV3(Vector3 position, float radius, float damage) {
			List<Enemy> enemiesInRange = GetEnemiesInRangeV3(position, radius);
			float totalDamage = 0;
			int kills = 0;
			foreach(Enemy e in enemiesInRange) {
				bool dead = e.TakeDamage(damage);
				if(dead) {
					kills++;
				}
				totalDamage += damage;
			}
			return (totalDamage, kills);
		}

		private void AddEnemy(Enemy enemy) {
			enemies.Add(enemy);
			UI.Instance.flowIconManager.AddHealthBar(enemy);
		}

		public Enemy SpawnEnemy(EnemyType type) {
			Vector2Int start = level.StartCoord;
			EnemyInfo info = Enemies.RequestByType(type);
			Enemy enemy = Instantiate(info.prefab, enemiesParent).GetComponent<Enemy>();
			enemy.info = info;
			enemy.transform.position = new Vector3(start.x, 0f, start.y);
			enemy.path = level.targetPath;
			//enemy.speed = 0.5f;
			//enemy.maxHealth = 300 + 1 * 500;//wave
			//enemy.Health = enemy.maxHealth;
			AddEnemy(enemy);
			return enemy;
		}

		public List<Enemy> SpawnEnemies(APC apc) {
			List<Enemy> result = new List<Enemy>();

			Vector2 start = new Vector2(
				apc.transform.position.x,
				apc.transform.position.z
			);

			for(int i = 0; i < Random.Range(4, 8); i++) {
				EnemyInfo info = Enemies.RequestByType(EnemyType.Cube);
				Enemy enemy = Instantiate(info.prefab, enemiesParent).GetComponent<Enemy>();
				enemy.info = info;
				enemy.transform.SetPositionAndRotation(
					new Vector3(start.x, 0f, start.y),
					Quaternion.Euler(0, Random.Range(0, 360), 0)
				);
				enemy.startPosition = new Vector3(start.x, 0f, start.y) + new Vector3(
					Random.Range(-0.3f, 0.3f),
					0,
					Random.Range(-0.3f, 0.3f)
				);
				enemy.GenerateRandomOffset();
				enemy.index = apc.index;
				enemy.path = apc.path;

				AddEnemy(enemy);

				result.Add(enemy);
			}

			return result;
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
			if(Physics.Raycast(ray, out RaycastHit hit, 100, LayerMask.GetMask("Ground"))) {
				return hit.point;
			}
			return new Vector3(int.MinValue, int.MinValue);
		}

	}
}
