using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TowerDefence.Data {
	[CreateAssetMenu(fileName = "Tower Prefabs Data", menuName = "Data/Tower Prefab Data")]
	[Serializable]
	public class TowerInfo: ScriptableObject {
		[HideInInspector]
		public short id;
		public string TowerName => this.name;
		public GameObject prefab;
		public Sprite icon;
		public int price;

		public Vector2Int size = new Vector2Int(1, 1);
		public float height = 1f;
		public bool upgradable = true;
		public int upgradePrice;
		public int upgradedSellPrice;
		public int sellPrice;
		public int shovelPrice;//only used for environment cubes

		public float star1Exp = 1000;
		public float star2Exp = 3000;
		public float star3Exp = 6000;

		public bool abilityEnabled;
		public float rechargeTime;

		public bool enableAntiAir;

		[TextArea()]
		public string description;
	}
}
