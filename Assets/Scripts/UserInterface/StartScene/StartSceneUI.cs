using System.Collections;
using TowerDefence.Local;
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
		[field: SerializeField]
		public StartScene_NameInputPanel NameInputPanel { get; private set; }


		private Coroutine loadingSceneCoroutine;

		private void Awake() {
			Instance = this;
		}

		private void Start() {
			TopPanel.gameObject.SetActive(false);
			MiddlePanel.gameObject.SetActive(false);
			BottomPanel.gameObject.SetActive(false);
			LoadingPanel.gameObject.SetActive(false);
			NameInputPanel.gameObject.SetActive(false);

			Player player = Player.FromJson(PlayerPrefs.GetString("UserJson", ""));
			if(player != null) {
				Player.Current = player;
				StartPanels();
			} else {
				StartNameInput();
			}
		}

		public void StartNameInput() {
			TopPanel.gameObject.SetActive(true);
			MiddlePanel.gameObject.SetActive(false);
			BottomPanel.gameObject.SetActive(false);
			LoadingPanel.gameObject.SetActive(false);
			NameInputPanel.gameObject.SetActive(true);
		}

		public void StartPanels() {
			TopPanel.gameObject.SetActive(true);
			MiddlePanel.gameObject.SetActive(true);
			BottomPanel.gameObject.SetActive(true);
			LoadingPanel.gameObject.SetActive(false);
			NameInputPanel.gameObject.SetActive(false);
		}

		public void RegisterNewUser(string username) {
			Player.Current = new Player(username);
			PlayerPrefs.SetString("UserJson", Player.ToJson());
			StartPanels();
		}

		private void Update() {
			if(Input.GetKeyDown(KeyCode.Delete)) {
				PlayerPrefs.DeleteAll();
			}
			if(Player.Current != null) {
				if(Input.GetKeyDown(KeyCode.Q)) {
					Player.Current.diamond += 500;
				} else if(Input.GetKeyDown(KeyCode.W)) {
					Player.Current.diamond -= 40;
				} else if(Input.GetKeyDown(KeyCode.E)) {
					Player.Current.cards.emplacementsCooldown.cardsCount += 40;
				}
			}
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
