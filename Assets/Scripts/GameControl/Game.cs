using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro.Examples;
using TowerDefence.Data;
using TowerDefence.UserInterface;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TowerDefence {
	public class Game: MonoBehaviour {
		public static Game Instance;
		public static TowerPrefabs Towers;

		public Level level;
		public LevelControl control;

		[SerializeField]
		private TowerPrefabs towers;
		[SerializeField]
		private Transform enemiesParent;

		[SerializeField]
		private GameObject testEnemy;

		public readonly List<Enemy> enemies = new List<Enemy>();

		private Coroutine creatingEnemiesCoroutine;

		private void Awake() {
			Instance = this;
			Towers = towers;
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

			if(creatingEnemiesCoroutine != null) {
				StopCoroutine(creatingEnemiesCoroutine);
			}
			creatingEnemiesCoroutine = StartCoroutine(CreateEnemiesWaves());
		}

		private int wave = 0;
		private IEnumerator CreateEnemiesWaves() {
			//CreateEnemy();
			//yield break;
			while(!Input.GetKeyDown(KeyCode.Y)) {
				wave++;
				for(int i = 0; i < Random.Range(7, 15); i++) {
					CreateEnemy();
					yield return new WaitForSeconds(Random.Range(0.4f, 1f));
				}
				yield return new WaitForSeconds(Random.Range(4, 7f));
			}
			creatingEnemiesCoroutine = null;
		}

		public static Vector2 MultiplyVectors(Vector3 v, Vector3 w) {
			v.x *= w.x;
			v.y *= w.y;
			v.z *= w.z;
			return v;
		}

		private void CreateEnemy() {
			Vector2Int start = level.StartCoord;
			Enemy enemy = Instantiate(testEnemy, enemiesParent).GetComponent<Enemy>();
			enemy.transform.position = new Vector3(start.x, 0.1f, start.y);
			enemy.path = level.targetPath;
			enemy.speed = 0.5f;
			enemy.maxHealth = 3000 + wave * 1000;
			enemy.health = enemy.maxHealth;
			enemy.game = this;
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
