using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence.UserInterface.StartScene {
	public class StartScene_BottomPanel: MonoBehaviour {
		public RectTransform Rt => transform as RectTransform;

		[SerializeField]
		private StartScene_BottomPanelsMamagement panelsMamagement;
		[SerializeField]
		private SettingsPanel settingsPanel;

		[Space]
		[SerializeField]
		private GameObject[] settingsButtonBackgrounds;
		private bool isMouseOnSettingsButton;
		[SerializeField]
		private GameObject[] userButtonBackgrounds;
		private bool isMouseOnUserButton;


		[Space]
		[SerializeField]
		private TextMeshProUGUI settingsText;
		[SerializeField]
		private TextMeshProUGUI userText;
		[SerializeField]
		private AboutDetailButton github;
		[SerializeField]
		private AboutDetailButton homepage;

		//[field: Space]
		//[field: SerializeField]
		//public bool IsSettingsOpen { get; set; }
		//[field: SerializeField]
		//public bool IsUserOpen { get; set; }

		public PanelType panelType = PanelType.None;

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
		}

		private void Update() {
			isMouseOnSettingsButton = UIRayCaster.HasElements(settingsButtonBackgrounds);
			isMouseOnUserButton = UIRayCaster.HasElements(userButtonBackgrounds);

			foreach(GameObject item in settingsButtonBackgrounds) {
				if(item.TryGetComponent(out Image image)) {
					image.color = Color.Lerp(image.color, isMouseOnSettingsButton ? Color.gray : Color.black, Time.deltaTime * 15);
				}
			}
			foreach(GameObject item in userButtonBackgrounds) {
				if(item.TryGetComponent(out Image image)) {
					image.color = Color.Lerp(image.color, isMouseOnUserButton ? Color.gray : Color.black, Time.deltaTime * 15);
				}
			}

			settingsText.text = panelType == PanelType.Settings ? "> Settings <" : "Settings";
			userText.text = panelType == PanelType.User ? "> User <" : "User";

			if(isMouseOnSettingsButton && Input.GetMouseButtonUp(0)) {
				if(panelType == PanelType.Settings) {
					panelType = PanelType.None;
				} else {
					panelType = PanelType.Settings;
				}
			}
			if(isMouseOnUserButton && Input.GetMouseButtonUp(0)) {
				if(panelType == PanelType.User) {
					panelType = PanelType.None;
				} else {
					panelType = PanelType.User;
				}
			}

			Rt.anchoredPosition = new Vector2(0,
				Mathf.Lerp(
					Rt.anchoredPosition.y,
					panelType != PanelType.None ? 600 : 0,
					Time.deltaTime * 15
				)
			);
		}


		public enum PanelType {
			None, Settings, User
		}
	}
}
