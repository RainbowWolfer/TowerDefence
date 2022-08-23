using System.Threading.Tasks;
using TowerDefence.Placements;
using UnityEngine;
using UnityEngine.VFX;

namespace TowerDefence.Effects.Visuals {
	public class LevelUpEffect: MonoBehaviour {
		[SerializeField]
		private Transform textParent;
		[SerializeField]
		private VisualEffect effect;
		[SerializeField]
		private TextOutline outline;
		[SerializeField]
		private TextMesh text;
		[SerializeField]
		private SpriteRenderer[] stars;

		public Star star;

		public bool show;
		public float targetHeight;

		private float cv1;

		private async void Start() {
			text.color = new Color(1, 0.57f, 0, 0f);
			text.transform.localPosition = new Vector3();
			show = true;
			await Task.Delay(3000);
			show = false;
			//await Task.Delay(3000);
			Destroy(gameObject, 3000);
		}

		private void Update() {
			//textParent.LookAt(CameraController.Instance.mainCamera.transform);
			textParent.forward = -CameraController.Instance.mainCamera.transform.forward;

			if(show) {
				text.transform.localPosition = new Vector3(-0.12f,
					Mathf.SmoothDamp(text.transform.localPosition.y, targetHeight, ref cv1, 0.2f)
				, 0);
				text.color = new Color(1, 0.57f, 0,
					Mathf.Lerp(text.color.a, 1, Time.deltaTime * 5)
				);
			} else {
				text.transform.localPosition = new Vector3(-0.12f,
						Mathf.SmoothDamp(text.transform.localPosition.y, targetHeight * 3, ref cv1, 0.1f)
					, 0);
				text.color = new Color(1, 0.57f, 0,
					Mathf.Lerp(text.color.a, 0, Time.deltaTime * 10)
				);
			}

			for(int i = 0; i < stars.Length; i++) {
				stars[i].gameObject.SetActive((int)star > i);
			}

			float star1X = 0;
			float star2X = 0;
			float star3X = 0;
			switch(star) {
				case Star.Star1:
					star1X = 0;
					break;
				case Star.Star2:
					star1X = -0.2f;
					star2X = 0.2f;
					break;
				case Star.Star3:
					star1X = -0.375f;
					star2X = 0;
					star3X = 0.375f;
					break;
			}
			stars[0].transform.localPosition = new Vector3(star1X, stars[0].transform.localPosition.y, 0);
			stars[1].transform.localPosition = new Vector3(star2X, stars[1].transform.localPosition.y, 0);
			stars[2].transform.localPosition = new Vector3(star3X, stars[2].transform.localPosition.y, 0);

			for(int i = 0; i < stars.Length; i++) {
				SpriteRenderer item = stars[i];
				item.color = new Color(item.color.r, item.color.g, item.color.b, text.color.a);
				item.material.SetColor("_SolidOutline", new Color(0, 0, 0, text.color.a));

				item.transform.localPosition = new Vector3(
					item.transform.localPosition.x,
					text.transform.localPosition.y + 0.35f
				, 0);
			}
		}
	}
}
