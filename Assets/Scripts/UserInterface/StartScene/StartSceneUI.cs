using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerDefence.UserInterface.StartScene {
	public class StartSceneUI: MonoBehaviour {
		public static StartSceneUI Instance { get; set; }

		[field: SerializeField]
		public GameObject TopPanel { get; private set; }
		[field: SerializeField]
		public StartScene_MiddlePanel MiddlePanel { get; private set; }
		[field: SerializeField]
		public StartScene_BottomPanel BottomPanel { get; private set; }
		[field: SerializeField]
		public StartScene_LoadingPanel LoadingPanel { get; private set; }


		private Coroutine loadingSceneCoroutine;

		private void Awake() {
			Instance = this;
		}

		private void Start() {
			TopPanel.gameObject.SetActive(true);
			MiddlePanel.gameObject.SetActive(true);
			BottomPanel.gameObject.SetActive(true);
			LoadingPanel.gameObject.SetActive(false);
		}

		public void LoadMainScene() {
			if(loadingSceneCoroutine != null) {
				return;
			}

			loadingSceneCoroutine = StartCoroutine(LoadMainSceneCoroutine());
		}

		private IEnumerator LoadMainSceneCoroutine() {
			TopPanel.gameObject.SetActive(false);
			MiddlePanel.gameObject.SetActive(false);
			BottomPanel.gameObject.SetActive(false);
			LoadingPanel.gameObject.SetActive(true);

			yield return null;

			AsyncOperation operation = SceneManager.LoadSceneAsync(1);
			while(!operation.isDone) {
				LoadingPanel.SetProgress(operation.progress);
				yield return new WaitForFixedUpdate();
			}

			loadingSceneCoroutine = null;
		}
	}
}
