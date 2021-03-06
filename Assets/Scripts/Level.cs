using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefence.Data;
using TowerDefence.GameControl;
using TowerDefence.Towers;
using TowerDefence.UserInterface;
using UnityEngine;

namespace TowerDefence {
	public class Level: MonoBehaviour {
		[SerializeField]
		private GameObject startPointCube;//prefab
		private GameObject startPoint;

		[SerializeField]
		private GameObject pathVisual_arrow;
		[SerializeField]
		private GameObject pathVisual_destination;

		[SerializeField]
		private Transform borderParent;
		[SerializeField]
		private Transform visualPathParent;
		[SerializeField]
		private Transform mapParent;

		private MapInfo map;

		[SerializeField]
		private GameObject plane;
		private readonly Dictionary<Vector2Int, GameObject> pool = new Dictionary<Vector2Int, GameObject>();
		private readonly List<GameObject> borders = new List<GameObject>();

		public Vector2Int StartCoord { get; private set; }
		public Path targetPath;

		private int cash;
		public int Cash {
			get => cash;
			set {
				cash = value;
				UI.Instance.financePanel.UpdateCash(value);
			}
		}

		public void Initialize(MapInfo map) {
			this.map = map;
			DrawMap();
			Cash = 1000;
		}


		private void DrawMap() {
			if(map == null) {
				throw new Exception("Map is Null");
			}
			ClearMap();
			for(int x = 0; x < map.nodes.GetLength(0); x++) {
				for(int y = 0; y < map.nodes.GetLength(1); y++) {
					var coord = new Vector2Int(x, y);
					Node node = map.nodes[x, y];
					if(node.type == NodeType.Path) {
						continue;
					}
					pool.Add(coord, CreateGameObject(x, y, node.towerID, mapParent));
				}
			}

			//draw border ; -5 is tanktrap
			foreach(GameObject item in borders) {
				Destroy(item);
			}
			borders.Clear();
			int x_max = map.nodes.GetLength(0) + 1;
			int y_max = map.nodes.GetLength(1) + 1;
			for(int x = -1; x < x_max; x++) {
				for(int y = -1; y < y_max; y++) {
					if(x == -1 || x == x_max - 1 || y == -1 || y == y_max - 1) {
						GameObject obj = CreateGameObject(x, y, -5, borderParent);
						borders.Add(obj);
					}
				}
			}

			if(startPoint != null) {
				Destroy(startPoint);
			}
			if(map.paths.Count > 0 && map.paths[0].path.Count > 0) {
				Path p = map.paths[0];
				targetPath = p;
				Node start = p.path[0];
				StartCoord = start.coord;
				startPoint = Instantiate(startPointCube, transform);
				startPoint.transform.localPosition = new Vector3(start.coord.x, 0, start.coord.y);
			}
		}

		private void ClearMap() {
			foreach(GameObject item in pool.Values) {
				Destroy(item);
			}
			pool.Clear();
		}

		public bool Check(int startX, int startY, Vector2Int size) {
			//Debug.Log(size + " = " + new Vector2Int(startX, startY) + " = " + new Vector2Int(map.nodes.GetLength(0), map.nodes.GetLength(1)));
			if(startX + size.x - 1 >= map.nodes.GetLength(0) || startY + size.y - 1 >= map.nodes.GetLength(1)) {//out of bounds
																												//Debug.LogWarning("OUT OF BOUNDS");
				return false;
			}
			bool result = true;
			for(int x = startX; x < startX + size.x; x++) {
				for(int y = startY; y < startY + size.y; y++) {
					Node node = map.nodes[x, y];
					result &= node.type == NodeType.Placable;
				}
			}
			return result;
		}

		public bool CheckWithin(int x, int y) =>
			x >= 0 &&
			y >= 0 &&
			x < map.nodes.GetLength(0) &&
			y < map.nodes.GetLength(1);
		public bool CheckWithin(Vector2Int coord) => CheckWithin(coord.x, coord.y);

		public bool CheckForPlacable(int x, int y) => CheckWithin(x, y) && map.nodes[x, y].type == NodeType.Placable;
		public bool CheckForPlacable(Vector2Int coord) => CheckWithin(coord.x, coord.y) && CheckForPlacable(coord.x, coord.y);

		public bool CheckForPath(int x, int y) => CheckWithin(x, y) && map.nodes[x, y].type == NodeType.Path;
		public bool CheckForPath(Vector2Int coord) => CheckWithin(coord.x, coord.y) && CheckForPath(coord.x, coord.y);

		public bool CheckForUnplacable(int x, int y) => CheckWithin(x, y) && map.nodes[x, y].type == NodeType.Unplacable;
		public bool CheckForUnplacable(Vector2Int coord) => CheckWithin(coord.x, coord.y) && CheckForUnplacable(coord.x, coord.y);

		public NodeType CheckType(int x, int y) => map.nodes[x, y].type;
		public NodeType CheckType(Vector2Int coord) => CheckType(coord.x, coord.y);

		public void GetHeightAndSize(int x, int y, out float height, out Vector2Int size) {
			TowerInfo info = Game.Towers.RequestByID(map.nodes[x, y].towerID);
			height = info.height;
			size = info.size;
		}
		public void GetHeightAndSize(Vector2Int coord, out float height, out Vector2Int size) {
			GetHeightAndSize(coord.x, coord.y, out height, out size);
		}

