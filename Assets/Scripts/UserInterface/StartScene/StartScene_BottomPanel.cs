using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence.UserInterface.StartScene {
	public class StartScene_BottomPanel: MonoBehaviour {
		public RectTransform Rt => transform as RectTransform;
		private StartScene_MiddlePanel MiddlePanel => StartSceneUI.Instance.MiddlePanel;


		[SerializeField]
		private SettingsPanel settingsPanel;

		[Space]
		[SerializeField]
		private GameObject[] settingsButtonBackgrounds;
		public bool IsMouseOnSettingsButton { get; private set; }

		[SerializeField]
		private GameObject[] userButtonBackgrounds;
		public bool IsMouseOnUserButton { get; private set; }

		[Space]
		[SerializeField]
		private GameObject background;
		public bool IsMouseOnPanel { get; private set; }


		[Space]
		[SerializeField]
		private TextMeshProUGUI settingsText;
		[SerializeField]
		private TextMeshProUGUI userText;
		[SerializeField]
		private AboutDetailButton github;
		[SerializeField]
		private AboutDetailButton homepage;

		[Space]
		[SerializeField]
		private HorizontalScroll userCardsScroll;

		//[field: Space]
		//[field: SerializeField]
		//public bool IsSettingsOpen { get; set; }
		//[field: SerializeField]
		//public bool IsUserOpen { get; set; }

		[Space]
		public PanelType panelType = PanelType.None;
		public bool Show { get; set; }

		private void Awake() {
			settingsPanel.openPanelIndexes = new int[] { 1, 2 };
			settingsPanel.ButtonSelector.SetButtonsAction(
				() => {
					settingsPanel.ShowSettings();
				}, () => {
					settingsPanel.ShowAbout();
				}, () => {
					Application.Quit();
				}
			);

			github.OnClick = () => {
				Application.OpenURL("https://www.github.com");
			};

			homepage.OnClick = () => {
				Application.OpenURL("https://rainbowwolfer.github.io");
			};

			Show = true;
		}

		private void OnEnable() {
			Rt.anchoredPosition = new Vector2(0, -100);
		}

		private void Update() {
			IsMouseOnSettingsButton = UIRayCaster.HasElements(settingsButtonBackgrounds);
			IsMouseOnUserButton = UIRayCaster.HasElements(userButtonBackgrounds);
			IsMouseOnPanel = UIRayCaster.HasElement(background);

			foreach(GameObject item in settingsButtonBackgrounds) {
				if(item.TryGetComponent(out Image image)) {
					image.color = Color.Lerp(image.color, IsMouseOnSettingsButton ? Color.gray : Color.black, Time.deltaTime * 15);
				}
			}
			foreach(GameObject item in userButtonBackgrounds) {
				if(item.TryGetComponent(out Image image)) {
					image.color = Color.Lerp(image.color, IsMouseOnUserButton ? Color.gray : Color.black, Time.deltaTime * 15);
				}
			}

			settingsText.text = panelType == PanelType.Settings ? "> Settings <" : "Settings";
			userText.text = panelType == PanelType.User ? "> User <" : "User";

			if(IsMouseOnSettingsButton && Input.GetMouseButtonUp(0)) {
				if(panelType == PanelType.Settings) {
					panelType = PanelType.None;
				} else {
					panelType = PanelType.Settings;
				}
			}
			if(IsMouseOnUserButton && Input.GetMouseButtonUp(0)) {
				if(panelType == PanelType.User) {
					panelType = PanelType.None;
				} else {
					panelType = PanelType.User;
				}
			}

			if(!IsMouseOnUserButton && !IsMouseOnSettingsButton && !IsMouseOnPanel && Input.GetMouseButtonUp(0) && panelType != PanelType.None) {
				MiddlePanel.ResetDelayedActionTimer();
				panelType = PanelType.None;
			}

			if(Input.GetKeyDown(KeyCode.Escape)) {
				panelType = PanelType.None;
			}

			userCardsScroll.EnableScroll = panelType == PanelType.User;

			Rt.anchoredPosition = new Vector2(0,
				Mathf.Lerp(
					Rt.anchoredPosition.y,
					Show ? (panelType != PanelType.None ? 600 : 0) : -100,
					Time.deltaTime * 15
				)
			);
		}


		public enum PanelType {
			None, Settings, User
		}
	}
}
