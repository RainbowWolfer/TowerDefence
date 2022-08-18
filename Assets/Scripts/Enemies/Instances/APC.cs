using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefence.Enemies.Interfaces;
using TowerDefence.GameControl.Waves;
using UnityEngine;

namespace TowerDefence.Enemies.Instances {
	public class APC: Enemy, ISpawnOnDeath {
		public int carriedCubes;
		public int carriedRobots;
		void ISpawnOnDeath.SpawnEnemies() {
			Game.Instance.SpawnEnemies(this);
		}
	}
}
