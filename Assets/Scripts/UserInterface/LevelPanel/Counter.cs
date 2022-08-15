using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence.UserInterface.LevelPanel {
	public class Counter: MonoBehaviour {
		[SerializeField]
		private Animator anim;
		[SerializeField]
		private GameObject background;

		[Space]
		[SerializeField]
		private Image border;
		[SerializeField]
		private Outline outline;
		[SerializeField]
		private Image filler;
		[SerializeField]
		private TextMeshProUGUI title;
		[SerializeField]
		private TextMeshProUGUI content;
		[SerializeField]
		private TextMeshProUGUI max;

		[field: Space]
		[field: SerializeField]
		public bool IsMouseOn { get; private set; }


		private void Update() {
			IsMouseOn = UIRayCaster.HasElement(background);
			anim.SetBool("IsActive", IsMouseOn);
		}
	}
}
