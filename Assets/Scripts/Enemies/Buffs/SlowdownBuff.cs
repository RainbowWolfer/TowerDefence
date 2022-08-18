using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefence.Enemies;
using TowerDefence.Enemies.Buffs;
using static UnityEngine.EventSystems.EventTrigger;

namespace TowerDefence.Enemies.Buffs {
	public class SlowdownBuff: BaseBuff {
		public float SlowdownPercentage { get; private set; }
		public SlowdownBuff(float duration, float slowdownPercentage) : base(duration) {
			SlowdownPercentage = slowdownPercentage;
		}

		public override void Update(Enemy enemy) {
			base.Update(enemy);
			enemy.speedMultiplier = SlowdownPercentage;
		}

		public override void OnLost(Enemy enemy) {
			enemy.speedMultiplier = 1f;
		}
	}
}
