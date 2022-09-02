using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TowerDefence {
	[Serializable]
	public class MapInfo {
		public string Name { get; private set; }
		public Node[,] Nodes { get; private set; }
		public List<Path> Paths { get; private set; }

		public MapInfo(string name, int width, int height) {
			Name = name;
			Nodes = new Node[width, height];
			Paths = new List<Path>();
		}

		public static MapInfo GetDefaultMapInfo() {
			var map = new MapInfo("Default", 20, 10);
			for(int x = 0; x < map.Nodes.GetLength(0); x++) {
				for(int y = 0; y < map.Nodes.GetLength(1); y++) {
					map.Nodes[x, y] = new Node(x, y) {
						type = (NodeType)Random.Range(0, 2),
						towerID = (short)Random.Range(0, 3),
					};
				}
			}
			return map;
		}

		public static MapInfo GenerateRandomMap(int width, int height) {
			width /= 2;
			height /= 2;
			FNode[,] nodes = new FNode[width, height];
			var unvisited = new List<FNode>();
			for(int x = 0; x < width; x++) {
				for(int y = 0; y < height; y++) {
					nodes[x, y] = new FNode(x, y);
					unvisited.Add(nodes[x, y]);
				}
			}

			bool IsInRange(Vector2Int c) => c.x >= 0 && c.y >= 0 && c.x < width && c.y < height;
			FNode GetFNode(Vector2Int c) => IsInRange(c) ? nodes[c.x, c.y] : null;
			FNode FindNextAvailable(Vector2Int coord) {
				FNode left = GetFNode(coord + new Vector2Int(-1, 0));
				FNode right = GetFNode(coord + new Vector2Int(1, 0));
				FNode up = GetFNode(coord + new Vector2Int(0, -1));
				FNode down = GetFNode(coord + new Vector2Int(0, 1));
				var randomList = new List<FNode>();
				foreach(FNode item in new FNode[] { left, right, up, down }) {
					if(item == null) {
						continue;
					}
					if(unvisited.Contains(item)) {
						randomList.Add(item);
					}
				}
				if(randomList.Count == 0) {
					return null;
				} else if(randomList.Count == 1) {
					return randomList[0];
				} else {
					return randomList[Random.Range(0, randomList.Count)];
				}
			}
			FNode BackTrace(FNode node) {
				FNode next = FindNextAvailable(node.previous.coord);
				if(next != null) {
					return node.previous;
				}
				while(next == null) {
					node = node.previous;
					next = FindNextAvailable(node.coord);
				}
				return node;
			}

			FNode start;
			if(Random.Range(0, 2) == 1) {
				start = nodes[0, Random.Range(0, (int)(height * 0.4f))];
			} else {
				start = nodes[Random.Range(0, (int)(width * 0.4f)), 0];
			}
			unvisited.Remove(start);

			FNode next = FindNextAvailable(start.coord);
			next.previous = start;
			while(next != null) {
				FNode current = next;
				if(unvisited.Contains(current)) {
					unvisited.Remove(current);
				}

				next = FindNextAvailable(next.coord);
				if(next != null) {
					next.previous = current;
				} else {
					if(unvisited.Count <= 0) {
						break;
					}
					next = BackTrace(current);
				}
			}

			FNode end = nodes[Random.Range((int)(width * 0.8f), width), Random.Range((int)(height * 0.8f), height)];

			var path = new List<FNode>() { end };
			while(end != null) {
				end = end.previous;
				path.Add(end);
			}
			path.RemoveAt(path.Count - 1);//last one is always null

			FNode[,] result = new FNode[width * 2 - 1, height * 2 - 1];
			for(int x = 0; x < width; x++) {
				for(int y = 0; y < height; y++) {
					result[x * 2, y * 2] = nodes[x, y];
				}
			}
			width = width * 2 - 1;
			height = height * 2 - 1;

			for(int x = 0; x < width; x++) {
				for(int y = 0; y < height; y++) {
					if(result[x, y] != null) {
						result[x, y].coord *= 2;
						continue;
					}
					result[x, y] = new FNode(x, y);
				}
			}
			path.Reverse();
			var tmp = path.ToList();
			for(int i = 0; i < path.Count - 1; i++) {
				FNode from = path[i];
				FNode to = path[i + 1];
				Vector2Int direction = (to.coord - from.coord) / 2;
				Vector2Int wayCoord = from.coord + direction;
				tmp.Insert(tmp.IndexOf(to), result[wayCoord.x, wayCoord.y]);
			}

			path = tmp;

			CalculateWeight(result, path);
			bool NoiseCheck(int x, int y) {
				float noiseThreshold;
				return true;
			}
			float threashold = 0.9f;
			var map = new MapInfo("Random", width, height);
			for(int x = 0; x < width; x++) {
				for(int y = 0; y < height; y++) {
					FNode n = result[x, y];
					short id = (short)(n.weight >= threashold && Random.Range(0, 100) < 85 ? Random.Range(-4, 0) : 0);
					NodeType type = path.Contains(n) ? NodeType.Path : NodeType.Placable;
					if(id < 0) {
						type = NodeType.Unplacable;
					}

					map.Nodes[x, y] = new Node(x, y) {
						towerID = id,
						type = type,
					};
				}
			}
			Path p = new Path();
			foreach(FNode item in path) {
				p.path.Add(map.Nodes[item.coord.x, item.coord.y]);
			}
			map.Paths.Add(p);
			return map;
		}

		private class FNode {
			public Vector2Int coord;
			//public Node node;
			//public bool isPath;
			public float weight;
			public FNode previous;
			public FNode(Vector2Int coord) {
				this.coord = coord;
			}
			public FNode(int x, int y) {
				this.coord = new Vector2Int(x, y);
			}

			public override string ToString() {
				return $"FNode : {coord}";
			}
		}

		private static void CalculateWeight(FNode[,] nodes, List<FNode> path) {
			bool IsInRange(Vector2Int coord) => coord.x >= 0 && coord.y >= 0 && coord.x < nodes.GetLength(0) && coord.y < nodes.GetLength(1);
			List<FNode> GetNeighbours(FNode n, int range = 3) {
				var result = new List<FNode>();
				for(int x = -range; x < range; x++) {
					for(int y = -range; y < range; y++) {
						var coord = n.coord + new Vector2Int(x, y);
						if(IsInRange(coord)) {
							result.Add(nodes[coord.x, coord.y]);
						}
					}
				}
				return result;
			}

			for(int x = 0; x < nodes.GetLength(0); x++) {
				for(int y = 0; y < nodes.GetLength(1); y++) {
					FNode n = nodes[x, y];
					int validCount = 0;
					int totalCount = 0;
					foreach(FNode item in GetNeighbours(n)) {
						if(!path.Contains(item)) {
							validCount++;
						}
						totalCount++;
					}
					n.weight = validCount / (float)totalCount;
				}
			}
		}

		private void AssignEnvironment() {

		}
	}

	[Serializable]
	public class Path {
		public List<Node> path;
		public int Length => path.Count;
		public Node Get(int index) => path[index];
		public Path() {
			path = new List<Node>();
		}
	}

	[Serializable]
	public class Node {
		public Vector2Int Coord { get; private set; }
		public Vector2Int Origin { get; set; }

		public NodeType type;

		public short towerID;

		public Node(Vector2Int coord) {
			Coord = coord;
			Origin = coord;
		}

		public Node(int x, int y) {
			Coord = new Vector2Int(x, y);
			Origin = new Vector2Int(x, y);
		}

		public Node(Vector2Int coord, Vector2Int origin) {
			Coord = coord;
			Origin = origin;
		}
		public Node(int x, int y, int x_origin, int y_origin) {
			Coord = new Vector2Int(x, y);
			Origin = new Vector2Int(x_origin, y_origin);
		}
	}

	[Serializable]
	public enum NodeType {
		Path, Placable, Unplacable
	}

}
