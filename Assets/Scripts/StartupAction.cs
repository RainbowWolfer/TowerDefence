using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts {
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
