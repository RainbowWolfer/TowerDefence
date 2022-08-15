using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TowerDefence.Data {
	[CreateAssetMenu(fileName = "Enemy Prefab Data", menuName = "Data/Enemy Prefab Data")]
	[Serializable]
	public class EnemyInfo: ScriptableObject {
		[HideInInspector]
		public short id;

		public string EnemyName => this.name;
		public GameObject prefab;
		//public Sprite icon;

		public float speed;
		public float health;

	}
}
