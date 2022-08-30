using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

namespace TowerDefence.UserInterface {
	public class PowersPanel: MonoBehaviour {
		[SerializeField]
		private Image[] blocks;
		[SerializeField]
		private TextMeshProUGUI text;

		[Space]
		public int maxPowersDisplay = 100;
		public float orangeThreshold = 0.1f;

		[Space]
		private float d_current;
		private float d_powers;
		public int current;
		public int powers;
		public float transitionTime = 1f;

		[Space]
		public Color red;
		public Color orange;
		public Color green;

		private float cv1;
		private float cv2;

		private void Update() {
			current = Game.Instance.level.CurrentPowers;
			powers = Game.Instance.level.MaxPowers;

			d_current = Mathf.SmoothDamp(d_current, current, ref cv1, transitionTime);
			d_powers = Mathf.SmoothDamp(d_powers, powers, ref cv2, transitionTime);

			text.text = $"{current}/{powers}";

			for(int i = 0; i < blocks.Length; i++) {
				float per = i / (float)blocks.Length;
				float limit = Mathf.Clamp(d_powers / maxPowersDisplay, 0, 1f);
				float cur = d_current / d_powers;

				blocks[i].gameObject.SetActive(per <= limit);

				if(per / limit <= cur) {
					if(current != 0 && cur - per / limit < orangeThreshold) {
						blocks[i].color = orange;
					} else {
						blocks[i].color = red;
					}
				} else {
					blocks[i].color = green;
				}

			}
		}
	}
}
