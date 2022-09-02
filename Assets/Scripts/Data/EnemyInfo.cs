using System;
using TowerDefence.Scripts.Data;
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

		public Range speed;
		public Range health;
		public Range coins;

		[TextArea]
		public string description;
	}
}
