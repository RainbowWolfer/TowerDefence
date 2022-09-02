using UnityEngine;

namespace TowerDefence.UserInterface {
	public class PlacementIcon: SelectionBaseIcon {
		[Space]
		[SerializeField]
		private DamageBasedAttributesPanel detailsPanel;


		protected override void Awake() {
			base.Awake();
			OnInfoUpdated += info => {
				detailsPanel.Set(info);
			};
		}

		protected override void Start() {
			base.Start();
		}

		protected override void Update() {
			base.Update();
		}
	}
}
