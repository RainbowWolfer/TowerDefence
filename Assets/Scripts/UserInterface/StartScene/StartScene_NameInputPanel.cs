using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence.UserInterface.StartScene {
	public class StartScene_NameInputPanel: MonoBehaviour {
		[SerializeField]
		private TMP_InputField input;
		[SerializeField]
		private Image inputBackground;
		[SerializeField]
		private AboutDetailButton confirmButton;
		[SerializeField]
		private TextMeshProUGUI errorText;

		private bool IsMouseOnInput { get; set; }

		private void Awake() {
			confirmButton.OnClick = () => OnConfirmInput();
			errorText.text = "";
		}

		private void Update() {
			IsMouseOnInput = UIRayCaster.HasElement(inputBackground.gameObject);
			inputBackground.color = Color.Lerp(inputBackground.color,
				IsMouseOnInput ? new Color(1f, 1f, 1f) : new Color(0.063f, 0.65f, 1f),
			Time.deltaTime * 15);

		}

		public string GetText() => input.text.Trim();

		private void OnConfirmInput() {
			if(string.IsNullOrWhiteSpace(GetText())) {
				errorText.text = "Username cannot be empty";
				return;
			}
			StartSceneUI.Instance.RegisterNewUser(GetText());
		}

		public void OnTextChanged() {
			errorText.text = "";
		}
	}
}
