using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefence.GameControl;
using UnityEngine;

namespace TowerDefence.Towers {
	public abstract class Tower: FieldPlacement {
		protected Game game;
		protected TargetUpdater targetUpdater;
		public Enemy Target { get; protected set; }


		public abstract float GetDamage();
		public abstract float GetAttackRadius();

		protected virtual void Awake() {
			game = Game.Instance;
			targetUpdater = new TargetUpdater(game, this);
		}

		protected override void Start() {
			base.Start();
			Exp = 0;
			Star = 0;
			Kills = 0;
			IsUpgraded = false;
			InitializeAttributes();
		}

		protected override void Update() {
			base.Update();
			targetUpdater.SelfUpdate();
			Target = targetUpdater.Target;
		}

		protected abstract void InitializeAttributes();

	}
}
