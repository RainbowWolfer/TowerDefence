using System;
using UnityEngine;

namespace TowerDefence.Data {
	[CreateAssetMenu(fileName = "Tower Prefabs Data", menuName = "Data/Tower Prefabs Data")]
	public class TowerPrefabs: ScriptableObject {
		public TowerInfo defaultCube;

		[Space]
		[Header("Towers")]
		public TowerInfo cannonTower;
		public TowerInfo lightningTower;
		public TowerInfo laserTower;
		public TowerInfo snipeTower;
		public TowerInfo missileTower;
		public TowerInfo artilleryTower;

		[Space]
		[Header("Emplacements")]
		public TowerInfo stormGeneratorEmplacement;
		public TowerInfo windPowerPlant;
		public TowerInfo nuclearPowerPlant;

		[Header("Environments")]
		[Space]
		[Header("Trees")]
		public TowerInfo oakTree;
		public TowerInfo firTree;
		public TowerInfo palmTree;
		public TowerInfo poplarTree;

		[Header("Buildings")]
		public TowerInfo abandonedCastle;

		[Space]
		public TowerInfo tankTrap;


		public TowerInfo RequestByID(short id) {
			TowerInfo target = id switch {
				-6 => abandonedCastle,
				-5 => tankTrap,
				-4 => poplarTree,
				-3 => palmTree,
				-2 => firTree,
				-1 => oakTree,
				0 => defaultCube,
				1 => cannonTower,
				2 => lightningTower,
				3 => laserTower,
				4 => snipeTower,
				5 => missileTower,
				6 => artilleryTower,
				7 => stormGeneratorEmplacement,
				8 => windPowerPlant,
				9 => nuclearPowerPlant,
				_ => throw new Exception($"Tower ID({id}) is not found"),
			};
			target.id = id;
			return target;
		}
	}
}
