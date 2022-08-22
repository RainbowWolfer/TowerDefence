using UnityEngine;

namespace TowerDefence.Placements.Towers {
	public interface ITurret {
		float HorAngle { get; set; }
		float VerAngle { get; set; }

		float FreeTimeTurningSpeed { get; }
		Vector3 RandomFreePoint { get; set; }

		void CalculateAngles(Vector3 target);
		void UpdateModel();
		bool CheckAimingReady();



	}
}
