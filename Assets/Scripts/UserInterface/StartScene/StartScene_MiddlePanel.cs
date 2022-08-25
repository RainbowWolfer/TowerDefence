
using System;
using System.Collections.Generic;
using TowerDefence.Scripts.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerDefence.UserInterface.StartScene {
	public class StartScene_MiddlePanel: MonoBehaviour {
		public RectTransform Rt => transform as RectTransform;
		private StartScene_BottomPanel BottomPanel => StartSceneUI.Instance.BottomPanel;


		[SerializeField]
		private GameObject leftClickArea;
		[SerializeField]
		private GameObject middleClickArae;
		[SerializeField]
		private GameObject rightClickArea;

		[field: Space]
		[field: SerializeField]
		public bool IsMouseOnLeft { get; private set; }
		[field: SerializeField]
		public bool IsMouseOnMiddle { get; private set; }
		[field: SerializeField]
		public bool IsMouseOnRight { get; private set; }

		[Space]
		[SerializeField]
		private CanvasGroup canvas;

		[Space]
		public RectTransform[] panels;

		[Space]
		public int index;
		public int mouseOnIndex;

		[Space]
		public float x_gap = 460;
		public float z_gap = 500;
		public float angle_y_gap = 10;

		[Space]
		public Range<float> self_z_range = new Range<float>(-200, 700);
		public Range<float> self_y_range = new Range<float>(-30, 500);

		//private float cv1;
		//private float cv2;
		//private float cv3;
		//private float cv4;

		[Space]
		[SerializeField]
		private List<CV> cvs = new List<CV>();
		private float cv1;
		private float cv2;
		private float cv3;

		public bool OnMouseClick => Input.GetMouseButtonUp(0);

		private float timeDelayedAction;

		private void Start() {
			mouseOnIndex = -1;
			foreach(var _ in panels) {
				cvs.Add(new CV());
			}

		}

		private void OnEnable() {
			Rt.anchoredPosition3D = new Vector3(
				Rt.anchoredPosition3D.x,
				Rt.anchoredPosition3D.y,
				-1000
			);
			foreach(RectTransform item in panels) {
				item.GetComponent<CanvasGroup>().alpha = 0;
			}
		}

		public void ResetDelayedActionTimer(float time = 0.2f) {
			timeDelayedAction = time;
		}

		private void Update() {
			bool settingsHide = BottomPanel.panelType == StartScene_BottomPanel.PanelType.None;

			Rt.anchoredPosition3D = new Vector3(
				Rt.anchoredPosition3D.x,
				Mathf.SmoothDamp(
					Rt.anchoredPosition3D.y,
					settingsHide ? self_y_range.from : self_y_range.to,
					ref cv3, 0.1f
				),
				Mathf.SmoothDamp(
					Rt.anchoredPosition3D.z,
					settingsHide ? self_z_range.from : self_z_range.to,
					ref cv1, 0.1f
				)
			);
			canvas.alpha = Mathf.SmoothDamp(
				canvas.alpha,
				settingsHide ? 1 : 0,
				ref cv2, 0.06f
			);

			//panels movements
			for(int i = 0; i < panels.Length; i++) {
				int offset = i - index;
				RectTransform item = panels[i];
				CanvasGroup canvas = item.GetComponent<CanvasGroup>();
				StartScene_Block block = item.GetComponent<StartScene_Block>();

				item.anchoredPosition3D = new Vector3(
					Mathf.SmoothDamp(
						item.anchoredPosition3D.x,
						x_gap * offset,
						ref cvs[i].cv1, 0.1f
					),
					0,
					Mathf.SmoothDamp(
						item.anchoredPosition3D.z,
						Mathf.Abs(z_gap * offset),
						ref cvs[i].cv2, 0.1f
					)
				);
				//item.anchoredPosition3D = Vector3.SmoothDamp(
				//	item.anchoredPosition3D,
				//	new Vector3(x_gap * offset, 0, Mathf.Abs(z_gap * offset)),
				//	ref cv1, 0.1f
				//);

				item.localEulerAngles = new Vector3(0,
					Mathf.SmoothDampAngle(
						item.localEulerAngles.y,
						angle_y_gap * offset,
						ref cvs[i].cv3, 0.1f
					)
				, 0);

				canvas.alpha = Mathf.SmoothDamp(
					canvas.alpha,
					offset == 0 ? 1 : 0.48f,
					ref cvs[i].cv4, 0.1f
				);

				if(offset == 0) {
					item.SetAsLastSibling();
				}

				block.IsMouseOn = mouseOnIndex == i;

			}

			timeDelayedAction -= Time.deltaTime;
			if(timeDelayedAction > 0) {
				return;
			}
			IsMouseOnLeft = UIRayCaster.HasElement(leftClickArea);
			IsMouseOnMiddle = UIRayCaster.HasElement(middleClickArae);
			IsMouseOnRight = UIRayCaster.HasElement(rightClickArea);


			if(!HasPanelOpen() && settingsHide && !BottomPanel.IsMouseOnSettingsButton && !BottomPanel.IsMouseOnUserButton) {
				if(IsMouseOnMiddle) {
					mouseOnIndex = index;
					if(OnMouseClick) {
						panels[index].GetComponent<StartScene_Block>().Open = true;
						BottomPanel.Show = false;
					}
				} else {
					if(IsMouseOnLeft) {
						mouseOnIndex = index - 1;
						if(OnMouseClick) {
							index--;
						}
					} else if(IsMouseOnRight) {
						mouseOnIndex = index + 1;
						if(OnMouseClick) {
							index++;
						}
					} else {
						mouseOnIndex = -1;
					}
				}
			}

			if(Input.GetKeyDown(KeyCode.Escape)) {
				QuitOpen();
			}

			if(Input.GetKeyDown(KeyCode.LeftArrow)) {
				index--;
			} else if(Input.GetKeyDown(KeyCode.RightArrow)) {
				index++;
			}
			index = Mathf.Clamp(index, 0, panels.Length - 1);

		}

		private bool HasPanelOpen() {
			foreach(RectTransform item in panels) {
				var block = item.GetComponent<StartScene_Block>();
				if(block.Open) {
					return true;
				}
			}
			return false;
		}

		private void QuitOpen() {
			BottomPanel.Show = true;
			foreach(RectTransform item in panels) {
				var block = item.GetComponent<StartScene_Block>();
				block.Open = false;
			}
		}

		[Serializable]
		public class CV {
			public float cv1;
			public float cv2;
			public float cv3;
			public float cv4;
			public float cv5;
		}
	}
}
