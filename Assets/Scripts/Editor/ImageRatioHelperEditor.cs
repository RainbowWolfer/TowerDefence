using TowerDefence.UserInterface.Helpers;
using UnityEditor;
using UnityEngine;

namespace TowerDefence.Editors {

	[CustomEditor(typeof(ImageRatioHelper))]
	public class ImageRatioHelperEditor: Editor {
		public override void OnInspectorGUI() {
			base.OnInspectorGUI();
			if(GUILayout.Button("Match Height")) {
				
			}

		}
	}
}
