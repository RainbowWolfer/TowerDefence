using TowerDefence.Enemies.Interfaces;

namespace TowerDefence.Enemies.Instances {
	public class APC: Enemy, ISpawnOnDeath {
		//public int carriedCubes;
		//public int carriedRobots;
		void ISpawnOnDeath.SpawnEnemies() {
			Game.Instance.SpawnEnemies(this);
		}
	}
}
