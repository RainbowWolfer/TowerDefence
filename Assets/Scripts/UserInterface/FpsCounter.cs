using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace TowerDefence.UserInterface {
	public class FpsCounter: MonoBehaviour {
		public TextMeshProUGUI fps;
		private bool active;

		public bool Active {
			get => active;
			set {
				active = value;
				gameObject.SetActive(value);
			}
		}

		private void Start() {
			StartCoroutine(FPSUpdate());
		}

		private IEnumerator FPSUpdate() {
			while(true) {
				fps.text = ((int)(1f / Time.deltaTime)).ToString();
				yield return new WaitForSeconds(0.4f);
			}
		}
	}
}
