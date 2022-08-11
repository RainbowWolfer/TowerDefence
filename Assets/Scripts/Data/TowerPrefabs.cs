using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TowerDefence.Data {
	[CreateAssetMenu(fileName = "Tower Prefabs Data", menuName = "Data/Tower Prefabs Data")]
	public class TowerPrefabs: ScriptableObject {
		public TowerInfo defaultCube;

		public TowerInfo cannonTower;
		public TowerInfo lightningTower;
		public TowerInfo laserTower;

		public TowerInfo stormGeneratorEmplacement;

		public TowerInfo oakTree;
		public TowerInfo firTree;
		public TowerInfo palmTree;
		public TowerInfo poplarTree;

		public TowerInfo tankTrap;


		public TowerInfo RequestByID(short id) {
			TowerInfo target = id switch {
				-5 => tankTrap,
				-4 => poplarTree,
				-3 => palmTree,
				-2 => firTree,
				-1 => oakTree,
				0 => defaultCube,
				1 => cannonTower,
				2 => lightningTower,
				3 => laserTower,
				4 => stormGeneratorEmplacement,
				_ => throw new Exception($"ID({id}) is not found"),
			};
			target.id = id;
			return target;
		}
	}
}
