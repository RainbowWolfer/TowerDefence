using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TowerDefence.UserInterface {
	public class FinancePanel: MonoBehaviour {
		[SerializeField]
		private TextMeshProUGUI cashText;

		public void UpdateCash(int cash) {
			cashText.text = $"${cash}";
		}
	}
}
