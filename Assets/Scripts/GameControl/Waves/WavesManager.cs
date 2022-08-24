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
		public static WavesManager Instance { get; private set; }
		public static StageLevel CurrentLevel => Instance?.levels[Instance.currentLevelInt];

		public List<StageLevel> levels;
		private int maxEscapes;
		private int currentEscapes;
		public int currentLevelInt = 0;//start from 1

		private Coroutine levelCoroutine;

		public bool IsSpawningEnemies { get; private set; } = false;
		public bool LevelGoing { get; private set; } = false;
		public int CurrentEscapes {
			get => currentEscapes;
			set {
				currentEscapes = value;
				if(currentEscapes > MaxEscapes) {

				}
			}
		}

		public int MaxEscapes {
			get => maxEscapes;
			set => maxEscapes = Mathf.Clamp(value, 0, 999);
		}

		//percentage
		public float healthMultiplier = 1f;

		private void Awake() {
			Instance = this;
		}

		private void Start() {

		}

		public async void StartGame() {
			if(levels == null || levels.Count == 0 || levelCoroutine != null) {
				return;
			}
			currentLevelInt = 1;

			await Task.Delay(500);
			await UI.Instance.incomingPanel.PopupStart();

			levelCoroutine = StartCoroutine(StartLevels(levels));
		}

		public void StopGame() {
			if(levelCoroutine != null) {
				StopCoroutine(levelCoroutine);
			}
			levelCoroutine = null;
		}

		private IEnumerator StartLevels(List<StageLevel> levels) {
			LevelGoing = true;
			CurrentEscapes = 0;
			for(int i = 0; i < levels.Count; i++) {
				StageLevel level = levels[i];
				//update values
				currentLevelInt = i + 1;
				//popup ui stuff
				UI.Instance.incomingPanel.UpdateCount(level.waves);
				UI.Instance.incomingPanel.PopupLevel(currentLevelInt, level, level.readyTime);
				UI.Instance.levelPanel.Levels.Set(currentLevelInt);
				UI.Instance.levelPanel.Escapes.Set(CurrentEscapes, MaxEscapes);
				UI.Instance.levelPanel.Waves.Set(0);
				yield return new WaitForSeconds(level.readyTime);
				IsSpawningEnemies = true;
				yield return CreateLevelWaves(level);
				//NotificationPanel.EarnDiamonds(10);
				if(i != levels.Count - 1) {//not the last level
					yield return new WaitForSeconds(level.finishWaitTime);
					IsSpawningEnemies = false;
				}
			}
			//pop up finish ui
			//wait until enemies is empty
			yield return new WaitUntil(() => Game.Instance.enemies.Count == 0);
			IsSpawningEnemies = false;

			UI.Instance.incomingPanel.PopupFinish();


			levelCoroutine = null;
			LevelGoing = false;
		}

		public static void UpdateEscapes() {
			if(Instance == null) {
				return;
			}
			UI.Instance.levelPanel.Escapes.Set(Instance.CurrentEscapes, Instance.MaxEscapes);
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
				if(i != level.waves.Count - 1) {
					yield return new WaitForSeconds(level.wavesInterval);
				}
			}
		}
	}
}
