using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence.UserInterface.StartScene {
	public class StartScene_LoadingPanel: MonoBehaviour {
		[SerializeField]
		private Image progressFiller;

		private void Start() {
			progressFiller.fillAmount = 0;
		}

		private void Update() {

		}

		public void SetProgress(float progress) {
			progressFiller.fillAmount = progress;
		}
	}
}
