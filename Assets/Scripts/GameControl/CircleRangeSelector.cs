using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TowerDefence.GameControl {
	[ExecuteInEditMode]
	public class CircleRangeSelector: MonoBehaviour {
		[SerializeField]
		private GameObject circle;
		[SerializeField]
		private GameObject centerArrows;
		[SerializeField]
		private GameObject arrow1;
		[SerializeField]
		private GameObject arrow2;
		[SerializeField]
		private GameObject arrow3;
		[SerializeField]
		private GameObject arrow4;

		[Range(1f, 10f)]
		public float radius;
		public Vector2 position;

		private void Update() {
			UpdateRadiusAndPosition(radius, position);
			UpdateCircleSway(0.25f, 3);
			RotateCenterArrows(Mathf.Sin(Time.time / 2));
			SwayCenterArrows(0.1f, 3f);
		}

		private void UpdateRadiusAndPosition(float radius, Vector2 position) {
			circle.transform.localScale = new Vector3(radius / 2, radius / 2, 40);
			transform.position = new Vector3(position.x, 0, position.y);
		}

		private void UpdateCircleSway(float centerY, float swaySpeed) {
			circle.transform.localPosition = new Vector3(0,
				Mathf.MoveTowards(
					circle.transform.localPosition.y,
					Mathf.Sin(Time.time * swaySpeed) * 0.1f + centerY,
					Time.deltaTime * 10
				)
			, 0);
		}

		private void RotateCenterArrows(float rotateSpeed) {
			centerArrows.transform.Rotate(0, rotateSpeed, 0, Space.Self);
		}

		private void SwayCenterArrows(float amplitude, float moveSpeed) {
			float distance = (Mathf.Sin(Time.time * moveSpeed) + 1) / 2 * amplitude;
			foreach(var item in new GameObject[] { arrow1, arrow2, arrow3, arrow4 }) {
				item.transform.localPosition = new Vector3(-distance, 0, -distance);
			}
		}
	}
}
