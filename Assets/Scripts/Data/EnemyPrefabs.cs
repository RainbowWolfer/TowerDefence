using System;
using TowerDefence.GameControl.Waves;
using UnityEngine;

namespace TowerDefence.Data {
	[CreateAssetMenu(fileName = "Enemy Prefabs Data", menuName = "Data/Enemy Prefabs Data")]
	[Serializable]
	public class EnemyPrefabs: ScriptableObject {
		public EnemyInfo cube;
		public EnemyInfo robot;
		public EnemyInfo hummer;
		public EnemyInfo apc;
		public EnemyInfo tank;

		public EnemyInfo RequestByType(EnemyType type) {
			EnemyInfo target = type switch {
				EnemyType.Cube => cube,
				EnemyType.Robot => robot,
				EnemyType.Hummer => hummer,
				EnemyType.APC => apc,
				EnemyType.Tank => tank,
				_ => throw new Exception($"Enemy ID({type}) is not found"),
			};
			target.id = (short)type;
			return target;
		}

		public EnemyInfo RequestByID(short id) {
			EnemyInfo target = id switch {
				1 => cube,
				2 => robot,
				3 => hummer,
				4 => apc,
				5 => tank,
				_ => throw new Exception($"Enemy ID({id}) is not found"),
			};
			target.id = id;
			return target;
		}
	}
}
