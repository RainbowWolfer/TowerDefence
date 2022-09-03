using System.Linq;
using System.Runtime.ConstrainedExecution;
using TowerDefence.Scripts.Data;
using UnityEngine;

namespace TowerDefence {
	public class CameraController: MonoBehaviour {
		public static CameraController Instance { get; private set; }

		public Camera mainCamera;
		public Camera uiCamera;

		[Space]
		[SerializeField]
		private Transform parent;
		[SerializeField]
		private GameObject ground;

		[Header("Zoom")]
		public float sizeOffset;
		public float sizeSpeed = 10;
		public Range<float> sizeOffsetRange;

		public bool IsMouseOnTop { get; private set; }
		public bool IsMouseOnBottom { get; private set; }
		public bool IsMouseOnLeft { get; private set; }
		public bool IsMouseOnRight { get; private set; }

		[Header("Move")]
		public float edgeThresholdPercentage = 0.01f;

		[Header("Sensitivity")]
		[Range(0, 1f)]
		public float horizontalSensitivity = 1f;
		[Range(0, 1f)]
		public float verticalSensitivity = 1f;

		private bool rightMouseDrag;
		private Vector3 startMousePosition;

		private void Awake() {
			Instance = this;
		}

		private void Start() {
			parent.localPosition = new Vector3(
				parent.localPosition.x,
				0.2f * parent.eulerAngles.x - 2f,
				parent.localPosition.z
			);
		}

		public void InitializeCamera() {
			Vector2Int size = Game.Instance.level.GetMapSize();
			transform.position = new Vector3(size.x, 0, size.y) / 2f;
		}


		private float cv1;
		private Vector3 cv2;
		private float cv3;

		private void Update() {
			Vector2Int size = Game.Instance.level.GetMapSize();

			int great = size.x > size.y ? size.x : size.y;

			//zoom
			sizeOffset = Mathf.Clamp(sizeOffset - sizeSpeed * Input.mouseScrollDelta.y, sizeOffsetRange.from, sizeOffsetRange.to);

			mainCamera.orthographicSize = Mathf.SmoothDamp(
				mainCamera.orthographicSize,
				Mathf.Clamp(great * 0.4f + 0.2f + sizeOffset, 1, float.MaxValue),
				ref cv1, 0.1f
			);

			float heightThreshold = Screen.height * edgeThresholdPercentage;
			float widthThreshold = Screen.width * edgeThresholdPercentage;

			IsMouseOnTop = Input.mousePosition.y >= Screen.height - heightThreshold;
			IsMouseOnBottom = Input.mousePosition.y <= heightThreshold;
			IsMouseOnRight = Input.mousePosition.x >= Screen.width - widthThreshold;
			IsMouseOnLeft = Input.mousePosition.x <= widthThreshold;

			//move
			float moveSpeed = 35f;
			Vector3 direction = Vector3.zero;
			if(!rightMouseDrag) {
				if(IsMouseOnTop) {
					direction += parent.forward;
				}
				if(IsMouseOnBottom) {
					direction -= parent.forward;
				}
				if(IsMouseOnLeft) {
					direction -= parent.right;
				}
				if(IsMouseOnRight) {
					direction += parent.right;
				}
				direction.y = 0;
				direction.Normalize();

				//direction.x *= horizontalSensitivity;
				//direction.z *= verticalSensitivity;
			}

			Vector3 targetPosition = transform.position + moveSpeed * Time.deltaTime * direction;
			targetPosition.x = Mathf.Clamp(targetPosition.x, -1, size.x);
			targetPosition.z = Mathf.Clamp(targetPosition.z, -1, size.y);

			transform.position = Vector3.SmoothDamp(
				transform.position, targetPosition,
				ref cv2, 0.1f
			);

			//rotate
			float rotateSpeed = 35;
			if(Input.GetMouseButtonDown(1)) {
				startMousePosition = Input.mousePosition;
				rightMouseDrag = true;
			}
			if(Input.GetMouseButtonUp(1)) {
				rightMouseDrag = false;
			}
			if(Input.GetMouseButton(1) && rightMouseDrag) {
				Vector3 offset = Input.mousePosition - startMousePosition;
				offset.x *= horizontalSensitivity;
				offset.y *= verticalSensitivity;

				transform.Rotate(0, offset.x * Time.deltaTime * rotateSpeed, 0);

				float ver = parent.eulerAngles.x - offset.y * Time.deltaTime * rotateSpeed;
				ver = Mathf.Clamp(ver, 20, 50);
				parent.eulerAngles = new Vector3(
					Mathf.SmoothDamp(
						parent.eulerAngles.x, ver,
						ref cv3, 0.1f
					),
					parent.eulerAngles.y,
					parent.eulerAngles.z
				);

				float height = 0.2f * parent.eulerAngles.x - 2f;
				parent.localPosition = new Vector3(
					parent.localPosition.x,
					height,
					parent.localPosition.z
				);

				startMousePosition = Input.mousePosition;
			}

			if(Input.GetMouseButtonUp(2)) {//middle mouse reset camera;
				sizeOffset = 0;
				transform.eulerAngles = new Vector3();
				parent.eulerAngles = new Vector3(
					30,
					parent.eulerAngles.y,
					parent.eulerAngles.z
				);
				parent.localPosition = new Vector3(
					parent.localPosition.x,
					0.2f * parent.eulerAngles.x - 2f,
					parent.localPosition.z
				);
				InitializeCamera();
			}
		}

		public void AdjustPosition(int width, int height) {
			Vector3 pos = new Vector3(width, 8, height);
			if(width > height) {
				pos.x += width / 2;
			} else if(width < height) {
				pos.y += height / 2;
			}
			transform.position = pos;
		}

	}
}
