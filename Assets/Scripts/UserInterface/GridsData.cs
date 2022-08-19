using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TowerDefence.UserInterface {
	public class GridsData: MonoBehaviour {
		[SerializeField]
		private GameObject fill1;
		[SerializeField]
		private GameObject fill2;
		[SerializeField]
		private GameObject fill3;
		[SerializeField]
		private GameObject fill4;
		[SerializeField]
		private GameObject fill5;

		[Range(0, 5)]
		public int data;

		private void OnValidate() {
			Set(data);
		}

		public void Set(int data) {
			data = Mathf.Clamp(data, 0, 5);
			fill1.SetActive(data >= 1);
			fill2.SetActive(data >= 2);
			fill3.SetActive(data >= 3);
			fill4.SetActive(data >= 4);
			fill5.SetActive(data >= 5);
		}
	}
}
