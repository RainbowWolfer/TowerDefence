using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TowerDefence.Effects {
	public class LightningStorm: MonoBehaviour {
		[SerializeField]
		private GameObject lightningPrefab;

		[field: SerializeField]
		[field: Range(1, 12f)]
		public float Radius { get; set; }

		[field: SerializeField]
		public int StrikeCount { get; set; }

		[field: SerializeField]
		public float StrikeInterval { get; set; }

		//private float height = 4;

		public float LightningEffectRange { get; set; } = 1;
		public float LightningDamage { get; set; } = 250;

		public void Start() {
			StartCoroutine(Begin());
		}

		private IEnumerator Begin() {
			for(int i = 0; i < StrikeCount; i++) {
				float x = Random.Range(-Radius, Radius);
				float y = Mathf.Sqrt(Radius * Radius - x * x) * (Random.Range(0, 2) == 1 ? 1 : -1);
				GameObject obj = Instantiate(lightningPrefab, transform);
				obj.transform.localPosition = new Vector3(x, 0, y);
				StartCoroutine(DoDamage(new Vector2(
					x + transform.position.x,
					y + transform.position.z
				)));
				yield return new WaitForSeconds(StrikeInterval);
			}
			Destroy(gameObject);
		}

		private IEnumerator DoDamage(Vector2 coord) {
			yield return new WaitForSeconds(0.15f);
			Game.Instance.EnemiesTakeAreaDamageV2(coord, LightningEffectRange, LightningDamage);
			//GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
			//obj.transform.localScale = Vector3.one * 0.3f;
			//obj.transform.position = new Vector3(coord.x, 0.4f, coord.y);
		}
	}
}
