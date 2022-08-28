using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace TowerDefence.UserInterface.StartScene {
	[ExecuteInEditMode]
	public class StartScene_Block: MonoBehaviour {
		public RectTransform Rt => transform as RectTransform;

		private StartScene_MiddlePanel MiddlePanel => StartSceneUI.Instance.MiddlePanel;
		private StartScene_BottomPanel BottomPanel => StartSceneUI.Instance.BottomPanel;

		[field: SerializeField]
		public CanvasGroup Canvas { get; private set; }
		[SerializeField]
		private TextMeshProUGUI bottomText;
		[SerializeField]
		private RectTransform imageMask;
		[SerializeField]
		private AboutDetailButton backButton;
		[SerializeField]
		private AboutDetailButton playButton;

		[field: Space]
		[field: SerializeField]
		public bool Open { get; set; }
		[field: SerializeField]
		public bool IsMouseOn { get; set; }

		public string text;
		public Action OnStart;


		private Vector2 cv1;
		private Vector2 cv2;

		protected virtual void Awake() {
			backButton.OnClick = () => {
				MiddlePanel.ResetDelayedActionTimer();
				BottomPanel.Show = true;
				Open = false;
			};
			playButton.OnClick = () => {
				OnStart?.Invoke();
				StartSceneUI.Instance.LoadMainScene();
			};
		}

		protected virtual void Update() {
			Vector2 targetSize;
			Vector2 thumbnailSize;
			if(Open) {
				targetSize = new Vector2(1300, 600);
				thumbnailSize = new Vector2(560, 410);
			} else {
				targetSize = new Vector2(600, IsMouseOn ? 450 : 400);
				if(IsMouseOn) {
					thumbnailSize = new Vector2(560, 410);
				} else {
					thumbnailSize = new Vector2(560, 360);
				}
			}
			imageMask.sizeDelta = Vector2.SmoothDamp(
				imageMask.sizeDelta,
				thumbnailSize,
				ref cv2, 0.1f
			);

			Rt.sizeDelta = Vector2.SmoothDamp(
				Rt.sizeDelta,
				targetSize,
				ref cv1, 0.1f
			);

			bottomText.text = text;

			if(Open) {
				//action for inner content


			}

			//inner content animation
		}

	}
}
