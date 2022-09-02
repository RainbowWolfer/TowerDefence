using UnityEngine;

namespace TowerDefence.UserInterface.StartScene.Blocks {
	public class StartBlock_EndlessMode: StartScene_Block {
		[Space]
		[SerializeField]
		private DifficultyItem easyItem;
		[SerializeField]
		private DifficultyItem normalItem;
		[SerializeField]
		private DifficultyItem hardItem;

		[Space]
		[SerializeField]
		private RectTransform selectRt;
		[SerializeField]
		private RewardsPanel easyReward;
		[SerializeField]
		private RewardsPanel normalReward;
		[SerializeField]
		private RewardsPanel hardReward;

		[Space]
		[SerializeField]
		private CanvasGroup noneCanvas;
		[SerializeField]
		private CanvasGroup easyCanvas;
		[SerializeField]
		private CanvasGroup normalCanvas;
		[SerializeField]
		private CanvasGroup hardCanvas;

		[Space]
		public DifficultyMode mode = DifficultyMode.None;


		private float s_cv1;
		private float s_cv2;
		private float s_cv3;
		private float s_cv4;

		private float a_cv1;
		private float a_cv2;
		private float a_cv3;
		private float a_cv4;

		protected override void Awake() {
			base.Awake();
		}

		protected override void Update() {
			base.Update();
			if(easyItem.IsSelected) {
				mode = DifficultyMode.Easy;
			} else if(normalItem.IsSelected) {
				mode = DifficultyMode.Normal;
			} else if(hardItem.IsSelected) {
				mode = DifficultyMode.Hard;
			} else {
				mode = DifficultyMode.None;
			}

			selectRt.anchoredPosition = new Vector2(0,
				Mathf.SmoothDamp(
					selectRt.anchoredPosition.y,
					mode == DifficultyMode.None ? 0 : -90,
					ref s_cv1, 0.1f
				)
			);

			noneCanvas.alpha = Mathf.SmoothDamp(
				noneCanvas.alpha,
				mode == DifficultyMode.None ? 1f : 0f,
				ref a_cv1, 0.1f
			);

			easyReward.Rt.anchoredPosition = new Vector2(0,
				Mathf.SmoothDamp(
					easyReward.Rt.anchoredPosition.y,
					mode == DifficultyMode.Easy ? 0 : -90,
					ref s_cv2, 0.1f
				)
			);

			easyCanvas.alpha = Mathf.SmoothDamp(
				easyCanvas.alpha,
				mode == DifficultyMode.Easy ? 1f : 0f,
				ref a_cv2, 0.1f
			);

			normalReward.Rt.anchoredPosition = new Vector2(0,
				Mathf.SmoothDamp(
					normalReward.Rt.anchoredPosition.y,
					mode == DifficultyMode.Normal ? 0 : -90,
					ref s_cv3, 0.1f
				)
			);

			normalCanvas.alpha = Mathf.SmoothDamp(
				normalCanvas.alpha,
				mode == DifficultyMode.Normal ? 1f : 0f,
				ref a_cv3, 0.1f
			);

			hardReward.Rt.anchoredPosition = new Vector2(0,
				Mathf.SmoothDamp(
					hardReward.Rt.anchoredPosition.y,
					mode == DifficultyMode.Hard ? 0 : -90,
					ref s_cv4, 0.1f
				)
			);

			hardCanvas.alpha = Mathf.SmoothDamp(
				hardCanvas.alpha,
				mode == DifficultyMode.Hard ? 1f : 0f,
				ref a_cv4, 0.1f
			);

		}

		public enum DifficultyMode {
			None, Easy, Normal, Hard
		}
	}
}
