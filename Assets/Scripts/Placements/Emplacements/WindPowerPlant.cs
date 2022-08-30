
using TowerDefence.Functions;
using TowerDefence.Scripts.Data;
using UnityEngine;

namespace TowerDefence.Placements {
	public class WindPowerPlant: Emplacement {
		public override EmplacementAbility Ability {
			get => null;
			set { return; }
		}

		[Space]
		[SerializeField]
		private Transform windmill1;
		[SerializeField]
		private Transform windmill2;

		[Space]
		[SerializeField]
		private float baseRotateSpeed = 5;
		private float rotateSpeedMultiplier1 = 1;
		private float rotateSpeedMultiplier2 = 1;

		private readonly LoopTimer rotateRefreshTimer1 = new LoopTimer(10, 2, 20);
		private readonly LoopTimer rotateRefreshTimer2 = new LoopTimer(10, 2, 20);

		protected override void Start() {
			base.Start();
			windmill1.localEulerAngles = new Vector3(-90, 0, Random.Range(0, 360));
			windmill2.localEulerAngles = new Vector3(-90, 0, Random.Range(0, 360));
			rotateRefreshTimer1.RandomizeInterval();
			rotateRefreshTimer2.RandomizeInterval();
		}

		protected override void Update() {
			base.Update();
			windmill1.Rotate(0, 0, baseRotateSpeed * rotateSpeedMultiplier1 * Time.deltaTime);
			windmill2.Rotate(0, 0, baseRotateSpeed * rotateSpeedMultiplier2 * Time.deltaTime);

			if(rotateRefreshTimer1.EveryTime(true)) {
				rotateSpeedMultiplier1 = Random.Range(0.5f, 1.5f);
			}
			if(rotateRefreshTimer2.EveryTime(true)) {
				rotateSpeedMultiplier2 = Random.Range(0.5f, 1.5f);
			}
		}

		public override void Upgrade() {
			base.Upgrade();
		}

		public override void Fire(Vector2 coord) {

		}
	}
}