		public Vector2Int GetOriginCoord(int x, int y) => map.nodes[x, y].origin;
		public Vector2Int GetOriginCoord(Vector2Int coord) => GetOriginCoord(coord.x, coord.y);

		public Placement GetPlacement(int x, int y) => pool[new Vector2Int(x, y)].GetComponent<Placement>();
		public Placement GetPlacement(Vector2Int coord) => pool[coord].GetComponent<Placement>();


		public void EditNode(int x, int y, short newID) {
			Vector2Int size = Game.Towers.RequestByID(newID).size;
			for(int i = x; i < size.x + x; i++) {
				for(int j = y; j < size.y + y; j++) {
					if(!CheckForPlacable(i, j)) {
						return;
					}
				}
			}
			for(int i = x; i < size.x + x; i++) {
				for(int j = y; j < size.y + y; j++) {
					Node n = map.nodes[i, j];
					n.towerID = newID;
					n.origin = new Vector2Int(x, y);
					if(newID == 0) {
						n.type = NodeType.Placable;
					} else {
						n.type = NodeType.Unplacable;
					}
					Destroy(pool[new Vector2Int(i, j)]);
				}
			}
			var obj = CreateGameObject(x, y, newID, mapParent);
			for(int i = x; i < size.x + x; i++) {
				for(int j = y; j < size.y + y; j++) {
					pool[new Vector2Int(i, j)] = obj;
				}
			}
		}
		public void EditNode(Vector2Int coord, short newID) {
			EditNode(coord.x, coord.y, newID);
		}

		public void ClearNode(int x, int y) {
			Vector2Int size = Game.Towers.RequestByID(map.nodes[x, y].towerID).size;
			//Debug.Log(size);
			for(int i = x; i < size.x + x; i++) {
				for(int j = y; j < size.y + y; j++) {
					if(!CheckForUnplacable(i, j)) {
						return;
					}
				}
			}
			for(int i = x; i < size.x + x; i++) {
				for(int j = y; j < size.y + y; j++) {
					map.nodes[i, j].towerID = 0;
					map.nodes[i, j].type = NodeType.Placable;
					map.nodes[i, j].origin = new Vector2Int(i, j);
					Destroy(pool[new Vector2Int(i, j)]);
					pool[new Vector2Int(i, j)] = CreateGameObject(i, j, 0, mapParent);
				}
			}
		}

		public void ClearNode(Vector2Int coord) {
			ClearNode(coord.x, coord.y);
		}

		private GameObject CreateGameObject(int x, int y, short id, Transform parent) {
			TowerInfo info = Game.Towers.RequestByID(id);
			GameObject prefab = info.prefab;
			GameObject obj = Instantiate(prefab, parent);
			var p = obj.GetComponent<Placement>();
			p.info = info;
			p.coord = new Vector2Int(x, y);
			obj.name = $"OBJ({id}) ({x},{y})";
			obj.transform.localPosition = new Vector3(x, -0.3f, y);
			return obj;
		}

		public void VisualizePath() {
			if(map.paths.Count == 0) {
				return;
			}
			Path path = map.paths[0];
			var list = new List<GameObject>();
			for(int i = 1; i < path.path.Count - 1; i++) {
				Node item = path.path[i];
				Node next = path.path[i + 1];
				Vector2Int direction = next.coord - item.coord;

				GameObject arrow = Instantiate(pathVisual_arrow, visualPathParent);
				arrow.transform.position = new Vector3(item.coord.x, -0.1f, item.coord.y);
				list.Add(arrow);

				if(direction == Vector2Int.down) {
					arrow.transform.eulerAngles = new Vector3(0, 0, 0);
				} else if(direction == Vector2Int.up) {
					arrow.transform.eulerAngles = new Vector3(0, 180, 0);
				} else if(direction == Vector2Int.right) {
					arrow.transform.eulerAngles = new Vector3(0, -90, 0);
				} else if(direction == Vector2Int.left) {
					arrow.transform.eulerAngles = new Vector3(0, 90, 0);
				}
			}
			GameObject des = Instantiate(pathVisual_destination, visualPathParent);
			Vector2Int end = path.path[path.path.Count - 1].coord;
			des.transform.position = new Vector3(end.x, -0.1f, end.y);
			list.Add(des);
			StartCoroutine(VisualizePathCoroutine(list));
		}

		private IEnumerator VisualizePathCoroutine(List<GameObject> list) {
			float gap = 1.5f;
			var target = new List<(GameObject, float, float)>();//object, startTime, targetPosition
			for(int i = 0; i < list.Count; i++) {
				target.Add((list[i], i * gap, 0.01f));
			}
			float time = 0;
			bool Check() {
				foreach(var item in target) {
					if(Mathf.Abs(item.Item1.transform.position.y - item.Item3) > 0.001f) {
						return true;
					}
				}
				return false;
			}
			while(Check()) {
				foreach((GameObject, float, float) item in target) {
					if(time > item.Item2) {
						item.Item1.transform.position = new Vector3(
							item.Item1.transform.position.x,
							Mathf.MoveTowards(item.Item1.transform.position.y, item.Item3, Time.deltaTime * 0.5f),
							item.Item1.transform.position.z);
					}
					time += Time.fixedDeltaTime;
				}
				yield return new WaitForFixedUpdate();
			}
			yield return new WaitForSeconds(2);
			foreach(GameObject item in list) {
				Destroy(item);
			}
		}
	}
}
