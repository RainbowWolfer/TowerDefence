using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace TowerDefence.UserInterface.StartScene.Blocks {
	public class RewardsPanel: MonoBehaviour {
		public RectTransform Rt => transform as RectTransform;

		[SerializeField]
		private GameObject rank;
		[SerializeField]
		private GameObject card;
		[SerializeField]
		private GameObject diamond;

		[SerializeField]
		private TextMeshProUGUI rankText;
		[SerializeField]
		private TextMeshProUGUI cardText;
		[SerializeField]
		private TextMeshProUGUI diamondText;

		public int ranks;
		public int cards;
		public int diamonds;

		private void Update() {
			rank.SetActive(ranks != 0);
			rankText.text = $"{ranks}";

			card.SetActive(ranks != 0);
			cardText.text = $"{cards}";

			diamond.SetActive(diamonds != 0);
			diamondText.text = $"{diamonds}";
		}

	}
}
