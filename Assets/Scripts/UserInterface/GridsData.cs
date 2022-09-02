using TowerDefence.Scripts.Data;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence.UserInterface {
	public class GridsData: MonoBehaviour {
		[SerializeField]
		private Image fill1;
		[SerializeField]
		private Image fill2;
		[SerializeField]
		private Image fill3;
		[SerializeField]
		private Image fill4;
		[SerializeField]
		private Image fill5;

		[Space]
		[Range(0, 5)]
		public int data;
		[Range(0, 5)]
		public int upgradeData;

		[Space]
		public bool upgraded;

		private void OnValidate() {
			Set(data, upgradeData);
		}

		public void Set(int data, int upgradeData) {
			Set(new Range<int>(data, upgradeData));
		}

		public void Set(Range<int> range) {
			data = range.from;
			upgradeData = range.to;
			data = Mathf.Clamp(data, 0, 5);
			upgradeData = Mathf.Clamp(upgradeData, data, 5);

			float Alpha(float i) {
				if(upgraded) {
					return upgradeData >= i ? 1 : 0;
				} else {
					return data >= i ? 1 : upgradeData >= i ? 0.2f : 0;
				}
			}
			try {
				fill1.color = new Color(1, 1, 1, Alpha(1));
				fill2.color = new Color(1, 1, 1, Alpha(2));
				fill3.color = new Color(1, 1, 1, Alpha(3));
				fill4.color = new Color(1, 1, 1, Alpha(4));
				fill5.color = new Color(1, 1, 1, Alpha(5));
			} catch { }
		}
	}
}
