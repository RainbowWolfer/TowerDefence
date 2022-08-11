using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.Networking.UnityWebRequest;

namespace TowerDefence.UserInterface {
	public class UIRayCaster: MonoBehaviour {
		[SerializeField]
		private GraphicRaycaster raycaster;
		[SerializeField]
		private EventSystem eventSystem;

		public bool enablePrint;

		public static List<RaycastResult> RayCastObjects { get; } = new List<RaycastResult>();

		private void Update() {
			CalculateUIRayCast();
			if(enablePrint) {
				PrintAllObjects();
			}
		}

		private void PrintAllObjects() {
			Debug.Log($"{RayCastObjects.Count} - {string.Join(", ", RayCastObjects.Select(l => l.gameObject.name).ToArray())}");
		}

		private void CalculateUIRayCast() {
			RayCastObjects.Clear();
			raycaster.Raycast(new PointerEventData(eventSystem) {
				position = Input.mousePosition,
			}, RayCastObjects);
		}

		public static bool HasElement(GameObject obj) {
			return RayCastObjects.Any(i => i.gameObject == obj);
		}

		public static bool HasElements(IEnumerable<GameObject> objs) {
			return RayCastObjects.Any(i => objs.Contains(i.gameObject));
		}
	}
}
