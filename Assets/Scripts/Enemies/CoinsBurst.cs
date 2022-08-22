using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
					Random.Range(3, 5f),
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

		}

	}
}
