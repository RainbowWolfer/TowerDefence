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

		public bool IsOnGoing { get; set; } = false;

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
				UI.Instance.levelPanel.Levels.Set(currentLevel);
				IsOnGoing = false;
				yield return new WaitForSeconds(level.readyTime);
				IsOnGoing = true;
				yield return CreateLevelWaves(level);

				//wait until all enemies killed or time runs out
				//yield return new WaitUntil(() => false);
			}
			levelCoroutine = null;
		}


		private IEnumerator CreateLevelWaves(StageLevel level) {
			for(int i = 0; i < level.waves.Count; i++) {
				Wave wave = level.waves[i];
				UI.Instance.levelPanel.ResetProgress();
				UI.Instance.levelPanel.Waves.Set(i + 1, level.waves.Count);
				List<EnemyType> list = wave.GetAll(Wave.GetClassicalWeights());
				for(int j = 0; j < list.Count; j++) {
					EnemyType item = list[j];
					Game.Instance.SpawnEnemy(item);
					UI.Instance.levelPanel.UpdateProgress((j + 1) / (float)list.Count);
					yield return new WaitForSeconds(wave.spawnInterval);
				}
				yield return new WaitForSeconds(level.wavesInterval);
			}
		}
	}
}
