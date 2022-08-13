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

		[Space]
		public Vector2Int size = new Vector2Int(1, 1);
		public float height = 1f;
		[Space]
		public bool upgradable = true;
		[Space]
		public int price;
		public int upgradePrice;
		public int upgradedSellPrice;
		public int sellPrice;

		public int shovelPrice;//only used for environment cubes

		[Space]
		public float star1Exp = 1000;
		public float star2Exp = 3000;
		public float star3Exp = 6000;

		//public bool abilityEnabled;
		//public float rechargeTime;

		//public bool enableAntiAir;
		[Space]
		public float damageUpgradeMultuplier = 1;
		public float fireRateUpgradeMultuplier = 1;
		public float radiusUpgradeMultuplier = 1;

		[Space]
		public StarValues<float> damage;
		public StarValues<float> fireRate;
		public StarValues<float> attackRadius;
		public StarValues<float> turnnigTime;

		[Space]
		public AbilityData ability;

		[Space]
		[TextArea()]
		public string description;
	}
}
