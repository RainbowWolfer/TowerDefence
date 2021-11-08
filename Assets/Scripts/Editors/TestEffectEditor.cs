using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TowerDefence.Editors {
	[CustomEditor(typeof(EffectTest))]
	public class TestEffectEditor: Editor {
		public override void OnInspectorGUI() {
			base.OnInspectorGUI();
			if(GUILayout.Button("Test")) {
				var t = target as EffectTest;
				t.effect.SetFloat(t.e_name, t.e_value);
			}
		}
	}
}