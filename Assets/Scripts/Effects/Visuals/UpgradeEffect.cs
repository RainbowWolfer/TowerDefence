using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.VFX;

namespace TowerDefence.Effects.Visuals {
	public class UpgradeEffect: MonoBehaviour {
		[SerializeField]
		private Transform textParent;
		[SerializeField]
		private VisualEffect effect;
		[SerializeField]
		private TextOutline outline;
		[SerializeField]
		private TextMesh text;
		[SerializeField]
		private SpriteRenderer tool;

		public bool show;
		public float targetHeight;

		private float cv1;


		private async void Start() {
			text.color = new Color(0, 0.8f, 1f, 0f);
			text.transform.localPosition = new Vector3();
			show = true;
			await Task.Delay(2000);
			show = false;
			//await Task.Delay(3000);
			Destroy(gameObject, 2000);
		}

		private void Update() {
			textParent.LookAt(CameraController.Instance.mainCamera.transform);

			if(show) {
				text.transform.localPosition = new Vector3(0,
					Mathf.SmoothDamp(text.transform.localPosition.y, targetHeight, ref cv1, 0.2f)
				, 0);
				text.color = new Color(0, 0.8f, 1f,
					Mathf.Lerp(text.color.a, 1, Time.deltaTime * 5)
				);
			} else {
				text.transform.localPosition = new Vector3(0,
						Mathf.SmoothDamp(text.transform.localPosition.y, targetHeight * 3, ref cv1, 0.1f)
					, 0);
				text.color = new Color(0, 0.8f, 1f,
					Mathf.Lerp(text.color.a, 0, Time.deltaTime * 10)
				);
			}

			tool.color = new Color(tool.color.r, tool.color.g, tool.color.b, text.color.a);
			tool.material.SetColor("_SolidOutline", new Color(0, 0, 0, text.color.a));

			tool.transform.localPosition = new Vector3(
				tool.transform.localPosition.x,
				text.transform.localPosition.y + 0.35f
			, 0);

		}
	}
}
