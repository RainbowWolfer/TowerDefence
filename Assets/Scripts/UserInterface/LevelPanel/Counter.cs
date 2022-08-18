using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence.UserInterface.LevelPanels {
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
		[field: SerializeField]
		public bool EnableMouseOn { get; private set; }
		[SerializeField]
		private bool fillerClockwise;

		public float defaultPercentage = 1;
		private float percentage;
		private float speed = 5;

		private void Update() {
			IsMouseOn = EnableMouseOn && UIRayCaster.HasElement(background);
			anim.SetBool("IsActive", IsMouseOn);

			filler.fillAmount = Mathf.Lerp(filler.fillAmount, percentage, Time.deltaTime * speed);
		}

		public void Set(int current, int? max = null) {
			content.text = $"{current}";
			if(max == null) {
				EnableMouseOn = false;
				percentage = defaultPercentage;
				this.max.text = "";
			} else {
				EnableMouseOn = true;
				if(fillerClockwise) {
					percentage = 1 - current / (float)max;
				} else {
					percentage = current / (float)max;
				}
				this.max.text = max.ToString();
			}
		}
	}
}
