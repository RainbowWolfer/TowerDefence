using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using TowerDefence.Placements;
using UnityEngine.UI;
using TowerDefence.Placements.Environments;

namespace TowerDefence.UserInterface {
	public class DescriptionPanel: PlacementPanel {
		private EnvironmentCube environmentCube;
		//public override Placement CurrentPlacement => environmentCube;

		[SerializeField]
		private TextMeshProUGUI titleText;
		[SerializeField]
		private TextMeshProUGUI descriptionText;

		[Space]
		[SerializeField]
		private IconButton shovelButton;

		public bool ShovelAvailable => Level.Cash >= TargetPlacement.info.shovelPrice;

		public float descriptionWidth;
		public override float Width => 500;

		protected override void Awake() {
			base.Awake();
		}

		public override void Initialize(Placement placement, PlacementPanelManager manager) {
			base.Initialize(placement, manager);
			UpdateTexts();
			if(placement is EnvironmentCube ec) {
				this.environmentCube = ec;
				shovelButton.Text.text = $"${ec.info.shovelPrice}";

				shovelButton.OnClick = s => {
					if(ShovelAvailable) {
						ec.Shovel();
					}
				};
				shovelButton.ExternalUpdate = (icon, outline, text) => {
					if(ShovelAvailable) {
						text.color = Color.white;
						outline.effectColor = new Color(0.6f, 0.6f, 0.6f, 0.5f);
					} else {
						text.color = Color.red;
						outline.effectColor = Color.red;
					}
				};
			} else {
				throw new Exception($"{nameof(placement)} type cast error");
			}
		}

		public void UpdateTexts() {
			titleText.text = $"{TargetPlacement.info.TowerName.Replace(' ', '_')} >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>";
			descriptionText.text = TargetPlacement.info.description;
		}
	}
}
