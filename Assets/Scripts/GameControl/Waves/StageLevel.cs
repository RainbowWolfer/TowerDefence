using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TowerDefence.GameControl.Waves {
	[Serializable]
	public class StageLevel {
		public int levelCount;
		public float wavesInterval;
		public float readyTime;

		public List<Wave> waves;

		//public float delta_health = 5;

		public static List<StageLevel> GetDefaultLevels() {
			var levels = new List<StageLevel> {
				new StageLevel() {
					wavesInterval = 30,
					readyTime = 5,
					waves = new List<Wave>() {
						new Wave() {
							cubes = new EnemyCount(30, 50),
							robots = new EnemyCount(10, 20),
							hummers = new EnemyCount(5, 10),
							spawnInterval = 2f,
						},
					},
				},
				new StageLevel() {
					wavesInterval = 20,
					readyTime = 5,
					waves = new List<Wave>() {
						new Wave() {
							cubes = new EnemyCount(60, 90),
							robots = new EnemyCount(30, 50),
							hummers = new EnemyCount(10, 20),
						},
					},
				},

			};
			return levels;
		}
	}

	[Serializable]
	public class Wave {
		public int wave;
		public float spawnInterval = 0.5f;

		public EnemyCount cubes;
		public EnemyCount robots;
		public EnemyCount hummers;
		public EnemyCount apcs;
		public EnemyCount tanks;

		public bool finalWave;

		public List<EnemyType> GetAll(Dictionary<EnemyType, float> weights) {
			List<EnemyType> result = new List<EnemyType>();
			new List<(EnemyCount count, EnemyType type)>() {
				(cubes, EnemyType.Cube),
				(robots, EnemyType.Robot),
				(hummers, EnemyType.Hummer),
				(apcs, EnemyType.APC),
				(tanks, EnemyType.Tank),
			}.ForEach(c => {
				if(c.count != null) {
					for(int i = 0; i < c.count.GetCount(); i++) {
						result.Add(c.type);
					}
				}
			});
			if(weights != null) {
				for(int i = 0; i < result.Count; i++) {
					EnemyType type = result[i];
					if(Random.Range(0, 1f) < weights[type]) {
						result.RemoveAt(i);
						result.Insert(0, type);
					}
				}
			}
			return result;
		}

		public static Dictionary<EnemyType, float> GetClassicalWeights() {
			return new Dictionary<EnemyType, float>() {
				{ EnemyType.Cube, 0.55f },
				{ EnemyType.Robot, 0.3f },
				{ EnemyType.Hummer, 0.2f },
				{ EnemyType.APC, 0.1f },
				{ EnemyType.Tank, 0.05f },
			};
		}
	}

	[Serializable]
	public class EnemyCount {
		public EnemyCountType type;
		public int count;
		public int maxCount;

		public EnemyCount(int count) {
			this.count = count;
			type = EnemyCountType.Exact;
		}

		public EnemyCount(int count, int maxCount) {
			this.count = count;
			this.maxCount = maxCount;
			type = EnemyCountType.Random;
		}

		public int GetCount() {
			return type switch {
				EnemyCountType.Exact => count,
				EnemyCountType.Random => Random.Range(count, maxCount),
				_ => throw new Exception(),
			};
		}
	}

	public enum EnemyType {
		Cube, Robot, Hummer, APC, Tank
	}

	public enum EnemyCountType {
		Exact, Random
	}
}
