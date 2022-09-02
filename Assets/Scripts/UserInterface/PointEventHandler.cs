using UnityEngine;
using UnityEngine.EventSystems;

namespace TowerDefence.UserInterface {
	public class PointEventHandler: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {
		public bool IsMouseOn { get; private set; }
		public delegate void MyEventHandler(PointEventHandler handler);
		public event MyEventHandler MouseEnter;
		public event MyEventHandler MouseExit;
		public event MyEventHandler MouseDown;
		public event MyEventHandler MouseUp;

		public void OnPointerEnter(PointerEventData eventData) {
			IsMouseOn = true;
			MouseEnter?.Invoke(this);
		}

		public void OnPointerExit(PointerEventData eventData) {
			IsMouseOn = false;
			MouseExit?.Invoke(this);
		}

		public void OnPointerDown(PointerEventData eventData) {
			MouseDown?.Invoke(this);
		}

		public void OnPointerUp(PointerEventData eventData) {
			MouseUp?.Invoke(this);
		}

	}
}
