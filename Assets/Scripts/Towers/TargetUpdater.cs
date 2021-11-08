using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TowerDefence.Towers {
	public class TargetUpdater {
		private readonly Game game;
		private readonly Transform self;
		private readonly Tower tower;

		public FindType findType = FindType.Default;
		public Action<Enemy> OnTargetChanged { get; set; }

		private Enemy _target;
		public Enemy Target {
			get => _target;
			private set {
				_target = value;
				OnTargetChanged?.Invoke(value);
			}
		}

		public bool pause = false;

		public TargetUpdater(Game game, Tower t) {
			this.game = game;
			this.self = t.transform;
			this.tower = t;
		}

		public void SelfUpdate() {
			if(pause) {
				return;
			}
			List<Enemy> found = new List<Enemy>();
			foreach(Enemy e in game.enemies) {
				if(Vector3.Distance(e.transform.position, self.position) >= tower.GetAttackRadius()) {
					continue;
				}
				found.Add(e);
			}

			switch(findType) {
				case FindType.Default:
					if(found.Count <= 0) {
						Target = null;
					} else if(!found.Contains(Target)) {
						//the moment that lose target
						//so we need to find a new one which is random of the list
						Target = found[Random.Range(0, found.Count)];
					}
					break;
				case FindType.First:
					if(found.Count > 0) {
						Target = found[0];
					} else {
						Target = null;
					}
					break;
				case FindType.Last:
					if(found.Count > 0) {
						Target = found[found.Count - 1];
					} else {
						Target = null;
					}
					break;
				default:
					throw new Exception("no way");
			}
		}

		public enum FindType {
			Default, First, Last,
		}
	}
}
