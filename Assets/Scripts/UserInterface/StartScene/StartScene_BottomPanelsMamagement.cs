using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TowerDefence.UserInterface.StartScene {
	public class StartScene_BottomPanelsMamagement: MonoBehaviour {
		public StartScene_BottomPanel parent;

		[SerializeField]
		private RectTransform settings;
		[SerializeField]
		private RectTransform user;

		private void Update() {
			
			switch(parent.panelType) {
				case StartScene_BottomPanel.PanelType.None:
					break;
				case StartScene_BottomPanel.PanelType.Settings:
					break;
				case StartScene_BottomPanel.PanelType.User:
					break;
				default:
					break;
			}
		}
	}
}
