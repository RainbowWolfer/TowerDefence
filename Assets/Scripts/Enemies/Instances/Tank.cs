using UnityEngine;
using Random = UnityEngine.Random;

namespace TowerDefence.Enemies.Instances {
	public class Tank: Enemy {
		[Space]
		[SerializeField]
		private Animator anim;

		private readonly Timer timer = new Timer();
		private float randomInterval;
		private bool firstTime = true;

		protected override void Start() {
			base.Start();
			randomInterval = Random.Range(2, 7f);
			anim.SetFloat("A1Speed", Random.Range(0.5f, 1.5f));
			anim.SetFloat("A2Speed", Random.Range(0.5f, 1.5f));
			anim.SetFloat("A3Speed", Random.Range(0.5f, 1.5f));
		}

		protected override void Update() {
			base.Update();
			if(timer.EverySeconds(randomInterval)) {
				if(firstTime) {
					firstTime = false;
					return;
				}
				randomInterval = Random.Range(2, 7f);
				float random = Random.Range(0, 100f);
				if(random < 25) {
					anim.SetTrigger("A1");
				} else if(random < 50) {
					anim.SetTrigger("A2");
				} else if(random < 75) {
					anim.SetTrigger("A3");
				} else {

				}
			}
		}
	}
}
