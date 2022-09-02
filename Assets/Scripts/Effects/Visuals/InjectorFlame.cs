using TowerDefence;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Effects.Visuals {
	[ExecuteInEditMode]
	public class InjectorFlame: MonoBehaviour {
		[SerializeField]
		private Transform cylinder;

		[Space]
		public float upadteInterval1 = 0.1f;
		public float upadteInterval2 = 0.2f;
		private readonly Timer timer1 = new Timer();
		private readonly Timer timer2 = new Timer();

		private void Update() {
			if(timer1.EverySeconds(upadteInterval1)) {
				cylinder.localEulerAngles = new Vector3(-90, Random.Range(0, 360f), 0);
			}
			if(timer2.EverySeconds(upadteInterval2)) {
				cylinder.localScale = new Vector3(cylinder.localScale.x, cylinder.localScale.y, Random.Range(0.25f, 0.35f));
			}
		}
	}
}
