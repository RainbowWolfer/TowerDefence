using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefence.Data;
using UnityEngine;

namespace TowerDefence.Towers {
	public class Placement: MonoBehaviour {
		public TowerInfo info;
		public float Height => info.height;
		public Vector2Int Size => info.size;

		public Vector2Int coord;
	}
}
