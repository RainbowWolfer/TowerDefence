using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TowerDefence.GameControl.Waves {
	public class WavesManager: MonoBehaviour {
		public List<StageLevel> levels;
		public int currentLevel = 0;//start from 1

		private Coroutine levelCoroutine;

		private void Start() {

		}

		public void StartGame() {
			if(levels == null || levels.Count == 0) {
				return;
			}
			currentLevel = 1;


			//StartCoroutine();
		}


		private IEnumerator CreateWaves(StageLevel level) {
			foreach(Wave wave in level.waves) {
				foreach(EnemyType item in wave.GetAll(Wave.GetClassicalWeights())) {
					Game.Instance.SpawnEnemy(item);
					yield return new WaitForSeconds(wave.spawnInterval);
				}
				yield return new WaitForSeconds(level.wavesInterval);
			}
			levelCoroutine = null;
		}
	}
}
