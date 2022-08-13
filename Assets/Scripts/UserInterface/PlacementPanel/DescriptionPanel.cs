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
		[SerializeField]
		private TextMeshProUGUI shovelPriceText;

		[SerializeField]
		private PointEventHandler shovelButton;
		[SerializeField]
		private Outline shovelButton_outline;

		public float descriptionWidth;
		public override float Width => 500;

		protected override void Awake() {
			base.Awake();
			shovelButton.MouseEnter += s => shovelButton_outline.enabled = true;
			shovelButton.MouseExit += s => shovelButton_outline.enabled = false;
		}

		public override void Initialize(Placement placement, PlacementPanelManager manager) {
			base.Initialize(placement, manager);
			if(placement is EnvironmentCube ec) {
				this.environmentCube = ec;
				shovelPriceText.text = $"${ec.info.shovelPrice}";

				shovelButton.MouseUp += s => {
					ec.Shovel();
				};
			} else {
				throw new Exception($"{nameof(placement)} type cast error");
			}
		}

		public void UpdateLayout() {

		}
	}
}
