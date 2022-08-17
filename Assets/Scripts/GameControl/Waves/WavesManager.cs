using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefence.UserInterface;
using UnityEngine;

namespace TowerDefence.GameControl.Waves {
	public class WavesManager: MonoBehaviour {
		public List<StageLevel> levels;
		public int currentLevel = 0;//start from 1

		private Coroutine levelCoroutine;

		private void Start() {

		}

		public void StartGame() {
			if(levels == null || levels.Count == 0 || levelCoroutine != null) {
				return;
			}
			currentLevel = 1;

			levelCoroutine = StartCoroutine(StartLevels(levels));
		}

		public void StopGame() {
			if(levelCoroutine != null) {
				StopCoroutine(levelCoroutine);
			}
			levelCoroutine = null;
		}

		private IEnumerator StartLevels(List<StageLevel> levels) {
			for(int i = 0; i < levels.Count; i++) {
				StageLevel level = levels[i];
				//update values
				currentLevel = i + 1;
				//popup ui stuff
				UI.Instance.incomingPanel.PopupLevel(currentLevel, level, level.readyTime);
				yield return new WaitForSeconds(level.readyTime);
				StartCoroutine(CreateLevelWaves(level));

				//wait until all enemies killed or time runs out
				yield return new WaitUntil(() => false);
			}
		}


		private IEnumerator CreateLevelWaves(StageLevel level) {
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
