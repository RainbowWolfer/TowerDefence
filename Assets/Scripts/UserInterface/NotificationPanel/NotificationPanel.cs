using System.Collections.Generic;
using UnityEngine;

namespace TowerDefence.UserInterface {
	public class NotificationPanel: MonoBehaviour {
		public static NotificationPanel Instance { get; private set; }
		[SerializeField]
		private GameObject itemPrefab;

		private readonly List<NotificationItem> items = new List<NotificationItem>();

		private readonly int maxCount = 6;

		[SerializeField]
		private Sprite icon_level;
		[SerializeField]
		private Sprite icon_fund;
		[SerializeField]
		private Sprite icon_diamond;
		[SerializeField]
		private Sprite icon_powers;

		private void Awake() {
			Instance = this;
		}

		private void Start() {
			Clear();
		}

		private void Update() {
			if(Input.GetKeyDown(KeyCode.B)) {
				EarnDiamonds(10);
			}

			for(int i = 0; i < items.Count; i++) {
				NotificationItem item = items[i];
				if(item.fade) {
					continue;
				}
				if(i >= maxCount) {
					item.targetPosotion = new Vector2(0, 700);
					item.fade = true;
				} else {
					item.targetPosotion = new Vector2(0, 60 * i);
				}
			}

		}

		public void Clear() {
			items.Clear();
			for(int i = 0; i < transform.childCount; i++) {
				Destroy(transform.GetChild(i).gameObject);
			}
		}

		public void Add(string text, Sprite sprite, Color color) {
			var item = Instantiate(itemPrefab, transform).GetComponent<NotificationItem>();
			item.Rt.anchoredPosition = new Vector2(item.Rt.sizeDelta.x, 0);
			item.parent = this;
			item.Set(text, sprite, color);
			items.Insert(0, item);
		}

		public void Remove(NotificationItem item) {
			if(items.Contains(item)) {
				items.Remove(item);
			}
			Destroy(item.gameObject);
		}

		//Static Quick Functions
		public static void InsufficientFund() {
			if(Instance == null) {
				return;
			}
			Instance.Add("Insufficient Fund", Instance.icon_fund, new Color(1, 0.6f, 0, 0.3f));
		}

		//public static void TowerLevelUp() {
		//	if(Instance == null) {
		//		return;
		//	}
		//	Instance.Add("Tower Level Up", Instance.icon_level, new Color(0, 0.85f, 1, 0.3f));
		//}

		public static void EarnDiamonds(int amount) {
			if(Instance == null) {
				return;
			}
			Instance.Add($"Diamonds Earned : {amount}", Instance.icon_diamond, new Color(0, 0.85f, 1, 0.3f));
		}

		public static void LowPowers() {
			if(Instance == null) {
				return;
			}
			Instance.Add($"Low Power", Instance.icon_powers, new Color(1, 0, 0, 0.3f));
		}
	}
}
