using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TowerDefence.Enemies.Buffs {
	public class BaseBuff {
		public float Time { get; private set; }
		public float Duration { get; private set; }

		public bool HasLostEffect => Time >= Duration;

		public BaseBuff(float duration) {
			Time = 0;
			Duration = duration;
		}

		public virtual void Update(Enemy enemy) {
			Time += UnityEngine.Time.deltaTime;
		}
	}
}
