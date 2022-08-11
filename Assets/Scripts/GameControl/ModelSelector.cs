using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TowerDefence.GameControl {
	public class ModelSelector: MonoBehaviour {
		[SerializeField]
		private GameObject topB;
		[SerializeField]
		private GameObject topF;
		[SerializeField]
		private GameObject topL;
		[SerializeField]
		private GameObject topR;
		[SerializeField]
		private GameObject botB;
		[SerializeField]
		private GameObject botF;
		[SerializeField]
		private GameObject botL;
		[SerializeField]
		private GameObject botR;

		private ShowMode mode;
		public ShowMode Mode {
			get => mode;
			set {
				if(mode == value) {
					return;
				}
				mode = value;
				(bool top, bool bot) = value switch {
					ShowMode.Full => (true, true),
					ShowMode.Half => (false, true),
					ShowMode.None => (false, false),
					_ => throw new Exception("it cannot be wrong"),
				};
				topB.SetActive(top);
				topF.SetActive(top);
				topL.SetActive(top);
				topR.SetActive(top);
				botB.SetActive(bot);
				botF.SetActive(bot);
				botL.SetActive(bot);
				botR.SetActive(bot);
			}
		}

		public bool smoothTransition = true;
		public bool smoothTransitionInHeight = true;
		public bool shrinkAnimation = true;
		public float shrinkSize = 0.2f;
		public float shrinksSpeed = 0.2f;
		public bool floatingHeight = false;
		public float floatingMagnitude = 0.2f;
		public float floatingSpeed = 1;

		private Vector2Int coord;
		public Vector2Int Coord {
			get => coord;
			set {
				if(coord == value) {
					return;
				}
				coord = value;
				if(shrinkAnimation) {
					ExpandBorder();
				}
			}
		}
		public Vector2Int size = new Vector2Int(1, 1);

		private const float height = 0.215f;
		private float topHeight = 0;
		private float heightOffset = 0;
		private Vector3 cv1;
		private float cv2;
		//private Vector3 cv3;

		private void Update() {
			Transition();
			Shrink();
		}

		public void SetState(ShowMode mode, Vector2Int coord, float height, Vector2Int size) {
			Mode = mode;
			Coord = coord;
			SetHeight(height);
			SetSize(size);
		}
		public void SetState(ShowMode mode) {
			Mode = mode;
		}
		public void SetSize(Vector2Int size) {
			this.size = size;
		}

		public void SetHeight(float topHeight) {
			this.topHeight = Mathf.Clamp(topHeight - 1, 0, float.MaxValue);
		}

		private void Transition() {
			var pos = new Vector3(Coord.x, height, Coord.y);
			if(transform.position != pos) {
				if(smoothTransition) {
					transform.position = Vector3.SmoothDamp(transform.position, pos, ref cv1, 0.1f);
				} else {
					transform.position = pos;
				}
			}

			GameObject[] list = { topR, topL, topF, topB };
			heightOffset = floatingHeight ? Mathf.Sin(Time.time * floatingSpeed) * floatingMagnitude : 0;
			float h = topHeight + heightOffset;
			foreach(GameObject item in list) {
				if(item.transform.localPosition.y == h) {
					continue;
				}
				if(smoothTransitionInHeight) {
					item.transform.localPosition = new Vector3(item.transform.localPosition.x,
						Mathf.SmoothDamp(item.transform.localPosition.y, h, ref cv2, 0.1f),
					item.transform.localPosition.z);
				} else {
					item.transform.localPosition = new Vector3(item.transform.localPosition.x, h, item.transform.localPosition.z);
				}
			}


			if(floatingHeight) {

			}

		}

		private void Shrink() {
			GameObject[] list = { topB, topF, topL, topR, botB, botF, botL, botR, };
			foreach(var item in list) {
				var pos = new Vector3(0, item.transform.localPosition.y, 0);
				if(item == topR || item == botR) {
					pos = new Vector3(size.x - 1, pos.y, 0);
				} else if(item == topL || item == botL) {
					pos = new Vector3(0, pos.y, size.y - 1);
				} else if(item == topB || item == botB) {
					pos = new Vector3(size.x - 1, pos.y, size.y - 1);
				}
				if(item.transform.localPosition == pos) {
					continue;
				}
				if(shrinkAnimation) {
					item.transform.localPosition = Vector3.Lerp(item.transform.localPosition, pos, shrinksSpeed);
				} else {
					item.transform.localPosition = pos;
				}
			}
		}

		private void ExpandBorder() {
			botB.transform.localPosition = new Vector3(shrinkSize, 0, shrinkSize);
			botF.transform.localPosition = new Vector3(-shrinkSize, 0, -shrinkSize);
			botL.transform.localPosition = new Vector3(-shrinkSize, 0, shrinkSize);
			botR.transform.localPosition = new Vector3(shrinkSize, 0, -shrinkSize);

			topB.transform.localPosition = new Vector3(shrinkSize, topB.transform.localPosition.y, shrinkSize);
			topF.transform.localPosition = new Vector3(-shrinkSize, topF.transform.localPosition.y, -shrinkSize);
			topL.transform.localPosition = new Vector3(-shrinkSize, topL.transform.localPosition.y, shrinkSize);
			topR.transform.localPosition = new Vector3(shrinkSize, topR.transform.localPosition.y, -shrinkSize);
		}

		public enum ShowMode {
			Full, Half, None
		}
	}
}
