using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence.UserInterface {
	public class IconButton: MonoBehaviour {
		public string title;

		[SerializeField]
		private GameObject background;
		[SerializeField]
		private TextMeshProUGUI titleText;
		[SerializeField]
		private Animator anim;

		[field: SerializeField]
		public Image Icon { get; set; }
		[field: SerializeField]
		public Outline Outline { get; set; }
		[field: SerializeField]
		public TextMeshProUGUI Text { get; set; }


		public bool IsMouseOn { get; private set; }

		public Action<IconButton> OnClick { get; set; } = null;

		public Action<Image, Outline, TextMeshProUGUI> ExternalUpdate { get; set; }

		private void Start() {
			titleText.text = title;
		}

		private void Update() {
			IsMouseOn = UIRayCaster.HasElement(background);

			Outline.enabled = IsMouseOn;

			anim.SetBool("On", IsMouseOn);

			if(IsMouseOn && Input.GetMouseButtonUp(0)) {
				OnClick?.Invoke(this);
			}

			ExternalUpdate.Invoke(Icon, Outline, Text);
		}

	}
}
