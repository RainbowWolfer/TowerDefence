using UnityEngine;

namespace TowerDefence {
	public class StartupAction: MonoBehaviour {
		public ActionType actionType;

		private void Start() {
			switch(actionType) {
				case ActionType.Inactive:
					gameObject.SetActive(false);
					break;
				case ActionType.Destory:
					Destroy(gameObject);
					break;
				default:
					break;
			}
		}

		public enum ActionType {
			Inactive, Destory
		}
	}
}
