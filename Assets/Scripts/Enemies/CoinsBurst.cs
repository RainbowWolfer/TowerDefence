using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefence.Data;
using TowerDefence.Enemies;
using TowerDefence.Functions;
using UnityEngine;

namespace TowerDefence.Scripts.Enemies {
	public class CoinsBurst: MonoBehaviour {
		[SerializeField]
		private GameObject coinPrefab;

		[Space]
		public int coinCount;
		public float coinSize;
		public float radius = 2f;
		public float upForce = 1;

		[Space]
		public float interval = 0.5f;
		public float dieTime = 5f;

		private async void Start() {
			for(int i = 0; i < coinCount; i++) {
				var rb = Instantiate(coinPrefab, transform).GetComponent<Rigidbody>();

				rb.transform.localScale = Vector3.one * coinSize;

				Destroy(rb.gameObject, dieTime);

				rb.transform.SetPositionAndRotation(
					transform.position,
					Methods.RandomRotation()
				);

				rb.AddForce(
					Random.Range(-radius, radius),
					Random.Range(3, 5f) * upForce,
					Random.Range(-radius, radius),
					ForceMode.Impulse
				);

				rb.AddTorque(
					Random.Range(-70, 70),
					Random.Range(-70, 70),
					Random.Range(-70, 70) * Time.deltaTime,
					ForceMode.Impulse
				);

				await Task.Delay((int)(interval * 1000));
			}

			Destroy(gameObject, dieTime);
		}

		public void Set(int coin) {
			if(coin <= 50) {
				coinCount = 1;
				coinSize = 0.4f;
				upForce = 0.8f;
			} else if(coin <= 100) {
				coinCount = 2;
				coinSize = 0.6f;
				upForce = 1f;
			} else if(coin <= 150) {
				coinCount = 3;
				coinSize = 0.6f;
				upForce = 1f;
			} else if(coin <= 200) {
				coinCount = 4;
				coinSize = 0.8f;
				upForce = 1.3f;
			} else {
				coinCount = 5;
				coinSize = 0.8f;
				upForce = 1.4f;
			}
		}
	}
}
